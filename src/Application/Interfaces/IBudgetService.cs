using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IBudgetService
    {
        Task<BudgetDto> GetBudgetByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BudgetDto>> GetAllBudgetsAsync(CancellationToken cancellationToken = default);
        Task<BudgetDto> CreateBudgetAsync(CreateBudgetDto createBudgetDto, CancellationToken cancellationToken = default);
        Task UpdateBudgetAsync(int id, UpdateBudgetDto updateBudgetDto, CancellationToken cancellationToken = default);
        Task DeleteBudgetAsync(int id, CancellationToken cancellationToken = default);
    }
}
