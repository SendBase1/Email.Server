using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class UsagePeriods
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [ForeignKey("Subscription")]
    public Guid? SubscriptionId { get; set; }

    public DateTime PeriodStart { get; set; }

    public DateTime PeriodEnd { get; set; }

    public long EmailsSent { get; set; }

    public long IncludedEmailsLimit { get; set; }

    public long OverageEmails { get; set; }

    public long OverageReportedToStripe { get; set; }

    public DateTime? LastStripeReportUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenants? Tenant { get; set; }

    public TenantSubscriptions? Subscription { get; set; }
}
