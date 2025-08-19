using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IApprovalLogService
    {
        Task<ApprovalLogDto> GetApprovalLogByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApprovalLogDto>> GetAllApprovalLogsAsync(CancellationToken cancellationToken = default);
        Task<ApprovalLogDto> CreateApprovalLogAsync(CreateApprovalLogDto createApprovalLogDto, CancellationToken cancellationToken = default);
        Task UpdateApprovalLogAsync(int id, UpdateApprovalLogDto updateApprovalLogDto, CancellationToken cancellationToken = default);
        Task DeleteApprovalLogAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ApprovalLogHistoryDto>> GetApprovalLogsByReportIdAsync(int reportId, CancellationToken cancellationToken = default);
    }
}
