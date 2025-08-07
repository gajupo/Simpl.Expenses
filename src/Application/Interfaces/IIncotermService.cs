using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IIncotermService
    {
        Task<IncotermDto> GetIncotermByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<IncotermDto>> GetAllIncotermsAsync(CancellationToken cancellationToken = default);
        Task<IncotermDto> CreateIncotermAsync(CreateIncotermDto createIncotermDto, CancellationToken cancellationToken = default);
        Task UpdateIncotermAsync(int id, UpdateIncotermDto updateIncotermDto, CancellationToken cancellationToken = default);
        Task DeleteIncotermAsync(int id, CancellationToken cancellationToken = default);
    }
}
