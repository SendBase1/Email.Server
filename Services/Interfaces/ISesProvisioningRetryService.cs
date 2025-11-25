namespace Email.Server.Services.Interfaces
{
    public interface ISesProvisioningRetryService
    {
        Task RetryFailedProvisionsAsync(CancellationToken cancellationToken = default);
    }
}
