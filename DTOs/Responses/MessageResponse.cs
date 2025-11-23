namespace Email.Server.DTOs.Responses;

public class MessageResponse
{
    public Guid Id { get; set; }
    public required string FromEmail { get; set; }
    public string? FromName { get; set; }
    public string? Subject { get; set; }
    public string? SesMessageId { get; set; }
    public byte Status { get; set; }
    public string StatusText => Status switch
    {
        1 => "Sent",
        2 => "Failed",
        3 => "Partial",
        _ => "Queued"
    };
    public DateTime RequestedAtUtc { get; set; }
    public DateTime? SentAtUtc { get; set; }
    public string? Error { get; set; }
    public List<MessageRecipientResponse> Recipients { get; set; } = new();
    public List<MessageEventResponse> Events { get; set; } = new();
    public Dictionary<string, string> Tags { get; set; } = new();
}

public class MessageRecipientResponse
{
    public long Id { get; set; }
    public byte Kind { get; set; } // 0=To,1=CC,2=BCC
    public string KindText => Kind switch
    {
        1 => "CC",
        2 => "BCC",
        _ => "To"
    };
    public required string Email { get; set; }
    public string? Name { get; set; }
    public byte DeliveryStatus { get; set; } // 0=Pending,1=Delivered,2=Bounced,3=Complained
    public string DeliveryStatusText => DeliveryStatus switch
    {
        1 => "Delivered",
        2 => "Bounced",
        3 => "Complained",
        _ => "Pending"
    };
    public string? SesDeliveryId { get; set; }
}

public class MessageEventResponse
{
    public long Id { get; set; }
    public required string EventType { get; set; }
    public DateTime OccurredAtUtc { get; set; }
    public string? Recipient { get; set; }
}
