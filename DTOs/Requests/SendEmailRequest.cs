using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests;

public class SendEmailRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public required string FromEmail { get; set; }

    [MaxLength(200)]
    public string? FromName { get; set; }

    [Required]
    public required List<EmailRecipient> To { get; set; }

    public List<EmailRecipient>? Cc { get; set; }

    public List<EmailRecipient>? Bcc { get; set; }

    [Required]
    [MaxLength(998)]
    public required string Subject { get; set; }

    public string? HtmlBody { get; set; }

    public string? TextBody { get; set; }

    public Dictionary<string, string>? Tags { get; set; }

    public Guid? ConfigSetId { get; set; }

    public Guid? TemplateId { get; set; }
}

public class EmailRecipient
{
    [Required]
    [EmailAddress]
    [MaxLength(320)]
    public required string Email { get; set; }

    [MaxLength(200)]
    public string? Name { get; set; }
}
