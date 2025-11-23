using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class DomainDnsRecords
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("Domain")]
    public Guid DomainId { get; set; }

    [MaxLength(10)]
    public required string RecordType { get; set; } // TXT, CNAME, MX

    [MaxLength(255)]
    public required string Host { get; set; }

    [MaxLength(2048)]
    public required string Value { get; set; }

    public bool Required { get; set; } = true;

    public DateTime? LastCheckedUtc { get; set; }

    public byte Status { get; set; } = 0; // 0=Unknown,1=Found,2=Missing,3=Invalid

    // Navigation properties
    public Domains? Domain { get; set; }
}
