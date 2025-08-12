using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/approval-logs")]
    [ApiController]
    public class ApprovalLogsController : ControllerBase
    {
        private readonly IApprovalLogService _approvalLogService;

        public ApprovalLogsController(IApprovalLogService approvalLogService)
        {
            _approvalLogService = approvalLogService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApprovalLogDto>> GetApprovalLogById(int id)
        {
            var approvalLog = await _approvalLogService.GetApprovalLogByIdAsync(id);
            if (approvalLog == null)
            {
                return NotFound();
            }
            return Ok(approvalLog);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ApprovalLogDto>>> GetAllApprovalLogs()
        {
            var approvalLogs = await _approvalLogService.GetAllApprovalLogsAsync();
            return Ok(approvalLogs);
        }

        [HttpPost]
        public async Task<ActionResult<ApprovalLogDto>> CreateApprovalLog([FromBody] CreateApprovalLogDto createApprovalLogDto)
        {
            var createdApprovalLog = await _approvalLogService.CreateApprovalLogAsync(createApprovalLogDto);
            return CreatedAtAction(nameof(GetApprovalLogById), new { id = createdApprovalLog.Id }, createdApprovalLog);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateApprovalLog(int id, [FromBody] UpdateApprovalLogDto updateApprovalLogDto)
        {
            await _approvalLogService.UpdateApprovalLogAsync(id, updateApprovalLogDto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApprovalLog(int id)
        {
            await _approvalLogService.DeleteApprovalLogAsync(id);
            return NoContent();
        }
    }
}
