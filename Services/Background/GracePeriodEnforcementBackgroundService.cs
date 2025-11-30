using Email.Server.Data;
using Email.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Email.Server.Services.Background;

public class GracePeriodEnforcementBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<GracePeriodEnforcementBackgroundService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(15);

    public GracePeriodEnforcementBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<GracePeriodEnforcementBackgroundService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Grace Period Enforcement Background Service started");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await EnforceGracePeriodsAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in grace period enforcement task");
                }

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Expected during shutdown, ignore
        }

        _logger.LogInformation("Grace Period Enforcement Background Service stopped");
    }

    private async Task EnforceGracePeriodsAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var enforcementService = scope.ServiceProvider.GetRequiredService<ISubscriptionEnforcementService>();

        _logger.LogDebug("Running grace period enforcement task");

        // Find tenants with expired grace periods
        var now = DateTime.UtcNow;
        var expiredGracePeriods = await context.Tenants
            .Where(t =>
                t.IsInGracePeriod &&
                t.GracePeriodEndsAt != null &&
                t.GracePeriodEndsAt < now &&
                t.SendingDisabledAt == null)
            .ToListAsync(ct);

        foreach (var tenant in expiredGracePeriods)
        {
            try
            {
                _logger.LogWarning(
                    "Grace period expired for tenant {TenantId}. Disabling sending.",
                    tenant.Id);

                await enforcementService.DisableSendingAsync(
                    tenant.Id,
                    "Grace period expired due to unpaid invoice",
                    ct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to disable sending for tenant {TenantId}",
                    tenant.Id);
            }
        }

        if (expiredGracePeriods.Count > 0)
        {
            _logger.LogInformation(
                "Processed {Count} expired grace periods",
                expiredGracePeriods.Count);
        }
    }
}
