using Email.Server.DTOs.Requests;
using Email.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Email.Server.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class EmailsController : ControllerBase
{
    private readonly IEmailSendingService _emailService;
    private readonly ILogger<EmailsController> _logger;

    public EmailsController(IEmailSendingService emailService, ILogger<EmailsController> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost("send")]
    public async Task<IActionResult> SendEmail([FromBody] SendEmailRequest request, CancellationToken cancellationToken)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _emailService.SendEmailAsync(request, cancellationToken);

            if (result.Status == 2) // Failed
            {
                return BadRequest(new { error = result.Error, messageId = result.MessageId });
            }

            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email");
            return StatusCode(500, new { error = "An error occurred while sending the email" });
        }
    }
}
