using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IReportTypeService
    {
        Task<ReportTypeDto> GetReportTypeByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReportTypeDto>> GetAllReportTypesAsync(CancellationToken cancellationToken = default);
        Task<ReportTypeDto> CreateReportTypeAsync(CreateReportTypeDto createReportTypeDto, CancellationToken cancellationToken = default);
        Task UpdateReportTypeAsync(int id, UpdateReportTypeDto updateReportTypeDto, CancellationToken cancellationToken = default);
        Task DeleteReportTypeAsync(int id, CancellationToken cancellationToken = default);
    }
}
