namespace Email.Server.Exceptions;

public class SendingDisabledException : Exception
{
    public Guid TenantId { get; }
    public string? DisabledReason { get; }
    public DateTime? DisabledAt { get; }

    public SendingDisabledException(Guid tenantId, string? reason, DateTime? disabledAt)
        : base($"Email sending is disabled for tenant {tenantId}. Reason: {reason ?? "Unknown"}")
    {
        TenantId = tenantId;
        DisabledReason = reason;
        DisabledAt = disabledAt;
    }

    public SendingDisabledException(string message) : base(message)
    {
    }

    public SendingDisabledException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
