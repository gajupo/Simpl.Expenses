using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos.Common;
using Simpl.Expenses.Application.Dtos.Report;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportDto>> GetAllReportsAsync();
        Task<ReportDto> GetReportByIdAsync(int id);
        Task<ReportDto> CreateReportAsync(CreateReportDto createReportDto);
        Task<ReportDto> CreateReportAsDraftAsync(CreateReportDto createReportDto);
        Task UpdateReportAsync(int id, UpdateReportDto updateReportDto);
        Task DeleteReportAsync(int id);
        Task<IEnumerable<ReportOverviewDto>> GetReportOverviewByUserIdAsync(int userId);
        Task<int> GetPendingApprovalCountAsync(int userId, int[] plantIds);
        Task UpdateReportAndSubmitAsync(int id, UpdateReportDto updateReportDto);
        Task<PaginatedResultDto<ReportOverviewDto>> GetReportsByUserIdAsync(int userId, int pageNumber, int pageSize, DateTime? startDate, DateTime? endDate, string? searchText);
        Task<PaginatedResultDto<ReportOverviewDto>> GetPendingApprovalReportsByRoleIdAsync(int roleId, string? searchText, int pageNumber, int pageSize);
        Task ApproveReportAsync(int reportId, int userId);
    }
}
