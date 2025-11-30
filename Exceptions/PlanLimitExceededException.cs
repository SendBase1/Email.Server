namespace Email.Server.Exceptions;

public class PlanLimitExceededException : Exception
{
    public string ResourceType { get; }
    public int CurrentCount { get; }
    public int MaxAllowed { get; }

    public PlanLimitExceededException(string resourceType, int currentCount, int maxAllowed)
        : base($"Plan limit exceeded for {resourceType}. Current: {currentCount}, Max allowed: {maxAllowed}")
    {
        ResourceType = resourceType;
        CurrentCount = currentCount;
        MaxAllowed = maxAllowed;
    }

    public PlanLimitExceededException(string message) : base(message)
    {
        ResourceType = "Unknown";
    }

    public PlanLimitExceededException(string message, Exception innerException) : base(message, innerException)
    {
        ResourceType = "Unknown";
    }
}
