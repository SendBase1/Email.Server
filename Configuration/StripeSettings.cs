namespace Email.Server.Configuration;

public class StripeSettings
{
    public const string SectionName = "Stripe";

    public required string SecretKey { get; set; }
    public required string PublishableKey { get; set; }
    public required string WebhookSecret { get; set; }
    public string? MeterId { get; set; }
}
