using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class MessageEvents
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("Message")]
    public Guid? MessageId { get; set; }

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [ForeignKey("RegionCatalog")]
    [MaxLength(32)]
    public required string Region { get; set; }

    [MaxLength(40)]
    public required string EventType { get; set; } // delivery,bounce,complaint,open,click,reject,renderingFailure

    public DateTime OccurredAtUtc { get; set; }

    [MaxLength(320)]
    public string? Recipient { get; set; }

    public required string PayloadJson { get; set; }

    // Navigation properties
    public Messages? Message { get; set; }
    public Tenants? Tenant { get; set; }
    public RegionsCatalog? RegionCatalog { get; set; }
}
