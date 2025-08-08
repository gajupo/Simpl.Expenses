using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;
using Simpl.Expenses.Domain.Entities;
using Simpl.Expenses.Domain.Constants;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkflowsController : ControllerBase
    {
        private readonly IWorkflowService _workflowService;

        public WorkflowsController(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.WorkflowRead)]
        public async Task<IActionResult> GetWorkflowById(int id, CancellationToken cancellationToken = default)
        {
            var workflow = await _workflowService.GetWorkflowByIdAsync(id, cancellationToken);
            if (workflow == null)
            {
                return NotFound();
            }
            return Ok(workflow);
        }

        [HttpGet]
        [Authorize(Policy = PermissionCatalog.WorkflowRead)]
        public async Task<IActionResult> GetAllWorkflows(CancellationToken cancellationToken = default)
        {
            var workflows = await _workflowService.GetAllWorkflowsAsync(cancellationToken);
            return Ok(workflows);
        }

        [HttpPost]
        [Authorize(Policy = PermissionCatalog.WorkflowCreate)]
        public async Task<IActionResult> CreateWorkflow([FromBody] CreateWorkflowDto createWorkflowDto, CancellationToken cancellationToken = default)
        {
            var newWorkflow = await _workflowService.CreateWorkflowAsync(createWorkflowDto, cancellationToken);
            return CreatedAtAction(nameof(GetWorkflowById), new { id = newWorkflow.Id }, newWorkflow);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = PermissionCatalog.WorkflowUpdate)]
        public async Task<IActionResult> UpdateWorkflow(int id, [FromBody] UpdateWorkflowDto updateWorkflowDto, CancellationToken cancellationToken = default)
        {
            await _workflowService.UpdateWorkflowAsync(id, updateWorkflowDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.WorkflowDelete)]
        public async Task<IActionResult> DeleteWorkflow(int id, CancellationToken cancellationToken = default)
        {
            await _workflowService.DeleteWorkflowAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
