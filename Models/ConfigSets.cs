using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class ConfigSets
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("SesRegion")]
    public Guid SesRegionId { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(255)]
    public required string ConfigSetName { get; set; }

    public bool IsDefault { get; set; } = false;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public SesRegions? SesRegion { get; set; }
}
