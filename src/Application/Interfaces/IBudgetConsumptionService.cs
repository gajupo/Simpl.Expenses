using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IBudgetConsumptionService
    {
        Task<BudgetConsumptionDto?> GetBudgetConsumptionByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<BudgetConsumptionDto>> GetAllBudgetConsumptionsAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<BudgetConsumptionDto>> GetBudgetConsumptionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default);
        Task<IEnumerable<BudgetConsumptionDto>> GetBudgetConsumptionsByDateRangeAndCostCenterAsync(DateTime startDate, DateTime endDate, int costCenterId, CancellationToken cancellationToken = default);
        Task<IEnumerable<BudgetConsumptionDto>> GetBudgetConsumptionsByDateRangeAndAccountProjectAsync(DateTime startDate, DateTime endDate, int accountProjectId, CancellationToken cancellationToken = default);
    }
}
