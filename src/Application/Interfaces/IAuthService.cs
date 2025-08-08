using System.Threading;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos.Auth;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IAuthService
    {
        Task<AuthProjectionDto?> GetAuthProjectionByUsernameAsync(string username, CancellationToken cancellationToken = default);
        Task AssignRolesToUserAsync(int userId, int[] roleIds, CancellationToken cancellationToken = default);
        Task SetDirectPermissionsForUserAsync(int userId, string[] permissionNames, CancellationToken cancellationToken = default);
        Task AssignPermissionsToRoleAsync(int roleId, string[] permissionNames, CancellationToken cancellationToken = default);
    }
}
