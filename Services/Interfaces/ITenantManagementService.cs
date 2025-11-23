using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Email.Server.DTOs.Requests;
using Email.Server.DTOs.Responses;
using Email.Server.Models;

namespace Email.Server.Services.Interfaces
{
    public interface ITenantManagementService
    {
        /// <summary>
        /// Creates a new tenant and assigns the specified user as the owner
        /// </summary>
        Task<TenantResponse> CreateTenantAsync(string tenantName, string userId);

        /// <summary>
        /// Gets all tenants that a user is a member of
        /// </summary>
        Task<IEnumerable<TenantResponse>> GetTenantsByUserAsync(string userId);

        /// <summary>
        /// Gets a specific tenant by ID (validates user has access)
        /// </summary>
        Task<TenantResponse> GetTenantByIdAsync(Guid tenantId, string userId);

        /// <summary>
        /// Updates tenant information
        /// </summary>
        Task<TenantResponse> UpdateTenantAsync(Guid tenantId, UpdateTenantRequest request, string userId);

        /// <summary>
        /// Deletes a tenant (only owners can delete)
        /// </summary>
        Task<bool> DeleteTenantAsync(Guid tenantId, string userId);

        /// <summary>
        /// Gets all members of a tenant
        /// </summary>
        Task<IEnumerable<TenantMemberResponse>> GetTenantMembersAsync(Guid tenantId, string userId);

        /// <summary>
        /// Adds a new member to a tenant
        /// </summary>
        Task<TenantMemberResponse> AddTenantMemberAsync(Guid tenantId, AddTenantMemberRequest request, string userId);

        /// <summary>
        /// Removes a member from a tenant
        /// </summary>
        Task<bool> RemoveTenantMemberAsync(Guid tenantId, string memberUserId, string requestingUserId);

        /// <summary>
        /// Updates a member's role in a tenant
        /// </summary>
        Task<TenantMemberResponse> UpdateMemberRoleAsync(Guid tenantId, string memberUserId, TenantRole newRole, string requestingUserId);

        /// <summary>
        /// Checks if a user has a specific role in a tenant
        /// </summary>
        Task<bool> UserHasRoleAsync(Guid tenantId, string userId, TenantRole requiredRole);

        /// <summary>
        /// Gets the user's role in a specific tenant
        /// </summary>
        Task<TenantRole?> GetUserRoleAsync(Guid tenantId, string userId);
    }
}