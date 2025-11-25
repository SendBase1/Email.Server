using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public enum ProvisioningStatus : byte
{
    Pending = 0,
    Provisioned = 1,
    Failed = 2
}

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

    [MaxLength(255)]
    public string? AwsSesTenantName { get; set; }

    // AWS SES Tenant Metadata (from CreateTenantResponse)
    [MaxLength(255)]
    public string? AwsSesTenantId { get; set; }

    [MaxLength(2048)]
    public string? AwsSesTenantArn { get; set; }

    [MaxLength(50)]
    public string? SendingStatus { get; set; }

    public DateTime? SesTenantCreatedAt { get; set; }

    // Provisioning tracking
    public ProvisioningStatus ProvisioningStatus { get; set; } = ProvisioningStatus.Pending;

    [MaxLength(1000)]
    public string? ProvisioningErrorMessage { get; set; }

    public DateTime? LastStatusCheckUtc { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenants? Tenant { get; set; }
    public RegionsCatalog? RegionCatalog { get; set; }
}
