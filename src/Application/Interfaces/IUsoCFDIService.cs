using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IUsoCFDIService
    {
        Task<UsoCFDIDto> GetUsoCFDIByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<UsoCFDIDto>> GetAllUsoCFDIsAsync(CancellationToken cancellationToken = default);
        Task<UsoCFDIDto> CreateUsoCFDIAsync(CreateUsoCFDIDto createUsoCFDIDto, CancellationToken cancellationToken = default);
        Task UpdateUsoCFDIAsync(int id, UpdateUsoCFDIDto updateUsoCFDIDto, CancellationToken cancellationToken = default);
        Task DeleteUsoCFDIAsync(int id, CancellationToken cancellationToken = default);
    }
}
