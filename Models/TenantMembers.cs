using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Email.Server.Data;

namespace Email.Server.Models;

public enum TenantRole
{
    Owner,
    Admin,
    Viewer
}

public class TenantMembers
{
    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [ForeignKey("User")]
    [MaxLength(450)]
    public required string UserId { get; set; }

    [MaxLength(50)]
    public TenantRole TenantRole { get; set; } = TenantRole.Viewer;

    public DateTime JoinedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public Tenants? Tenant { get; set; }
    public ApplicationUser? User { get; set; }
}
