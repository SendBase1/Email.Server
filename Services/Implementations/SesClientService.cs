using Amazon.SimpleEmailV2;
using Amazon.SimpleEmailV2.Model;
using Email.Server.Services.Interfaces;

namespace Email.Server.Services.Implementations;

public class SesClientService(IAmazonSimpleEmailServiceV2 sesClient, ILogger<SesClientService> logger) : ISesClientService
{
    private readonly IAmazonSimpleEmailServiceV2 _sesClient = sesClient;
    private readonly ILogger<SesClientService> _logger = logger;

    public async Task<CreateEmailIdentityResponse> CreateEmailIdentityAsync(string domain, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new CreateEmailIdentityRequest
            {
                EmailIdentity = domain
                // Not setting DkimSigningAttributes defaults to Easy DKIM (AWS-managed)
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

    public async Task<CreateTenantResponse> CreateSesTenantAsync(string tenantName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new CreateTenantRequest
            {
                TenantName = tenantName
            };

            _logger.LogInformation("Creating AWS SES tenant: {TenantName}", tenantName);
            var response = await _sesClient.CreateTenantAsync(request, cancellationToken);
            _logger.LogInformation("Successfully created AWS SES tenant: {TenantName}", tenantName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating AWS SES tenant: {TenantName}", tenantName);
            throw;
        }
    }

    public async Task<CreateTenantResourceAssociationResponse> AssociateResourceToTenantAsync(string tenantName, string resourceArn, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new CreateTenantResourceAssociationRequest
            {
                TenantName = tenantName,
                ResourceArn = resourceArn
            };

            _logger.LogInformation("Associating resource {ResourceArn} to AWS SES tenant: {TenantName}", resourceArn, tenantName);
            var response = await _sesClient.CreateTenantResourceAssociationAsync(request, cancellationToken);
            _logger.LogInformation("Successfully associated resource to AWS SES tenant: {TenantName}", tenantName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error associating resource {ResourceArn} to AWS SES tenant: {TenantName}", resourceArn, tenantName);
            throw;
        }
    }

    public async Task<DeleteTenantResponse> DeleteSesTenantAsync(string tenantName, CancellationToken cancellationToken = default)
    {
        try
        {
            var request = new DeleteTenantRequest
            {
                TenantName = tenantName
            };

            _logger.LogInformation("Deleting AWS SES tenant: {TenantName}", tenantName);
            var response = await _sesClient.DeleteTenantAsync(request, cancellationToken);
            _logger.LogInformation("Successfully deleted AWS SES tenant: {TenantName}", tenantName);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting AWS SES tenant: {TenantName}", tenantName);
            throw;
        }
    }

    public async Task UpdateTenantSendingStatusAsync(string tenantArn, bool enabled, CancellationToken cancellationToken = default)
    {
        try
        {
            var sendingStatus = enabled ? SendingStatus.ENABLED : SendingStatus.DISABLED;

            var request = new UpdateReputationEntityCustomerManagedStatusRequest
            {
                ReputationEntityType = ReputationEntityType.RESOURCE,
                ReputationEntityReference = tenantArn,
                SendingStatus = sendingStatus
            };

            _logger.LogInformation("Updating sending status for tenant {TenantArn} to {Status}", tenantArn, sendingStatus);
            await _sesClient.UpdateReputationEntityCustomerManagedStatusAsync(request, cancellationToken);
            _logger.LogInformation("Successfully updated sending status for tenant {TenantArn}", tenantArn);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating sending status for tenant {TenantArn}", tenantArn);
            throw;
        }
    }
}
