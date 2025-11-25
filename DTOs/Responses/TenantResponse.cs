using System;
using Email.Server.Models;

namespace Email.Server.DTOs.Responses
{
    public class TenantResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public TenantStatus Status { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public int MemberCount { get; set; }
        public TenantRole? CurrentUserRole { get; set; }
    }
}