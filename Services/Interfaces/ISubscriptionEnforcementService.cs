using Email.Server.DTOs.Responses.Billing;

namespace Email.Server.Services.Interfaces;

public interface ISubscriptionEnforcementService
{
    Task<bool> CanAddApiKeyAsync(Guid tenantId, CancellationToken ct = default);
    Task<bool> CanAddDomainAsync(Guid tenantId, CancellationToken ct = default);
    Task<bool> CanAddTeamMemberAsync(Guid tenantId, CancellationToken ct = default);
    Task<bool> CanAddWebhookAsync(Guid tenantId, CancellationToken ct = default);
    Task<bool> CanAddTemplateAsync(Guid tenantId, CancellationToken ct = default);
    Task<bool> CanSendEmailsAsync(Guid tenantId, int emailCount, CancellationToken ct = default);
    Task<PlanLimitsResponse> GetCurrentLimitsAsync(Guid tenantId, CancellationToken ct = default);
    Task StartGracePeriodAsync(Guid tenantId, string reason, CancellationToken ct = default);
    Task EndGracePeriodAsync(Guid tenantId, CancellationToken ct = default);
    Task DisableSendingAsync(Guid tenantId, string reason, CancellationToken ct = default);
    Task EnableSendingAsync(Guid tenantId, CancellationToken ct = default);
}
