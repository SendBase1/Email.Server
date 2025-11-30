using System.ComponentModel.DataAnnotations;

namespace Email.Server.Models;

public class StripeWebhookEvents
{
    [Key]
    [MaxLength(100)]
    public required string EventId { get; set; }

    [Required]
    [MaxLength(100)]
    public required string EventType { get; set; }

    public bool Processed { get; set; }

    public bool ProcessedSuccessfully { get; set; }

    [MaxLength(2000)]
    public string? ErrorMessage { get; set; }

    public string? PayloadJson { get; set; }

    public DateTime ReceivedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ProcessedAtUtc { get; set; }
}
