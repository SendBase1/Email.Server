using Email.Server.Data;
using Email.Server.Models;
using Email.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Email.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration,
    ILogger<AuthController> logger,
    ITenantManagementService tenantManagementService,
    ApplicationDbContext dbContext) : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager = userManager;
    private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
    private readonly IConfiguration _configuration = configuration;
    private readonly ILogger<AuthController> _logger = logger;
    private readonly ITenantManagementService _tenantManagementService = tenantManagementService;
    private readonly ApplicationDbContext _dbContext = dbContext;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Invalid input",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "User registration failed",
                Errors = [.. result.Errors.Select(e => e.Description)]
            });
        }

        _logger.LogInformation("User {Email} registered successfully", model.Email);

        // Create a default tenant for the new user
        var tenantName = $"{model.Email}'s Organization";
        var tenant = await _tenantManagementService.CreateTenantAsync(tenantName, user.Id);

        _logger.LogInformation("Created default tenant {TenantId} for new user {UserId}", tenant.Id, user.Id);

        // Generate token with the tenant ID
        var token = await GenerateJwtToken(user, tenant.Id);
        return Ok(token);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ErrorResponse
            {
                Message = "Invalid input",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            return Unauthorized(new ErrorResponse
            {
                Message = "Invalid email or password"
            });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return Unauthorized(new ErrorResponse
            {
                Message = "Invalid email or password"
            });
        }

        _logger.LogInformation("User {Email} logged in successfully", model.Email);

        // Get user's tenants
        var userTenants = await _tenantManagementService.GetTenantsByUserAsync(user.Id);
        var tenantsList = userTenants.ToList();

        if (!tenantsList.Any())
        {
            // If user has no tenants (edge case), create one
            var tenantName = $"{user.Email}'s Organization";
            var newTenant = await _tenantManagementService.CreateTenantAsync(tenantName, user.Id);
            tenantsList.Add(newTenant);
        }

        // For now, select the first tenant. In the future, we might want to let the user choose
        var selectedTenant = model.TenantId.HasValue
            ? tenantsList.FirstOrDefault(t => t.Id == model.TenantId.Value) ?? tenantsList.First()
            : tenantsList.First();

        var token = await GenerateJwtToken(user, selectedTenant.Id);

        // Return token with available tenants for future tenant switching
        return Ok(new LoginResponse
        {
            Token = token.Token,
            Email = token.Email,
            UserId = token.UserId,
            Expiration = token.Expiration,
            TenantId = selectedTenant.Id,
            AvailableTenants = tenantsList.Select(t => new TenantInfo
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        });
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok(new { message = "Logged out successfully" });
    }

    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        var tenantIdClaim = User.FindFirstValue("TenantId");
        var tenantId = Guid.TryParse(tenantIdClaim, out var tid) ? tid : (Guid?)null;

        return Ok(new
        {
            userId = user.Id,
            email = user.Email,
            userName = user.UserName,
            tenantId = tenantId
        });
    }

    [Authorize]
    [HttpPost("switch-tenant")]
    public async Task<IActionResult> SwitchTenant([FromBody] SwitchTenantRequest request)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound();
        }

        // Verify user has access to the requested tenant
        var userTenants = await _tenantManagementService.GetTenantsByUserAsync(userId);
        var selectedTenant = userTenants.FirstOrDefault(t => t.Id == request.TenantId);

        if (selectedTenant == null)
        {
            return Forbid("You don't have access to this tenant");
        }

        // Generate new token with the selected tenant
        var token = await GenerateJwtToken(user, request.TenantId);

        return Ok(new LoginResponse
        {
            Token = token.Token,
            Email = token.Email,
            UserId = token.UserId,
            Expiration = token.Expiration,
            TenantId = selectedTenant.Id,
            AvailableTenants = userTenants.Select(t => new TenantInfo
            {
                Id = t.Id,
                Name = t.Name
            }).ToList()
        });
    }

    private async Task<AuthResponse> GenerateJwtToken(ApplicationUser user, Guid tenantId)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];
        var expirationMinutes = int.Parse(jwtSettings["ExpirationMinutes"] ?? "60");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.Name, user.UserName!),
            new("TenantId", tenantId.ToString()),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // Add roles to claims
        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: credentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponse
        {
            Token = tokenString,
            Email = user.Email!,
            UserId = user.Id,
            Expiration = expiration
        };
    }
}
