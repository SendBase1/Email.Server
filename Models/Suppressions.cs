using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class Suppressions
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long Id { get; set; }

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [MaxLength(32)]
    public string? Region { get; set; } // NULL = global

    [MaxLength(320)]
    public required string Email { get; set; }

    [MaxLength(20)]
    public required string Reason { get; set; } // bounce|complaint|manual

    [MaxLength(20)]
    public required string Source { get; set; } // ses|import|ui|api

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ExpiresAtUtc { get; set; }

    // Navigation properties
    public Tenants? Tenant { get; set; }
}
