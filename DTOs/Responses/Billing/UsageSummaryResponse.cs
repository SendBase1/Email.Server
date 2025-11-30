namespace Email.Server.DTOs.Responses.Billing;

public class UsageSummaryResponse
{
    public Guid PeriodId { get; set; }
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public long EmailsSent { get; set; }
    public long IncludedEmailsLimit { get; set; }
    public long OverageEmails { get; set; }
    public long RemainingIncluded { get; set; }
    public decimal UsagePercentage { get; set; }
    public int EstimatedOverageCostCents { get; set; }
    public bool IsCurrentPeriod { get; set; }
}
