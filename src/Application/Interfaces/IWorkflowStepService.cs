using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IWorkflowStepService
    {
        Task<WorkflowStepDto> GetWorkflowStepByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowStepDto>> GetAllWorkflowStepsAsync(int workflowId, CancellationToken cancellationToken = default);
        Task<WorkflowStepDto> CreateWorkflowStepAsync(CreateWorkflowStepDto createWorkflowStepDto, CancellationToken cancellationToken = default);
        Task UpdateWorkflowStepAsync(int id, UpdateWorkflowStepDto updateWorkflowStepDto, CancellationToken cancellationToken = default);
        Task DeleteWorkflowStepAsync(int id, CancellationToken cancellationToken = default);
        Task<WorkflowStepDto> GetCurrentStepAsync(int workflowId, int currentStepId, CancellationToken cancellationToken = default);
        Task<WorkflowStepDto> GetNextStepAsync(int workflowId, int currentStepId, CancellationToken cancellationToken = default);
    }
}
