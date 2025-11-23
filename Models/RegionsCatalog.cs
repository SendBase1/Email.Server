using System.ComponentModel.DataAnnotations;

namespace Email.Server.Models;

public class RegionsCatalog
{
    [Key]
    [MaxLength(32)]
    public required string Region { get; set; } // e.g., 'us-east-1'

    [MaxLength(100)]
    public required string DisplayName { get; set; } // e.g., 'US East (N. Virginia)'

    public bool SendSupported { get; set; } = true;

    public bool ReceiveSupported { get; set; } = false;

    public bool DefaultForNewTenants { get; set; } = false;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
}
