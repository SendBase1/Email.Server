namespace Email.Server.Exceptions;

public class SubscriptionRequiredException : Exception
{
    public Guid TenantId { get; }

    public SubscriptionRequiredException(Guid tenantId)
        : base($"An active subscription is required to perform this action. Tenant: {tenantId}")
    {
        TenantId = tenantId;
    }

    public SubscriptionRequiredException(string message) : base(message)
    {
    }

    public SubscriptionRequiredException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
