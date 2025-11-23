using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class InboundMessages
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [ForeignKey("RegionCatalog")]
    [MaxLength(32)]
    public required string Region { get; set; }

    [MaxLength(320)]
    public required string Recipient { get; set; } // address that received it (your inbound domain)

    [MaxLength(320)]
    public required string FromAddress { get; set; }

    [MaxLength(998)]
    public string? Subject { get; set; }

    public DateTime ReceivedAtUtc { get; set; }

    [MaxLength(1024)]
    public required string S3ObjectKey { get; set; } // raw MIME in S3

    public string? ParsedJson { get; set; } // optional parsed metadata

    // Navigation properties
    public Tenants? Tenant { get; set; }
    public RegionsCatalog? RegionCatalog { get; set; }
}
