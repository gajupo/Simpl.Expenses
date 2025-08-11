using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos.Report;
using Simpl.Expenses.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReportDto>>> GetAllReports()
        {
            var reports = await _reportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpGet("{id}")]
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
        public async Task<ActionResult<ReportDto>> CreateReport(CreateReportDto createReportDto)
        {
            var createdReport = await _reportService.CreateReportAsync(createReportDto);
            return CreatedAtAction(nameof(GetReportById), new { id = createdReport.Id }, createdReport);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReport(int id, UpdateReportDto updateReportDto)
        {
            await _reportService.UpdateReportAsync(id, updateReportDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            await _reportService.DeleteReportAsync(id);
            return NoContent();
        }
    }
}
