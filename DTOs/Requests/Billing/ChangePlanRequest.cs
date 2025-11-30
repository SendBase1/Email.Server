using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests.Billing;

public class ChangePlanRequest
{
    [Required]
    public Guid NewPlanId { get; set; }
}
