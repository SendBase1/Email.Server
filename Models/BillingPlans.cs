using System.ComponentModel.DataAnnotations;

namespace Email.Server.Models;

public class BillingPlans
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(50)]
    public required string Name { get; set; }

    [MaxLength(100)]
    public string? DisplayName { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Required]
    [MaxLength(100)]
    public required string StripePriceId { get; set; }

    [MaxLength(100)]
    public string? StripeMeteredPriceId { get; set; }

    [MaxLength(100)]
    public string? StripeProductId { get; set; }

    [MaxLength(200)]
    public string? StripePaymentLinkUrl { get; set; }

    public int MonthlyPriceCents { get; set; }

    public int IncludedEmails { get; set; }

    public int OverageRateCentsPer1K { get; set; }

    public bool AllowsOverage { get; set; }

    public int MaxApiKeys { get; set; }

    public int MaxDomains { get; set; }

    public int MaxTeamMembers { get; set; }

    public int MaxWebhooks { get; set; }

    public int MaxTemplates { get; set; }

    public int AnalyticsRetentionDays { get; set; }

    public bool HasDedicatedIp { get; set; }

    [MaxLength(50)]
    public string? SupportLevel { get; set; }

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;
}
