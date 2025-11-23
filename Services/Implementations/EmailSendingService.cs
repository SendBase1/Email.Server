using Amazon.SimpleEmailV2.Model;
using Email.Server.Data;
using Email.Server.DTOs.Requests;
using Email.Server.DTOs.Responses;
using Email.Server.Models;
using Email.Server.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Email.Server.Services.Implementations;

public class EmailSendingService : IEmailSendingService
{
    private readonly ApplicationDbContext _context;
    private readonly ITenantContextService _tenantContext;
    private readonly ISesClientService _sesClient;
    private readonly ILogger<EmailSendingService> _logger;

    public EmailSendingService(
        ApplicationDbContext context,
        ITenantContextService tenantContext,
        ISesClientService sesClient,
        ILogger<EmailSendingService> logger)
    {
        _context = context;
        _tenantContext = tenantContext;
        _sesClient = sesClient;
        _logger = logger;
    }

    public async Task<DTOs.Responses.SendEmailResponse> SendEmailAsync(DTOs.Requests.SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        var tenantId = _tenantContext.GetTenantId();

        // Extract domain from FromEmail
        var fromDomain = request.FromEmail.Split('@')[1];

        // Verify domain exists and is verified
        var domain = await _context.Domains
            .FirstOrDefaultAsync(d => d.TenantId == tenantId && d.Domain == fromDomain && d.VerificationStatus == 1, cancellationToken);

        if (domain == null)
        {
            throw new InvalidOperationException($"Domain {fromDomain} is not verified for sending");
        }

        // Check suppression list for all recipients
        var allRecipients = new List<string>();
        allRecipients.AddRange(request.To.Select(r => r.Email));
        if (request.Cc != null) allRecipients.AddRange(request.Cc.Select(r => r.Email));
        if (request.Bcc != null) allRecipients.AddRange(request.Bcc.Select(r => r.Email));

        var suppressedEmails = await _context.Suppressions
            .Where(s => s.TenantId == tenantId && allRecipients.Contains(s.Email))
            .Select(s => s.Email)
            .ToListAsync(cancellationToken);

        if (suppressedEmails.Any())
        {
            throw new InvalidOperationException($"The following recipients are suppressed: {string.Join(", ", suppressedEmails)}");
        }

        // Create message entity
        var message = new Messages
        {
            TenantId = tenantId,
            Region = domain.Region,
            ConfigSetId = request.ConfigSetId,
            FromEmail = request.FromEmail,
            FromName = request.FromName,
            Subject = request.Subject,
            TemplateId = request.TemplateId,
            Status = 0, // Queued
            RequestedAtUtc = DateTime.UtcNow
        };

        _context.Messages.Add(message);

        // Add recipients
        foreach (var recipient in request.To)
        {
            _context.MessageRecipients.Add(new MessageRecipients
            {
                MessageId = message.Id,
                Kind = 0, // To
                Email = recipient.Email,
                Name = recipient.Name,
                DeliveryStatus = 0 // Pending
            });
        }

        if (request.Cc != null)
        {
            foreach (var recipient in request.Cc)
            {
                _context.MessageRecipients.Add(new MessageRecipients
                {
                    MessageId = message.Id,
                    Kind = 1, // CC
                    Email = recipient.Email,
                    Name = recipient.Name,
                    DeliveryStatus = 0
                });
            }
        }

        if (request.Bcc != null)
        {
            foreach (var recipient in request.Bcc)
            {
                _context.MessageRecipients.Add(new MessageRecipients
                {
                    MessageId = message.Id,
                    Kind = 2, // BCC
                    Email = recipient.Email,
                    Name = recipient.Name,
                    DeliveryStatus = 0
                });
            }
        }

        // Add tags
        if (request.Tags != null)
        {
            foreach (var tag in request.Tags)
            {
                _context.MessageTags.Add(new MessageTags
                {
                    MessageId = message.Id,
                    Name = tag.Key,
                    Value = tag.Value
                });
            }
        }

        // Send via SES
        try
        {
            var sesRequest = new Amazon.SimpleEmailV2.Model.SendEmailRequest
            {
                FromEmailAddress = request.FromName != null
                    ? $"{request.FromName} <{request.FromEmail}>"
                    : request.FromEmail,
                Destination = new Destination
                {
                    ToAddresses = request.To.Select(r => r.Email).ToList(),
                    CcAddresses = request.Cc?.Select(r => r.Email).ToList(),
                    BccAddresses = request.Bcc?.Select(r => r.Email).ToList()
                },
                Content = new EmailContent
                {
                    Simple = new Message
                    {
                        Subject = new Content { Data = request.Subject },
                        Body = new Body
                        {
                            Html = request.HtmlBody != null ? new Content { Data = request.HtmlBody } : null,
                            Text = request.TextBody != null ? new Content { Data = request.TextBody } : null
                        }
                    }
                }
            };

            if (request.ConfigSetId.HasValue)
            {
                var configSet = await _context.ConfigSets.FindAsync(request.ConfigSetId.Value);
                if (configSet != null)
                {
                    sesRequest.ConfigurationSetName = configSet.Name;
                }
            }

            var sesResponse = await _sesClient.SendEmailAsync(sesRequest, cancellationToken);

            message.SesMessageId = sesResponse.MessageId;
            message.Status = 1; // Sent
            message.SentAtUtc = DateTime.UtcNow;

            _logger.LogInformation("Email sent successfully. MessageId: {MessageId}, SesMessageId: {SesMessageId}",
                message.Id, sesResponse.MessageId);
        }
        catch (Exception ex)
        {
            message.Status = 2; // Failed
            message.Error = ex.Message;
            _logger.LogError(ex, "Failed to send email. MessageId: {MessageId}", message.Id);
        }

        await _context.SaveChangesAsync(cancellationToken);

        return new DTOs.Responses.SendEmailResponse
        {
            MessageId = message.Id,
            SesMessageId = message.SesMessageId,
            Status = message.Status,
            RequestedAtUtc = message.RequestedAtUtc,
            Error = message.Error
        };
    }
}
