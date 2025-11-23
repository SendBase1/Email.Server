using System.ComponentModel.DataAnnotations;
using Email.Server.Models;

namespace Email.Server.DTOs.Requests
{
    public class AddTenantMemberRequest
    {
        [Required]
        [EmailAddress]
        public string UserEmail { get; set; }

        [Required]
        public TenantRole Role { get; set; }
    }
}