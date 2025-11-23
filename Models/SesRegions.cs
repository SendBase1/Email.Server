using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class SesRegions
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [ForeignKey("RegionCatalog")]
    [MaxLength(32)]
    public required string Region { get; set; }

    [MaxLength(2048)]
    public string? EventBusArn { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenants? Tenant { get; set; }
    public RegionsCatalog? RegionCatalog { get; set; }
}
