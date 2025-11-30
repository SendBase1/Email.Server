using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public class StripeCustomers
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string StripeCustomerId { get; set; }

    [MaxLength(320)]
    public string? Email { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }

    [MaxLength(100)]
    public string? DefaultPaymentMethodId { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime UpdatedAtUtc { get; set; } = DateTime.UtcNow;

    // Navigation property
    public Tenants? Tenant { get; set; }
}
