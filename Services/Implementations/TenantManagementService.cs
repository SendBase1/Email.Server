using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Email.Server.Data;
using Email.Server.DTOs.Requests;
using Email.Server.DTOs.Responses;
using Email.Server.Models;
using Email.Server.Services.Interfaces;

namespace Email.Server.Services.Implementations
{
    public class TenantManagementService(ApplicationDbContext context, ILogger<TenantManagementService> logger) : ITenantManagementService
    {
        private readonly ApplicationDbContext _context = context;
        private readonly ILogger<TenantManagementService> _logger = logger;

        public async Task<TenantResponse> CreateTenantAsync(string tenantName, string userId)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Create the tenant
                var tenant = new Tenants
                {
                    Name = tenantName,
                    Status = TenantStatus.Active,
                    CreatedAtUtc = DateTime.UtcNow
                };

                _context.Tenants.Add(tenant);
                await _context.SaveChangesAsync();

                // Add the user as the owner
                var tenantMember = new TenantMembers
                {
                    TenantId = tenant.Id,
                    UserId = userId,
                    TenantRole = TenantRole.Owner,
                    JoinedAtUtc = DateTime.UtcNow
                };

                _context.TenantMembers.Add(tenantMember);

                // Enable default regions for the tenant
                var defaultRegions = await _context.RegionsCatalog
                    .Where(r => r.DefaultForNewTenants)
                    .ToListAsync();

