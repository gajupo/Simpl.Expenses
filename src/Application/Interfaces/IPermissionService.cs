using Simpl.Expenses.Application.Dtos;

namespace Simpl.Expenses.Application.Interfaces;

public interface IPermissionService
{
    Task<IEnumerable<PermissionDto>> GetAllPermissionsAsync(CancellationToken cancellationToken = default);
    Task<PermissionDto?> GetPermissionByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PermissionDto> CreatePermissionAsync(CreatePermissionDto createPermissionDto, CancellationToken cancellationToken = default);
    Task UpdatePermissionAsync(int id, UpdatePermissionDto updatePermissionDto, CancellationToken cancellationToken = default);
    Task DeletePermissionAsync(int id, CancellationToken cancellationToken = default);
}
