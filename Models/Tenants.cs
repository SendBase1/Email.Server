using Email.Server.Data;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Email.Server.Models
{
    public enum TenantStatus : byte
    {
        Suspended = 0,
        Active = 1
    }

    public class Tenants
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(200)]
        public required string Name { get; set; }

        public TenantStatus Status { get; set; } = TenantStatus.Active;

        public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;
    }
}