                foreach (var region in defaultRegions)
                {
                    _context.SesRegions.Add(new SesRegions
                    {
                        TenantId = tenant.Id,
                        Region = region.Region,
                        //Enabled = true,
                        CreatedAtUtc = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                _logger.LogInformation("Created new tenant {TenantId} for user {UserId}", tenant.Id, userId);

                return new TenantResponse
                {
                    Id = tenant.Id,
                    Name = tenant.Name,
                    Status = tenant.Status,
                    CreatedAtUtc = tenant.CreatedAtUtc,
                    MemberCount = 1,
                    CurrentUserRole = TenantRole.Owner
                };
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error creating tenant for user {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<TenantResponse>> GetTenantsByUserAsync(string userId)
        {
            var tenants = await _context.TenantMembers
                .Where(tm => tm.UserId == userId)
                .Include(tm => tm.Tenant)
                .Select(tm => new
                {
                    Tenant = tm.Tenant,
                    UserRole = tm.TenantRole,
                    MemberCount = _context.TenantMembers.Count(m => m.TenantId == tm.TenantId)
                })
                .ToListAsync();

            return tenants.Select(t => new TenantResponse
            {
                Id = t.Tenant.Id,
                Name = t.Tenant.Name,
                Status = t.Tenant.Status,
                CreatedAtUtc = t.Tenant.CreatedAtUtc,
                MemberCount = t.MemberCount,
                CurrentUserRole = t.UserRole
            });
        }

        public async Task<TenantResponse> GetTenantByIdAsync(Guid tenantId, string userId)
        {
            var tenantMember = await _context.TenantMembers
                .Include(tm => tm.Tenant)
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            if (tenantMember == null)
            {
                throw new UnauthorizedAccessException("User does not have access to this tenant");
            }

            var memberCount = await _context.TenantMembers.CountAsync(tm => tm.TenantId == tenantId);

            return new TenantResponse
            {
                Id = tenantMember.Tenant.Id,
                Name = tenantMember.Tenant.Name,
                Status = tenantMember.Tenant.Status,
                CreatedAtUtc = tenantMember.Tenant.CreatedAtUtc,
                MemberCount = memberCount,
                CurrentUserRole = tenantMember.TenantRole
            };
        }

        public async Task<TenantResponse> UpdateTenantAsync(Guid tenantId, UpdateTenantRequest request, string userId)
        {
            var tenantMember = await _context.TenantMembers
                .Include(tm => tm.Tenant)
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            if (tenantMember == null)
            {
                throw new UnauthorizedAccessException("User does not have access to this tenant");
            }

            if (tenantMember.TenantRole != TenantRole.Owner && tenantMember.TenantRole != TenantRole.Admin)
            {
                throw new UnauthorizedAccessException("Only owners and admins can update tenant settings");
            }

            if (!string.IsNullOrEmpty(request.Name))
            {
                tenantMember.Tenant.Name = request.Name;
            }

            if (request.Status.HasValue && tenantMember.TenantRole == TenantRole.Owner)
            {
                tenantMember.Tenant.Status = request.Status.Value;
            }

            await _context.SaveChangesAsync();

            var memberCount = await _context.TenantMembers.CountAsync(tm => tm.TenantId == tenantId);

            return new TenantResponse
            {
                Id = tenantMember.Tenant.Id,
                Name = tenantMember.Tenant.Name,
                Status = tenantMember.Tenant.Status,
                CreatedAtUtc = tenantMember.Tenant.CreatedAtUtc,
                MemberCount = memberCount,
                CurrentUserRole = tenantMember.TenantRole
            };
        }

        public async Task<bool> DeleteTenantAsync(Guid tenantId, string userId)
        {
            var tenantMember = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            if (tenantMember == null || tenantMember.TenantRole != TenantRole.Owner)
            {
                throw new UnauthorizedAccessException("Only owners can delete tenants");
            }

            var tenant = await _context.Tenants.FindAsync(tenantId);
            if (tenant != null)
            {
                _context.Tenants.Remove(tenant);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Deleted tenant {TenantId} by user {UserId}", tenantId, userId);
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<TenantMemberResponse>> GetTenantMembersAsync(Guid tenantId, string userId)
        {
            // Verify user has access to this tenant
            var hasAccess = await _context.TenantMembers
                .AnyAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            if (!hasAccess)
            {
                throw new UnauthorizedAccessException("User does not have access to this tenant");
            }

            var members = await _context.TenantMembers
                .Where(tm => tm.TenantId == tenantId)
                .Include(tm => tm.User)
                .Select(tm => new TenantMemberResponse
                {
                    UserId = tm.UserId,
                    Email = tm.User.Email,
                    Name = tm.User.UserName,
                    Role = tm.TenantRole,
                    JoinedAtUtc = tm.JoinedAtUtc
                })
                .ToListAsync();

            return members;
        }

        public async Task<TenantMemberResponse> AddTenantMemberAsync(Guid tenantId, AddTenantMemberRequest request, string userId)
        {
            // Check if requesting user has permission
            var requestingMember = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            if (requestingMember == null ||
                (requestingMember.TenantRole != TenantRole.Owner && requestingMember.TenantRole != TenantRole.Admin))
            {
                throw new UnauthorizedAccessException("Only owners and admins can add members");
            }

            // Find user by email
            var newUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.UserEmail);

            if (newUser == null)
            {
                throw new ArgumentException("User not found");
            }

            // Check if user is already a member
            var existingMember = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == newUser.Id);

            if (existingMember != null)
            {
                throw new ArgumentException("User is already a member of this tenant");
            }

            // Add new member
            var tenantMember = new TenantMembers
            {
                TenantId = tenantId,
                UserId = newUser.Id,
                TenantRole = request.Role,
                JoinedAtUtc = DateTime.UtcNow
            };

            _context.TenantMembers.Add(tenantMember);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Added user {NewUserId} to tenant {TenantId} with role {Role}",
                newUser.Id, tenantId, request.Role);

            return new TenantMemberResponse
            {
                UserId = newUser.Id,
                Email = newUser.Email,
                Name = newUser.UserName,
                Role = request.Role,
                JoinedAtUtc = tenantMember.JoinedAtUtc
            };
        }

        public async Task<bool> RemoveTenantMemberAsync(Guid tenantId, string memberUserId, string requestingUserId)
        {
            // Check if requesting user has permission
            var requestingMember = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == requestingUserId);

            if (requestingMember == null ||
                (requestingMember.TenantRole != TenantRole.Owner && requestingMember.TenantRole != TenantRole.Admin))
            {
                throw new UnauthorizedAccessException("Only owners and admins can remove members");
            }

            // Prevent owner from removing themselves if they're the only owner
            if (memberUserId == requestingUserId && requestingMember.TenantRole == TenantRole.Owner)
            {
                var ownerCount = await _context.TenantMembers
                    .CountAsync(tm => tm.TenantId == tenantId && tm.TenantRole == TenantRole.Owner);

                if (ownerCount == 1)
                {
                    throw new InvalidOperationException("Cannot remove the last owner from a tenant");
                }
            }

            var member = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == memberUserId);

            if (member != null)
            {
                _context.TenantMembers.Remove(member);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Removed user {MemberUserId} from tenant {TenantId}", memberUserId, tenantId);
                return true;
            }

            return false;
        }

        public async Task<TenantMemberResponse> UpdateMemberRoleAsync(Guid tenantId, string memberUserId, TenantRole newRole, string requestingUserId)
        {
            // Check if requesting user has permission (only owners can change roles)
            var requestingMember = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == requestingUserId);

            if (requestingMember == null || requestingMember.TenantRole != TenantRole.Owner)
            {
                throw new UnauthorizedAccessException("Only owners can change member roles");
            }

            // Cannot change own role if last owner
            if (memberUserId == requestingUserId && requestingMember.TenantRole == TenantRole.Owner && newRole != TenantRole.Owner)
            {
                var ownerCount = await _context.TenantMembers
                    .CountAsync(tm => tm.TenantId == tenantId && tm.TenantRole == TenantRole.Owner);

                if (ownerCount == 1)
                {
                    throw new InvalidOperationException("Cannot demote the last owner");
                }
            }

            var member = await _context.TenantMembers
                .Include(tm => tm.User)
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == memberUserId);

            if (member == null)
            {
                throw new ArgumentException("Member not found in this tenant");
            }

            member.TenantRole = newRole;
            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated role for user {MemberUserId} in tenant {TenantId} to {NewRole}",
                memberUserId, tenantId, newRole);

            return new TenantMemberResponse
            {
                UserId = member.UserId,
                Email = member.User.Email,
                Name = member.User.UserName,
                Role = newRole,
                JoinedAtUtc = member.JoinedAtUtc
            };
        }

        public async Task<bool> UserHasRoleAsync(Guid tenantId, string userId, TenantRole requiredRole)
        {
            var member = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            if (member == null)
            {
                return false;
            }

            // Owner has all permissions
            if (member.TenantRole == TenantRole.Owner)
            {
                return true;
            }

            // Admin has Admin and Viewer permissions
            if (member.TenantRole == TenantRole.Admin &&
                (requiredRole == TenantRole.Admin || requiredRole == TenantRole.Viewer))
            {
                return true;
            }

            // Check exact role match
            return member.TenantRole == requiredRole;
        }

        public async Task<TenantRole?> GetUserRoleAsync(Guid tenantId, string userId)
        {
            var member = await _context.TenantMembers
                .FirstOrDefaultAsync(tm => tm.TenantId == tenantId && tm.UserId == userId);

            return member?.TenantRole;
        }
    }
}