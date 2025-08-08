using System.Linq;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos.Auth;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;

namespace Simpl.Expenses.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _users;
        private readonly IGenericRepository<RolePermission> _rolePermissions;
        private readonly IGenericRepository<UserPermission> _userPermissions;
        private readonly IGenericRepository<Permission> _permissions;
        private readonly IGenericRepository<Role> _roles;

        public AuthService(
            IGenericRepository<User> users,
            IGenericRepository<RolePermission> rolePermissions,
            IGenericRepository<UserPermission> userPermissions,
            IGenericRepository<Permission> permissions,
            IGenericRepository<Role> roles)
        {
            _users = users;
            _rolePermissions = rolePermissions;
            _userPermissions = userPermissions;
            _permissions = permissions;
            _roles = roles;
        }

        public async Task<AuthProjectionDto?> GetAuthProjectionByUsernameAsync(string username, CancellationToken cancellationToken = default)
        {
            var userProjection = await _users.GetAll(cancellationToken)
                .Where(u => u.Username == username && u.IsActive)
                .Select(u => new
                {
                    u.Id,
                    u.Username,
                    u.Email,
                    u.PasswordHash,
                    RoleName = u.Role != null ? u.Role.Name : null,
                    RolePermissions = u.Role != null ? u.Role.RolePermissions.Select(rp => rp.Permission.Name) : Enumerable.Empty<string>(),
                    UserPermissions = u.UserPermissions.Where(up => up.IsGranted).Select(up => up.Permission.Name)
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (userProjection == null)
            {
                return null;
            }

            var roles = new List<string>();
            if (!string.IsNullOrEmpty(userProjection.RoleName))
            {
                roles.Add(userProjection.RoleName);
            }

            var permissions = userProjection.RolePermissions.Union(userProjection.UserPermissions).Distinct().ToArray();

            return new AuthProjectionDto
            {
                Id = userProjection.Id,
                Username = userProjection.Username,
                Email = userProjection.Email,
                PasswordHash = userProjection.PasswordHash,
                Roles = roles.ToArray(),
                Permissions = permissions
            };
        }

        public async Task AssignRolesToUserAsync(int userId, int[] roleIds, CancellationToken cancellationToken = default)
        {
            // Current model supports single RoleId. We'll set the first provided role.
            if (roleIds?.Length > 0)
            {
                var user = await _users.GetByIdAsync(userId, cancellationToken);
                if (user != null)
                {
                    user.RoleId = roleIds[0];
                    await _users.UpdateAsync(user, cancellationToken);
                }
            }
        }

        public async Task SetDirectPermissionsForUserAsync(int userId, string[] permissionNames, CancellationToken cancellationToken = default)
        {
            var user = await _users.GetByIdAsync(userId, cancellationToken);
            if (user == null) return;

            var permissionIds = await _permissions.GetAll(cancellationToken)
                .Where(p => permissionNames.Contains(p.Name))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            var existing = await _userPermissions.GetAll(cancellationToken)
                .Where(up => up.UserId == userId)
                .ToListAsync(cancellationToken);

            await _userPermissions.RemoveRangeAsync(existing, cancellationToken);

            var newPermissions = permissionIds
                .Select(pid => new UserPermission { UserId = userId, PermissionId = pid, IsGranted = true });
            await _userPermissions.AddRangeAsync(newPermissions, cancellationToken);
        }

        public async Task AssignPermissionsToRoleAsync(int roleId, string[] permissionNames, CancellationToken cancellationToken = default)
        {
            var permissionIds = await _permissions.GetAll(cancellationToken)
                .Where(p => permissionNames.Contains(p.Name))
                .Select(p => p.Id)
                .ToListAsync(cancellationToken);

            var existing = await _rolePermissions.GetAll(cancellationToken)
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync(cancellationToken);

            await _rolePermissions.RemoveRangeAsync(existing, cancellationToken);

            var newPermissions = permissionIds.Select(pid => new RolePermission { RoleId = roleId, PermissionId = pid });
            await _rolePermissions.AddRangeAsync(newPermissions, cancellationToken);
        }
    }
}
