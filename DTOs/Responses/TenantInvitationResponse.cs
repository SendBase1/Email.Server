using System.Text.Json.Serialization;
using Email.Server.Models;

namespace Email.Server.DTOs.Responses;

public class TenantInvitationResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("tenant_id")]
    public Guid TenantId { get; set; }

    [JsonPropertyName("tenant_name")]
    public string? TenantName { get; set; }

    [JsonPropertyName("invitee_email")]
    public string InviteeEmail { get; set; } = string.Empty;

    [JsonPropertyName("role")]
    public TenantRole Role { get; set; }

    [JsonPropertyName("status")]
    public InvitationStatus Status { get; set; }

    [JsonPropertyName("invited_by_email")]
    public string? InvitedByEmail { get; set; }

    [JsonPropertyName("created_at_utc")]
    public DateTime CreatedAtUtc { get; set; }

    [JsonPropertyName("expires_at_utc")]
    public DateTime ExpiresAtUtc { get; set; }

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}
