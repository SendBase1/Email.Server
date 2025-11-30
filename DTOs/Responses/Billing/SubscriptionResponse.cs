namespace Email.Server.DTOs.Responses.Billing;

public class SubscriptionResponse
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public required string Status { get; set; }
    public required BillingPlanResponse Plan { get; set; }
    public DateTime CurrentPeriodStart { get; set; }
    public DateTime CurrentPeriodEnd { get; set; }
    public bool CancelAtPeriodEnd { get; set; }
    public DateTime? CanceledAt { get; set; }
    public bool IsInGracePeriod { get; set; }
    public DateTime? GracePeriodEndsAt { get; set; }
    public bool SendingEnabled { get; set; }
}
