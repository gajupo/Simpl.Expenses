
using Simpl.Expenses.Application.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IUserService
    {
    Task<UserDto?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(CancellationToken cancellationToken = default);
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(int id, UpdateUserDto updateUserDto, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    }
}
