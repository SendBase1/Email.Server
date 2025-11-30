namespace Email.Server.Services.Interfaces;

public interface IStripeWebhookService
{
    Task<bool> ProcessWebhookAsync(string payload, string signature, CancellationToken ct = default);
}
