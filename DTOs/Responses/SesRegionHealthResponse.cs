using Email.Server.Models;

namespace Email.Server.DTOs.Responses
{
    public class SesRegionHealthResponse
    {
        public Guid Id { get; set; }
        public string Region { get; set; } = string.Empty;
        public string? AwsSesTenantName { get; set; }
        public string? AwsSesTenantId { get; set; }
        public string? AwsSesTenantArn { get; set; }
        public string? SendingStatus { get; set; }
        public DateTime? SesTenantCreatedAt { get; set; }
        public ProvisioningStatus ProvisioningStatus { get; set; }
        public string? ProvisioningErrorMessage { get; set; }
        public DateTime? LastStatusCheckUtc { get; set; }
        public DateTime CreatedAtUtc { get; set; }
    }

    public class TenantSesHealthResponse
    {
        public Guid TenantId { get; set; }
        public string TenantName { get; set; } = string.Empty;
        public List<SesRegionHealthResponse> Regions { get; set; } = new();
        public int TotalRegions { get; set; }
        public int ProvisionedRegions { get; set; }
        public int FailedRegions { get; set; }
        public int PendingRegions { get; set; }
    }
}
