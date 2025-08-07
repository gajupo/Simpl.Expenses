using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IAccountProjectService
    {
        Task<AccountProjectDto> GetAccountProjectByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AccountProjectDto>> GetAllAccountProjectsAsync(CancellationToken cancellationToken = default);
        Task<AccountProjectDto> CreateAccountProjectAsync(CreateAccountProjectDto createAccountProjectDto, CancellationToken cancellationToken = default);
        Task UpdateAccountProjectAsync(int id, UpdateAccountProjectDto updateAccountProjectDto, CancellationToken cancellationToken = default);
        Task DeleteAccountProjectAsync(int id, CancellationToken cancellationToken = default);
    }
}
