using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface ICostCenterService
    {
        Task<CostCenterDto> GetCostCenterByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CostCenterDto>> GetAllCostCentersAsync(CancellationToken cancellationToken = default);
        Task<CostCenterDto> CreateCostCenterAsync(CreateCostCenterDto createCostCenterDto, CancellationToken cancellationToken = default);
        Task UpdateCostCenterAsync(int id, UpdateCostCenterDto updateCostCenterDto, CancellationToken cancellationToken = default);
        Task DeleteCostCenterAsync(int id, CancellationToken cancellationToken = default);
    }
}
