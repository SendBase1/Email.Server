using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests.Billing;

public class CreatePortalSessionRequest
{
    [Required]
    [Url]
    public required string ReturnUrl { get; set; }
}
