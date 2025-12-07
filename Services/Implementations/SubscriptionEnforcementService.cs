using Email.Server.Configuration;
using Email.Server.Data;
using Email.Server.DTOs.Responses.Billing;
using Email.Server.Models;
using Email.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Email.Server.Services.Implementations;

public class SubscriptionEnforcementService : ISubscriptionEnforcementService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SubscriptionEnforcementService> _logger;
    private readonly BillingSettings _billingSettings;

    // Value indicating unlimited
    private const int Unlimited = -1;

    public SubscriptionEnforcementService(
        ApplicationDbContext context,
        ILogger<SubscriptionEnforcementService> logger,
        IOptions<BillingSettings> billingSettings)
    {
        _context = context;
        _logger = logger;
        _billingSettings = billingSettings.Value;
    }

    public async Task<bool> CanAddApiKeyAsync(Guid tenantId, CancellationToken ct = default)
    {
        var limits = await GetCurrentLimitsAsync(tenantId, ct);
        return limits.MaxApiKeys == Unlimited || limits.CurrentApiKeys < limits.MaxApiKeys;
    }

    public async Task<bool> CanAddDomainAsync(Guid tenantId, CancellationToken ct = default)
    {
        var limits = await GetCurrentLimitsAsync(tenantId, ct);
        return limits.MaxDomains == Unlimited || limits.CurrentDomains < limits.MaxDomains;
    }

    public async Task<bool> CanAddTeamMemberAsync(Guid tenantId, CancellationToken ct = default)
    {
        var limits = await GetCurrentLimitsAsync(tenantId, ct);
        return limits.MaxTeamMembers == Unlimited || limits.CurrentTeamMembers < limits.MaxTeamMembers;
    }

    public async Task<bool> CanAddWebhookAsync(Guid tenantId, CancellationToken ct = default)
    {
        var limits = await GetCurrentLimitsAsync(tenantId, ct);
        return limits.MaxWebhooks == Unlimited || limits.CurrentWebhooks < limits.MaxWebhooks;
    }

    public async Task<bool> CanAddTemplateAsync(Guid tenantId, CancellationToken ct = default)
    {
        var limits = await GetCurrentLimitsAsync(tenantId, ct);
        return limits.MaxTemplates == Unlimited || limits.CurrentTemplates < limits.MaxTemplates;
    }

    public async Task<bool> CanSendEmailsAsync(
        Guid tenantId,
        int emailCount,
        CancellationToken ct = default)
    {
        var tenant = await _context.Tenants.FindAsync([tenantId], ct);
        if (tenant == null)
            return false;

        // Check if sending is disabled
        if (tenant.SendingDisabledAt != null)
            return false;

        var subscription = await _context.TenantSubscriptions
            .Include(s => s.BillingPlan)
            .FirstOrDefaultAsync(s => s.TenantId == tenantId, ct);

        // No subscription = no sending
        if (subscription == null)
            return false;

        // Check subscription status
        if (subscription.Status is SubscriptionStatus.Canceled or SubscriptionStatus.Unpaid)
            return false;

        // Get current usage
        var now = DateTime.UtcNow;
        var currentUsage = await _context.UsagePeriods
            .Where(p => p.TenantId == tenantId && p.PeriodStart <= now && p.PeriodEnd > now)
            .Select(p => p.EmailsSent)
            .FirstOrDefaultAsync(ct);

        var includedLimit = subscription.BillingPlan?.IncludedEmails ?? 0;
        var allowsOverage = subscription.BillingPlan?.AllowsOverage ?? false;

        // Check if within limits or overage allowed
        if (currentUsage + emailCount <= includedLimit)
            return true;

        return allowsOverage;
    }

    public async Task<PlanLimitsResponse> GetCurrentLimitsAsync(
        Guid tenantId,
        CancellationToken ct = default)
    {
        var subscription = await _context.TenantSubscriptions
            .Include(s => s.BillingPlan)
            .FirstOrDefaultAsync(s => s.TenantId == tenantId, ct);

        var plan = subscription?.BillingPlan;

        // Get current counts
        var apiKeyCount = await _context.ApiKeys
            .CountAsync(k => k.TenantId == tenantId && !k.IsRevoked, ct);

        var domainCount = await _context.Domains
            .CountAsync(d => d.TenantId == tenantId, ct);

        // Count members directly in the tenant
        var teamMemberCount = await _context.TenantMembers
            .CountAsync(m => m.TenantId == tenantId, ct);

        var webhookCount = await _context.WebhookEndpoints
            .CountAsync(w => w.TenantId == tenantId && w.Enabled, ct);

        // Templates might not exist yet - use zero
        var templateCount = 0;

        // Get current usage
        var now = DateTime.UtcNow;
        var currentUsage = await _context.UsagePeriods
            .Where(p => p.TenantId == tenantId && p.PeriodStart <= now && p.PeriodEnd > now)
            .Select(p => p.EmailsSent)
            .FirstOrDefaultAsync(ct);

        // Default to zero limits if no plan
        var maxApiKeys = plan?.MaxApiKeys ?? 0;
        var maxDomains = plan?.MaxDomains ?? 0;
        var maxTeamMembers = plan?.MaxTeamMembers ?? 0;
        var maxWebhooks = plan?.MaxWebhooks ?? 0;
        var maxTemplates = plan?.MaxTemplates ?? 0;
        var includedEmails = plan?.IncludedEmails ?? 0;

        return new PlanLimitsResponse
        {
            PlanName = plan?.Name ?? "No Plan",

            // Current counts
            CurrentApiKeys = apiKeyCount,
            CurrentDomains = domainCount,
            CurrentTeamMembers = teamMemberCount,
            CurrentWebhooks = webhookCount,
            CurrentTemplates = templateCount,

            // Plan limits
            MaxApiKeys = maxApiKeys,
            MaxDomains = maxDomains,
            MaxTeamMembers = maxTeamMembers,
            MaxWebhooks = maxWebhooks,
            MaxTemplates = maxTemplates,

            // Remaining (-1 = unlimited)
            RemainingApiKeys = maxApiKeys == Unlimited ? Unlimited : Math.Max(0, maxApiKeys - apiKeyCount),
            RemainingDomains = maxDomains == Unlimited ? Unlimited : Math.Max(0, maxDomains - domainCount),
            RemainingTeamMembers = maxTeamMembers == Unlimited ? Unlimited : Math.Max(0, maxTeamMembers - teamMemberCount),
            RemainingWebhooks = maxWebhooks == Unlimited ? Unlimited : Math.Max(0, maxWebhooks - webhookCount),
            RemainingTemplates = maxTemplates == Unlimited ? Unlimited : Math.Max(0, maxTemplates - templateCount),

            // Usage
            EmailsSentThisPeriod = currentUsage,
            IncludedEmailsLimit = includedEmails,
            AllowsOverage = plan?.AllowsOverage ?? false,
            OverageRateCentsPer1K = plan?.OverageRateCentsPer1K ?? 0
        };
    }

    public async Task StartGracePeriodAsync(
        Guid tenantId,
        string reason,
        CancellationToken ct = default)
    {
        var tenant = await _context.Tenants.FindAsync([tenantId], ct);
        if (tenant == null)
            return;

        if (tenant.IsInGracePeriod)
        {
            _logger.LogDebug("Tenant {TenantId} already in grace period", tenantId);
            return;
        }

        tenant.IsInGracePeriod = true;
        tenant.GracePeriodEndsAt = DateTime.UtcNow.AddDays(_billingSettings.GracePeriodDays);

        await _context.SaveChangesAsync(ct);

        _logger.LogWarning(
            "Started {Days}-day grace period for tenant {TenantId}. Reason: {Reason}. Ends at: {EndsAt}",
            _billingSettings.GracePeriodDays, tenantId, reason, tenant.GracePeriodEndsAt);
    }

    public async Task EndGracePeriodAsync(Guid tenantId, CancellationToken ct = default)
    {
        var tenant = await _context.Tenants.FindAsync([tenantId], ct);
        if (tenant == null)
            return;

        if (!tenant.IsInGracePeriod)
            return;

        tenant.IsInGracePeriod = false;
        tenant.GracePeriodEndsAt = null;

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Ended grace period for tenant {TenantId}", tenantId);
    }

    public async Task DisableSendingAsync(
        Guid tenantId,
        string reason,
        CancellationToken ct = default)
    {
        var tenant = await _context.Tenants.FindAsync([tenantId], ct);
        if (tenant == null)
            return;

        if (tenant.SendingDisabledAt != null)
        {
            _logger.LogDebug("Sending already disabled for tenant {TenantId}", tenantId);
            return;
        }

        tenant.SendingDisabledAt = DateTime.UtcNow;
        tenant.SendingDisabledReason = reason.Length > 500 ? reason[..500] : reason;

        await _context.SaveChangesAsync(ct);

        _logger.LogWarning(
            "Disabled sending for tenant {TenantId}. Reason: {Reason}",
            tenantId, reason);
    }

    public async Task EnableSendingAsync(Guid tenantId, CancellationToken ct = default)
    {
        var tenant = await _context.Tenants.FindAsync([tenantId], ct);
        if (tenant == null)
            return;

        if (tenant.SendingDisabledAt == null)
            return;

        tenant.SendingDisabledAt = null;
        tenant.SendingDisabledReason = null;

        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Enabled sending for tenant {TenantId}", tenantId);
    }
}
