using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests.Billing;

public class CreateCheckoutRequest
{
    [Required]
    public Guid PlanId { get; set; }

    [Required]
    [Url]
    public required string SuccessUrl { get; set; }

    [Required]
    [Url]
    public required string CancelUrl { get; set; }
}
