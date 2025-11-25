using System.ComponentModel.DataAnnotations;

namespace Email.Server.DTOs.Requests
{
    public class CreateTenantRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 1)]
        public required string Name { get; set; }
    }
}