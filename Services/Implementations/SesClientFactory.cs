using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleEmailV2;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Email.Server.Services.Implementations;

public interface ISesClientFactory
{
    AmazonSimpleEmailServiceV2Client CreateClient(string region);
}

public class SesClientFactory : ISesClientFactory
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<SesClientFactory> _logger;

    public SesClientFactory(IConfiguration configuration, ILogger<SesClientFactory> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    public AmazonSimpleEmailServiceV2Client CreateClient(string region)
    {
        var regionEndpoint = RegionEndpoint.GetBySystemName(region);

        // 1. Try explicit access key from configuration (for Azure App Service)
        var accessKeyId = _configuration["AWS:AccessKeyId"];
        var secretAccessKey = _configuration["AWS:SecretAccessKey"];

        if (!string.IsNullOrEmpty(accessKeyId) && !string.IsNullOrEmpty(secretAccessKey))
        {
            _logger.LogDebug("Creating SES client for region {Region} using explicit credentials", region);
            var credentials = new BasicAWSCredentials(accessKeyId, secretAccessKey);
            return new AmazonSimpleEmailServiceV2Client(credentials, regionEndpoint);
        }

        // 2. Try AWS profile (for local development)
        var profileName = _configuration["AWS:Profile"];
        if (!string.IsNullOrEmpty(profileName))
        {
            var chain = new CredentialProfileStoreChain();
            if (chain.TryGetAWSCredentials(profileName, out var profileCredentials))
            {
                _logger.LogDebug("Creating SES client for region {Region} using profile {Profile}", region, profileName);
                return new AmazonSimpleEmailServiceV2Client(profileCredentials, regionEndpoint);
            }
        }

        // 3. Fall back to default credential chain (IAM roles, environment variables, etc.)
        _logger.LogDebug("Creating SES client for region {Region} using default credential chain", region);
        return new AmazonSimpleEmailServiceV2Client(regionEndpoint);
    }
}
