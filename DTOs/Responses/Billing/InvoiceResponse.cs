namespace Email.Server.DTOs.Responses.Billing;

public class InvoiceResponse
{
    public Guid Id { get; set; }
    public required string StripeInvoiceId { get; set; }
    public string? InvoiceNumber { get; set; }
    public required string Status { get; set; }
    public int AmountDueCents { get; set; }
    public int AmountPaidCents { get; set; }
    public int SubtotalCents { get; set; }
    public int TaxCents { get; set; }
    public int TotalCents { get; set; }
    public required string Currency { get; set; }
    public string? InvoicePdfUrl { get; set; }
    public string? HostedInvoiceUrl { get; set; }
    public DateTime? PeriodStart { get; set; }
    public DateTime? PeriodEnd { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreatedAtUtc { get; set; }
    public DateTime? PaidAtUtc { get; set; }
}
