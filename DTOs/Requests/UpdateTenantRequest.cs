using System.ComponentModel.DataAnnotations;
using Email.Server.Models;

namespace Email.Server.DTOs.Requests
{
    public class UpdateTenantRequest
    {
        [StringLength(100, MinimumLength = 1)]
        public string Name { get; set; }

        public TenantStatus? Status { get; set; }
    }
}