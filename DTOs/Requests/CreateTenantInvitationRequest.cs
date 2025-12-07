using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Email.Server.Models;

namespace Email.Server.DTOs.Requests;

public class CreateTenantInvitationRequest
{
    [Required]
    [EmailAddress]
    [MaxLength(256)]
    [JsonPropertyName("email")]
    public required string Email { get; set; }

    [JsonPropertyName("role")]
    public TenantRole Role { get; set; } = TenantRole.Viewer;
}
