using System;
using Email.Server.Models;

namespace Email.Server.DTOs.Responses
{
    public class TenantMemberResponse
    {
        public required string UserId { get; set; }
        public required string Email { get; set; }
        public required string Name { get; set; }
        public TenantRole Role { get; set; }
        public DateTime JoinedAtUtc { get; set; }
    }
}