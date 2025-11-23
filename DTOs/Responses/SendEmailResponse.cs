namespace Email.Server.DTOs.Responses;

public class SendEmailResponse
{
    public Guid MessageId { get; set; }
    public string? SesMessageId { get; set; }
    public byte Status { get; set; } // 0=Queued,1=Sent,2=Failed,3=Partial
    public string StatusText => Status switch
    {
        1 => "Sent",
        2 => "Failed",
        3 => "Partial",
        _ => "Queued"
    };
    public DateTime RequestedAtUtc { get; set; }
    public string? Error { get; set; }
}
