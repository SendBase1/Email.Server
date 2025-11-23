using Amazon.SimpleEmailV2.Model;

namespace Email.Server.Services.Interfaces;

public interface ISesClientService
{
    Task<CreateEmailIdentityResponse> CreateEmailIdentityAsync(string domain, CancellationToken cancellationToken = default);
    Task<GetEmailIdentityResponse> GetEmailIdentityAsync(string domain, CancellationToken cancellationToken = default);
    Task<DeleteEmailIdentityResponse> DeleteEmailIdentityAsync(string domain, CancellationToken cancellationToken = default);
    Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default);
    Task<CreateConfigurationSetResponse> CreateConfigurationSetAsync(string configurationSetName, CancellationToken cancellationToken = default);
    Task<DeleteConfigurationSetResponse> DeleteConfigurationSetAsync(string configurationSetName, CancellationToken cancellationToken = default);
}
