using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos.Common;
using Simpl.Expenses.Application.Dtos.Report;
using Simpl.Expenses.Application.Dtos.ReportState;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IReportStateService _reportStateService;

        private readonly ILogger<ReportsController> _logger;

        public ReportsController(IReportService reportService, ILogger<ReportsController> logger, IReportStateService reportStateService)
        {
            _reportService = reportService;
            _reportStateService = reportStateService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.ExpensesRead)]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.ExpensesRead)]
        public async Task<ActionResult<ReportDto>> GetReportById(int id)
        {
            var report = await _reportService.GetReportByIdAsync(id);
            if (report == null)
            {
                return NotFound();
            }
            return Ok(report);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.ExpensesCreate)]
        public async Task<ActionResult<ReportDto>> CreateReport(CreateReportDto createReportDto)
        {
            var createdReport = await _reportService.CreateReportAsync(createReportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.Id }, createdReport);
        }

        [HttpPost("draft")]
        [Authorize(Policy = PermissionCatalog.ExpensesCreate)]
        public async Task<ActionResult<ReportDto>> CreateReportAsDraft(CreateReportDto createReportDto)
        {
            var createdReport = await _reportService.CreateReportAsDraftAsync(createReportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.Id }, createdReport);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.ExpensesUpdate)]
        public async Task<IActionResult> UpdateReport(int id, UpdateReportDto updateReportDto)
        {
            await _reportService.UpdateReportAsync(id, updateReportDto);
            return NoContent();
        }

        [HttpPut("{id}/submit")]
        [Authorize(Policy = PermissionCatalog.ExpensesUpdate)]
        public async Task<IActionResult> SubmitReport(int id, UpdateReportDto updateReportDto)
        {
            await _reportService.UpdateReportAndSubmitAsync(id, updateReportDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.ExpensesDelete)]
        public async Task<IActionResult> DeleteReport(int id)
        {
            await _reportService.DeleteReportAsync(id);
            return NoContent();
        }

        [HttpGet("{reportId}/state")]
        [Authorize(Policy = PermissionCatalog.ExpensesRead)]
        public async Task<ActionResult<ReportStateDto>> GetReportState(int reportId)
        {
            var reportState = await _reportStateService.GetReportStateByReportIdAsync(reportId);
            if (reportState == null)
            {
                return NotFound();
            }
            return Ok(reportState);
        }

        [HttpGet("overview_by_user/{userId}")]
        [Authorize(Policy = PermissionCatalog.ExpensesRead)]
        public async Task<ActionResult<IEnumerable<ReportOverviewDto>>> GetReportOverviewByUserId(int userId)
        {
            var reports = await _reportService.GetReportOverviewByUserIdAsync(userId);
            return Ok(reports);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Policy = PermissionCatalog.ExpensesRead)]
        public async Task<ActionResult<PaginatedResultDto<ReportOverviewDto>>> GetReportsByUserId(int userId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate, [FromQuery] string? searchText, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var reports = await _reportService.GetReportsByUserIdAsync(userId, pageNumber, pageSize, startDate, endDate, searchText);
            return Ok(reports);
        }

        [HttpPost("{reportId}/state")]
        [Authorize(Policy = PermissionCatalog.ExpensesUpdate)]
        public async Task<ActionResult<ReportStateDto>> CreateReportState(int reportId, CreateReportStateDto createReportStateDto)
        {
            if (reportId != createReportStateDto.ReportId)
            {
                return BadRequest("Report ID mismatch");
            }

            var createdReportState = await _reportStateService.CreateReportStateAsync(createReportStateDto);
            return Ok(createdReportState);
        }

        [HttpGet("pending_approval_count/{userId}")]
        [Authorize(Policy = PermissionCatalog.ExpensesApprove)]
        public async Task<ActionResult<int>> GetPendingApprovalCount(int userId, [FromQuery] int[] plantIds)
        {
            var count = await _reportService.GetPendingApprovalCountAsync(userId, plantIds);
            return Ok(count);
        }
    }
}
