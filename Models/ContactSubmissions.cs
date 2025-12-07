using System.ComponentModel.DataAnnotations;

namespace Email.Server.Models;

public enum ContactSubmissionStatus
{
    New,
    Read,
    Replied,
    Closed
}

public class ContactSubmissions
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    [MaxLength(255)]
    public required string Email { get; set; }

    [Required]
    [MaxLength(5000)]
    public required string Message { get; set; }

    public ContactSubmissionStatus Status { get; set; } = ContactSubmissionStatus.New;

    /// <summary>
    /// IP address of the submitter (for spam tracking)
    /// </summary>
    [MaxLength(45)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent of the submitter
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }

    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public DateTime? ReadAtUtc { get; set; }

    public DateTime? RepliedAtUtc { get; set; }

    public DateTime? ClosedAtUtc { get; set; }

    /// <summary>
    /// Notes from support team
    /// </summary>
    [MaxLength(2000)]
    public string? Notes { get; set; }
}
