using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests;

public class CreateDomainRequest
{
    [Required]
    [MaxLength(255)]
    public required string Domain { get; set; }

    [Required]
    [MaxLength(32)]
    public required string Region { get; set; }
}
