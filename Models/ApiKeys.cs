using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class ApiKeys
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    [MaxLength(8)]
    public required string KeyPrefix { get; set; } // for lookup/UX only

    [MaxLength(64)]
    public required byte[] KeyHash { get; set; } // store hash (e.g., SHA-256/Argon2)

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? LastUsedAtUtc { get; set; }

    public bool IsRevoked { get; set; } = false;

    // Navigation properties
    public Tenants? Tenant { get; set; }
}
