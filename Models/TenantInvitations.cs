using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public enum InvitationStatus
{
    Pending,
    Accepted,
    Expired,
    Revoked
}

public class TenantInvitations
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [MaxLength(256)]
    public required string InviteeEmail { get; set; }

    public TenantRole Role { get; set; } = TenantRole.Viewer;

    /// <summary>
    /// The user ID of the person who sent this invitation
    /// </summary>
    [MaxLength(450)]
    public required string InvitedByUserId { get; set; }

    /// <summary>
    /// Unique token for accepting the invitation
    /// </summary>
    [MaxLength(64)]
    public required string InvitationToken { get; set; }

    public InvitationStatus Status { get; set; } = InvitationStatus.Pending;

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime ExpiresAtUtc { get; set; }

    public DateTime? AcceptedAtUtc { get; set; }

    /// <summary>
    /// The user ID who accepted the invitation (may differ from invitee if using a different account)
    /// </summary>
    [MaxLength(450)]
    public string? AcceptedByUserId { get; set; }

    // Navigation properties
    public Tenants? Tenant { get; set; }
}
