using Email.Server.Configuration;
using Email.Server.Services.Interfaces;
using Microsoft.Extensions.Options;

namespace Email.Server.Services.Background;

public class UsageReportingBackgroundService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<UsageReportingBackgroundService> _logger;
    private readonly BillingSettings _billingSettings;

    public UsageReportingBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<UsageReportingBackgroundService> logger,
        IOptions<BillingSettings> billingSettings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _billingSettings = billingSettings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Usage Reporting Background Service started");

        try
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ReportUsageAsync(stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in usage reporting background task");
                }

                // Wait for configured interval (default 5 minutes)
                var delay = TimeSpan.FromMinutes(_billingSettings.UsageReportingIntervalMinutes);
                await Task.Delay(delay, stoppingToken);
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            // Expected during shutdown, ignore
        }

        _logger.LogInformation("Usage Reporting Background Service stopped");
    }

    private async Task ReportUsageAsync(CancellationToken ct)
    {
        using var scope = _serviceProvider.CreateScope();
        var usageService = scope.ServiceProvider.GetRequiredService<IUsageTrackingService>();

        _logger.LogDebug("Running usage reporting task");

        try
        {
            await usageService.ReportOverageToStripeAsync(ct);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to report overage to Stripe");
        }
    }
}
