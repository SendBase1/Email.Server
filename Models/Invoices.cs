using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Email.Server.Models;

public enum InvoiceStatus : byte
{
    Draft = 0,
    Open = 1,
    Paid = 2,
    Void = 3,
    Uncollectible = 4
}

public class Invoices
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey("Tenant")]
    public Guid TenantId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string StripeInvoiceId { get; set; }

    [MaxLength(50)]
    public string? InvoiceNumber { get; set; }

    public InvoiceStatus Status { get; set; }

    public int AmountDueCents { get; set; }

    public int AmountPaidCents { get; set; }

    public int SubtotalCents { get; set; }

    public int TaxCents { get; set; }

    public int TotalCents { get; set; }

    [MaxLength(10)]
    public string Currency { get; set; } = "usd";

    [MaxLength(2048)]
    public string? InvoicePdfUrl { get; set; }

    [MaxLength(2048)]
    public string? HostedInvoiceUrl { get; set; }

    public DateTime? PeriodStart { get; set; }

    public DateTime? PeriodEnd { get; set; }

    public DateTime? DueDate { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? PaidAtUtc { get; set; }

    // Navigation property
    public Tenants? Tenant { get; set; }
}
