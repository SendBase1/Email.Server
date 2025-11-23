using Email.Server.DTOs.Requests;
using Email.Server.DTOs.Responses;

namespace Email.Server.Services.Interfaces;

public interface IEmailSendingService
{
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
}
