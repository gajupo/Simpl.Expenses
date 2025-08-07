using Simpl.Expenses.Application.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IRoleService
    {
        Task<RoleDto> GetRoleByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<RoleDto>> GetAllRolesAsync(CancellationToken cancellationToken = default);
        Task<RoleDto> CreateRoleAsync(CreateRoleDto createRoleDto, CancellationToken cancellationToken = default);
        Task UpdateRoleAsync(int id, UpdateRoleDto updateRoleDto, CancellationToken cancellationToken = default);
        Task DeleteRoleAsync(int id, CancellationToken cancellationToken = default);
    }
}
