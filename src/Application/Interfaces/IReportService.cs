using System.Collections.Generic;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos.Report;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        Task<ReportDto> GetReportByIdAsync(int id);
        Task<ReportDto> CreateReportAsync(CreateReportDto createReportDto);
        Task UpdateReportAsync(int id, UpdateReportDto updateReportDto);
        Task DeleteReportAsync(int id);
        Task<IEnumerable<ReportOverviewDto>> GetReportOverviewByUserIdAsync(int userId);
    }
}
