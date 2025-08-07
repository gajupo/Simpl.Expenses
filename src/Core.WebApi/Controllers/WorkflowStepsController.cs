using Microsoft.AspNetCore.Mvc;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using System.Threading.Tasks;

namespace Core.WebApi.Controllers
{
    [ApiController]
    [Route("api/workflows/{workflowId}/steps")]
    public class WorkflowStepsController : ControllerBase
    {
        private readonly IWorkflowStepService _workflowStepService;

        public WorkflowStepsController(IWorkflowStepService workflowStepService)
        {
            _workflowStepService = workflowStepService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWorkflowStepById(int id, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepService.GetWorkflowStepByIdAsync(id, cancellationToken);
            if (workflowStep == null)
            {
                return NotFound();
            }
            return Ok(workflowStep);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWorkflowSteps(int workflowId, CancellationToken cancellationToken = default)
        {
            var workflowSteps = await _workflowStepService.GetAllWorkflowStepsAsync(workflowId, cancellationToken);
            return Ok(workflowSteps);
        }

        [HttpPost]
        public async Task<IActionResult> CreateWorkflowStep(int workflowId, [FromBody] CreateWorkflowStepDto createWorkflowStepDto, CancellationToken cancellationToken = default)
        {
            createWorkflowStepDto.WorkflowId = workflowId;
            var newWorkflowStep = await _workflowStepService.CreateWorkflowStepAsync(createWorkflowStepDto, cancellationToken);
            return CreatedAtAction(nameof(GetWorkflowStepById), new { workflowId = newWorkflowStep.WorkflowId, id = newWorkflowStep.Id }, newWorkflowStep);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWorkflowStep(int id, [FromBody] UpdateWorkflowStepDto updateWorkflowStepDto, CancellationToken cancellationToken = default)
        {
            await _workflowStepService.UpdateWorkflowStepAsync(id, updateWorkflowStepDto, cancellationToken);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWorkflowStep(int id, CancellationToken cancellationToken = default)
        {
            await _workflowStepService.DeleteWorkflowStepAsync(id, cancellationToken);
            return NoContent();
        }

        [HttpGet("current/{currentStepId}")]
        public async Task<IActionResult> GetCurrentStep(int workflowId, int currentStepId, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepService.GetCurrentStepAsync(workflowId, currentStepId, cancellationToken);
            if (workflowStep == null)
            {
                return NotFound();
            }
            return Ok(workflowStep);
        }

        [HttpGet("next/{currentStepId}")]
        public async Task<IActionResult> GetNextStep(int workflowId, int currentStepId, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepService.GetNextStepAsync(workflowId, currentStepId, cancellationToken);
            if (workflowStep == null)
            {
                return NotFound();
            }
            return Ok(workflowStep);
        }
    }
}
