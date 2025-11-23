using System;
using Email.Server.Models;

namespace Email.Server.DTOs.Responses
{
    public class TenantMemberResponse
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public TenantRole Role { get; set; }
        public DateTime JoinedAtUtc { get; set; }
    }
}