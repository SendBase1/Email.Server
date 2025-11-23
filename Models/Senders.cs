using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class Senders
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Domain")]
    public Guid DomainId { get; set; }

    [MaxLength(320)]
    public required string Email { get; set; }

    [MaxLength(200)]
    public string? DisplayName { get; set; }

    public byte Status { get; set; } = 1; // 1=Active,0=Disabled

    // Navigation properties
    public Domains? Domain { get; set; }
}
