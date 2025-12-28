using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Email.Server.DTOs.Requests;

public class ProvisionPhoneNumberRequest
{
    /// <summary>
    /// Type of phone number to provision: TollFree, LongCode, or ShortCode.
    /// TollFree is recommended for business messaging.
    /// </summary>
    [Required]
    [JsonPropertyName("number_type")]
    [RegularExpression("^(TollFree|LongCode|ShortCode)$", ErrorMessage = "Number type must be TollFree, LongCode, or ShortCode")]
    public required string NumberType { get; set; }

    /// <summary>
    /// ISO 3166-1 alpha-2 country code (e.g., US, CA).
    /// </summary>
    [Required]
    [MaxLength(2)]
    [JsonPropertyName("country")]
    public string Country { get; set; } = "US";
}
