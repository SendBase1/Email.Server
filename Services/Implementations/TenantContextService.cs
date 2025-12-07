using System.Security.Claims;
using Email.Server.Services.Interfaces;

namespace Email.Server.Services.Implementations;

public class TenantContextService : ITenantContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string TenantIdHeader = "X-Tenant-Id";

    public TenantContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid GetTenantId()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // First, check for X-Tenant-Id header (used when switching tenants)
        if (httpContext?.Request.Headers.TryGetValue(TenantIdHeader, out var headerValue) == true
            && Guid.TryParse(headerValue.FirstOrDefault(), out var headerTenantId))
        {
            return headerTenantId;
        }

        // Fallback to TenantId claim in JWT (set during API key auth or initial setup)
        var tenantIdClaim = httpContext?.User?.FindFirst("TenantId");

        if (tenantIdClaim == null || !Guid.TryParse(tenantIdClaim.Value, out var tenantId))
        {
            throw new UnauthorizedAccessException("Tenant ID not found in token or header");
        }

        return tenantId;
    }

    public string GetUserId()
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }

        return userId;
    }

    public bool HasTenant()
    {
        var httpContext = _httpContextAccessor.HttpContext;

        // Check header first
        if (httpContext?.Request.Headers.TryGetValue(TenantIdHeader, out var headerValue) == true
            && Guid.TryParse(headerValue.FirstOrDefault(), out _))
        {
            return true;
        }

        // Fallback to claim
        var tenantIdClaim = httpContext?.User?.FindFirst("TenantId");
        return tenantIdClaim != null && Guid.TryParse(tenantIdClaim.Value, out _);
    }

    public bool IsApiKeyAuthenticated()
    {
        var authMethod = _httpContextAccessor.HttpContext?.User?.FindFirst("AuthMethod")?.Value;
        return authMethod == "ApiKey";
    }

    public Guid? GetApiKeyId()
    {
        var apiKeyIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("ApiKeyId");
        if (apiKeyIdClaim != null && Guid.TryParse(apiKeyIdClaim.Value, out var keyId))
        {
            return keyId;
        }
        return null;
    }
}
