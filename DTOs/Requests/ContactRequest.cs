using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests;

public class ContactRequest
{
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public required string Email { get; set; }

    [Required]
    [StringLength(5000)]
    public required string Message { get; set; }
}
