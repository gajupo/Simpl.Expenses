using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;
using Simpl.Expenses.Domain.Constants;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportTypesController : ControllerBase
    {
        private readonly IReportTypeService _reportTypeService;

        public ReportTypesController(IReportTypeService reportTypeService)
        {
            _reportTypeService = reportTypeService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.ReportTypeRead)]
        public async Task<IActionResult> GetReportTypeById(int id, CancellationToken cancellationToken = default)
        {
            var reportType = await _reportTypeService.GetReportTypeByIdAsync(id, cancellationToken);
            if (reportType == null)
            {
                return NotFound();
            }
            return Ok(reportType);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.ReportTypeRead)]
        public async Task<IActionResult> GetAllReportTypes(CancellationToken cancellationToken = default)
        {
            var reportTypes = await _reportTypeService.GetAllReportTypesAsync(cancellationToken);
            return Ok(reportTypes);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.ReportTypeCreate)]
        public async Task<IActionResult> CreateReportType([FromBody] CreateReportTypeDto createReportTypeDto, CancellationToken cancellationToken = default)
        {
            var newReportType = await _reportTypeService.CreateReportTypeAsync(createReportTypeDto, cancellationToken);
            return CreatedAtAction(nameof(GetReportTypeById), new { id = newReportType.Id }, newReportType);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.ReportTypeUpdate)]
        public async Task<IActionResult> UpdateReportType(int id, [FromBody] UpdateReportTypeDto updateReportTypeDto, CancellationToken cancellationToken = default)
        {
            await _reportTypeService.UpdateReportTypeAsync(id, updateReportTypeDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.ReportTypeDelete)]
        public async Task<IActionResult> DeleteReportType(int id, CancellationToken cancellationToken = default)
        {
            await _reportTypeService.DeleteReportTypeAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
