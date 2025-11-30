using Email.Server.DTOs.Responses.Billing;
using Email.Server.Models;

namespace Email.Server.Services.Interfaces;

public interface IUsageTrackingService
{
    Task RecordEmailSendAsync(Guid tenantId, int emailCount, string source, CancellationToken ct = default);
    Task<UsageLimitCheckResult> CheckUsageLimitAsync(Guid tenantId, int requestedCount, CancellationToken ct = default);
    Task ReportOverageToStripeAsync(CancellationToken ct = default);
    Task<UsagePeriods> GetOrCreateCurrentPeriodAsync(Guid tenantId, CancellationToken ct = default);
}

public class UsageLimitCheckResult
{
    public bool Allowed { get; set; }
    public bool IsOverage { get; set; }
    public long CurrentUsage { get; set; }
    public long IncludedLimit { get; set; }
    public long RemainingIncluded { get; set; }
    public string? DenialReason { get; set; }
}
