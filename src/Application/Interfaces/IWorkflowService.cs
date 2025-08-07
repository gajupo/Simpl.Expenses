using Simpl.Expenses.Application.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IWorkflowService
    {
        Task<WorkflowDto> GetWorkflowByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<WorkflowDto>> GetAllWorkflowsAsync(CancellationToken cancellationToken = default);
        Task<WorkflowDto> CreateWorkflowAsync(CreateWorkflowDto createWorkflowDto, CancellationToken cancellationToken = default);
        Task UpdateWorkflowAsync(int id, UpdateWorkflowDto updateWorkflowDto, CancellationToken cancellationToken = default);
        Task DeleteWorkflowAsync(int id, CancellationToken cancellationToken = default);
    }
}
