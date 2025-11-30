namespace Email.Server.Exceptions;

public class UsageLimitExceededException : Exception
{
    public long CurrentUsage { get; }
    public long Limit { get; }
    public int RequestedCount { get; }

    public UsageLimitExceededException(long currentUsage, long limit, int requestedCount)
        : base($"Usage limit exceeded. Current: {currentUsage}, Limit: {limit}, Requested: {requestedCount}")
    {
        CurrentUsage = currentUsage;
        Limit = limit;
        RequestedCount = requestedCount;
    }

    public UsageLimitExceededException(string message) : base(message)
    {
    }

    public UsageLimitExceededException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
