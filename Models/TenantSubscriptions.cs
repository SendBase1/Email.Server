using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public enum SubscriptionStatus : byte
{
    Trialing = 0,
    Active = 1,
    PastDue = 2,
    Canceled = 3,
    Unpaid = 4,
    Paused = 5
}

public class TenantSubscriptions
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [ForeignKey("BillingPlan")]
    public Guid BillingPlanId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string StripeSubscriptionId { get; set; }

    [MaxLength(100)]
    public string? StripeCustomerId { get; set; }

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;

    public DateTime CurrentPeriodStart { get; set; }

    public DateTime CurrentPeriodEnd { get; set; }

    public DateTime? TrialStart { get; set; }

    public DateTime? TrialEnd { get; set; }

    public DateTime? CanceledAt { get; set; }

    public DateTime? CancelAt { get; set; }

    public bool CancelAtPeriodEnd { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenants? Tenant { get; set; }

    public BillingPlans? BillingPlan { get; set; }
}
