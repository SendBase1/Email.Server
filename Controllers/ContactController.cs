using Email.Server.Data;
using Email.Server.DTOs.Requests;
using Email.Server.Models;
using Email.Server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Email.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController(
    ApplicationDbContext dbContext,
    ISystemEmailService systemEmailService,
    ILogger<ContactController> logger) : ControllerBase
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly ISystemEmailService _systemEmailService = systemEmailService;
    private readonly ILogger<ContactController> _logger = logger;

    /// <summary>
    /// Submit a contact form message
    /// </summary>
    [HttpPost]
    [EnableRateLimiting("ContactForm")]
    public async Task<IActionResult> Submit([FromBody] ContactRequest request, CancellationToken cancellationToken)
    {
        try
        {
            // Save to database
            var submission = new ContactSubmissions
            {
                Email = request.Email,
                Message = request.Message,
                IpAddress = HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = Request.Headers.UserAgent.ToString().Length > 500
                    ? Request.Headers.UserAgent.ToString()[..500]
                    : Request.Headers.UserAgent.ToString(),
                CreatedAtUtc = DateTime.UtcNow,
                Status = ContactSubmissionStatus.New
            };

            _dbContext.ContactSubmissions.Add(submission);
            await _dbContext.SaveChangesAsync(cancellationToken);

            // Send email notification
            try
            {
                await _systemEmailService.SendContactFormEmailAsync(
                    request.Email,
                    request.Message,
                    cancellationToken);
            }
            catch (Exception emailEx)
            {
                // Log but don't fail - the submission is already saved
                _logger.LogWarning(emailEx, "Failed to send contact form notification email, but submission was saved");
            }

            _logger.LogInformation("Contact form submitted from {Email}, Id: {Id}", request.Email, submission.Id);
            return Ok(new { message = "Message sent successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to process contact form submission from {Email}", request.Email);
            return StatusCode(500, new { error = "Failed to send message. Please try again later." });
        }
    }
}
