using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Email.Server.Services.Interfaces;

namespace Email.Server.Services.Implementations;

public class SesClientService : ISesClientService
{
    private readonly IAmazonSimpleEmailServiceV2 _sesClient;
    private readonly ILogger<SesClientService> _logger;

    public SesClientService(IAmazonSimpleEmailServiceV2 sesClient, ILogger<SesClientService> logger)
    {
        _sesClient = sesClient;
        _logger = logger;
    }

    public async Task<CreateEmailIdentityResponse> CreateEmailIdentityAsync(string domain, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new CreateEmailIdentityRequest
            {
                EmailIdentity = domain,
                DkimSigningAttributes = new DkimSigningAttributes
                {
                    DomainSigningSelector = null // Use Easy DKIM (AWS-managed)
                }
            };

            _logger.LogInformation("Creating SES email identity for domain: {Domain}", domain);
            var response = await _sesClient.CreateEmailIdentityAsync(request, cancellationToken);
            _logger.LogInformation("Successfully created SES email identity for domain: {Domain}", domain);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SES email identity for domain: {Domain}", domain);
            throw;
        }
    }

    public async Task<GetEmailIdentityResponse> GetEmailIdentityAsync(string domain, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new GetEmailIdentityRequest
            {
                EmailIdentity = domain
            };

            _logger.LogInformation("Getting SES email identity for domain: {Domain}", domain);
            var response = await _sesClient.GetEmailIdentityAsync(request, cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting SES email identity for domain: {Domain}", domain);
            throw;
        }
    }

    public async Task<DeleteEmailIdentityResponse> DeleteEmailIdentityAsync(string domain, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteEmailIdentityRequest
            {
                EmailIdentity = domain
            };

            _logger.LogInformation("Deleting SES email identity for domain: {Domain}", domain);
            var response = await _sesClient.DeleteEmailIdentityAsync(request, cancellationToken);
            _logger.LogInformation("Successfully deleted SES email identity for domain: {Domain}", domain);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SES email identity for domain: {Domain}", domain);
            throw;
        }
    }

    public async Task<SendEmailResponse> SendEmailAsync(SendEmailRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Sending email via SES from: {From}", request.FromEmailAddress);
            var response = await _sesClient.SendEmailAsync(request, cancellationToken);
            _logger.LogInformation("Successfully sent email via SES. MessageId: {MessageId}", response.MessageId);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email via SES from: {From}", request.FromEmailAddress);
            throw;
        }
    }

    public async Task<CreateConfigurationSetResponse> CreateConfigurationSetAsync(string configurationSetName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new CreateConfigurationSetRequest
            {
                ConfigurationSetName = configurationSetName
            };

            _logger.LogInformation("Creating SES configuration set: {ConfigurationSetName}", configurationSetName);
            var response = await _sesClient.CreateConfigurationSetAsync(request, cancellationToken);
            _logger.LogInformation("Successfully created SES configuration set: {ConfigurationSetName}", configurationSetName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating SES configuration set: {ConfigurationSetName}", configurationSetName);
            throw;
        }
    }

    public async Task<DeleteConfigurationSetResponse> DeleteConfigurationSetAsync(string configurationSetName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteConfigurationSetRequest
            {
                ConfigurationSetName = configurationSetName
            };

            _logger.LogInformation("Deleting SES configuration set: {ConfigurationSetName}", configurationSetName);
            var response = await _sesClient.DeleteConfigurationSetAsync(request, cancellationToken);
            _logger.LogInformation("Successfully deleted SES configuration set: {ConfigurationSetName}", configurationSetName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting SES configuration set: {ConfigurationSetName}", configurationSetName);
            throw;
        }
    }
}
