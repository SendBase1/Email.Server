using Email.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Email.Server.Controllers;

[ApiController]
[Route("api/v1/webhooks/stripe")]
public class StripeWebhooksController(
    IStripeWebhookService webhookService,
    ILogger<StripeWebhooksController> logger) : ControllerBase
{
    private readonly IStripeWebhookService _webhookService = webhookService;
    private readonly ILogger<StripeWebhooksController> _logger = logger;

    /// <summary>
    /// Handle incoming Stripe webhook events
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> HandleWebhook(CancellationToken ct)
    {
        string payload;
        using (var reader = new StreamReader(HttpContext.Request.Body))
        {
            payload = await reader.ReadToEndAsync(ct);
        }

        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault();

        if (string.IsNullOrEmpty(signature))
        {
            _logger.LogWarning("Stripe webhook received without signature");
            return BadRequest(new { error = "Missing Stripe signature" });
        }

        try
        {
            var success = await _webhookService.ProcessWebhookAsync(payload, signature, ct);

            if (success)
            {
                return Ok(new { received = true });
            }
            else
            {
                // Still return 200 to acknowledge receipt - Stripe will retry on 4xx/5xx
                // Processing errors are logged and stored for investigation
                return Ok(new { received = true, processed = false });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing Stripe webhook");
            // Return 200 to prevent infinite retries - error is logged
            return Ok(new { received = true, error = true });
        }
    }
}
