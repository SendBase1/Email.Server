namespace Email.Server.DTOs.Responses;

public class DomainResponse
{
    public Guid Id { get; set; }
    public required string Domain { get; set; }
    public required string Region { get; set; }
    public byte VerificationStatus { get; set; } // 0=Pending,1=Verified,2=Failed
    public string VerificationStatusText => VerificationStatus switch
    {
        1 => "Verified",
        2 => "Failed",
        _ => "Pending"
    };
    public byte DkimStatus { get; set; } // 0=Pending,1=Success,2=Failed
    public string DkimStatusText => DkimStatus switch
    {
        1 => "Success",
        2 => "Failed",
        _ => "Pending"
    };
    public byte MailFromStatus { get; set; }
    public string? MailFromSubdomain { get; set; }
    public string? IdentityArn { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? VerifiedAtUtc { get; set; }
    public List<DnsRecordResponse> DnsRecords { get; set; } = new();
}
