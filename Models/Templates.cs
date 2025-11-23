using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class Templates
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [MaxLength(200)]
    public required string Name { get; set; }

    public int Version { get; set; } = 1;

    [MaxLength(998)]
    public string? Subject { get; set; }

    public string? HtmlBody { get; set; }

    public string? TextBody { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenants? Tenant { get; set; }
}
