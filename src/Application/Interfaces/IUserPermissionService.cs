using Simpl.Expenses.Application.Dtos;

namespace Simpl.Expenses.Application.Interfaces;

public interface IUserPermissionService
{
    Task<IEnumerable<UserPermissionDto>> GetPermissionsForUserAsync(int userId, CancellationToken cancellationToken = default);
    Task<UserPermissionDto> AddPermissionToUserAsync(CreateUserPermissionDto createUserPermissionDto, CancellationToken cancellationToken = default);
    Task UpdatePermissionForUserAsync(int userId, int permissionId, UpdateUserPermissionDto updateUserPermissionDto, CancellationToken cancellationToken = default);
    Task RemovePermissionFromUserAsync(int userId, int permissionId, CancellationToken cancellationToken = default);
}
