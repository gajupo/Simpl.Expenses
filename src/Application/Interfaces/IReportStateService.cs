using Simpl.Expenses.Application.Dtos.ReportState;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IReportStateService
    {
        Task<ReportStateDto> CreateReportStateAsync(CreateReportStateDto createReportStateDto);
        Task<ReportStateDto> GetReportStateByReportIdAsync(int reportId);
    }
}
