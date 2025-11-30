namespace Email.Server.DTOs.Responses.Billing;

public class BillingPlanResponse
{
    public Guid Id { get; set; }
    public required string Name { get; set; }
    public required string DisplayName { get; set; }
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
    public required string SupportLevel { get; set; }
    public string? StripePaymentLinkUrl { get; set; }
}
