namespace Email.Server.DTOs.Responses;

public class DnsRecordResponse
{
    public long Id { get; set; }
    public required string RecordType { get; set; } // TXT, CNAME, MX
    public required string Host { get; set; }
    public required string Value { get; set; }
    public bool Required { get; set; }
    public DateTime? LastCheckedUtc { get; set; }
    public byte Status { get; set; } // 0=Unknown,1=Found,2=Missing,3=Invalid
    public string StatusText => Status switch
    {
        1 => "Found",
        2 => "Missing",
        3 => "Invalid",
        _ => "Unknown"
    };
}
