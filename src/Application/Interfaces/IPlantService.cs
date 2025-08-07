using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IPlantService
    {
        Task<PlantDto> GetPlantByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<PlantDto>> GetAllPlantsAsync(CancellationToken cancellationToken = default);
        Task<PlantDto> CreatePlantAsync(CreatePlantDto createPlantDto, CancellationToken cancellationToken = default);
        Task UpdatePlantAsync(int id, UpdatePlantDto updatePlantDto, CancellationToken cancellationToken = default);
        Task DeletePlantAsync(int id, CancellationToken cancellationToken = default);
    }
}
