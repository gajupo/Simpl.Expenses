using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Services
{
    public class WorkflowStepService : IWorkflowStepService
    {
        private readonly IGenericRepository<WorkflowStep> _workflowStepRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkflowStepService(IGenericRepository<WorkflowStep> workflowStepRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _workflowStepRepository = workflowStepRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WorkflowStepDto> GetWorkflowStepByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<WorkflowStepDto>(workflowStep);
        }

        public async Task<IEnumerable<WorkflowStepDto>> GetAllWorkflowStepsAsync(int workflowId, CancellationToken cancellationToken = default)
        {
            var workflowSteps = await _workflowStepRepository.GetAll(cancellationToken)
                .Where(ws => ws.WorkflowId == workflowId)
                .ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<WorkflowStepDto>>(workflowSteps);
        }

        public async Task<WorkflowStepDto> CreateWorkflowStepAsync(CreateWorkflowStepDto createWorkflowStepDto, CancellationToken cancellationToken = default)
        {
            var workflowStep = _mapper.Map<WorkflowStep>(createWorkflowStepDto);
            await _workflowStepRepository.AddAsync(workflowStep, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<WorkflowStepDto>(workflowStep);
        }

        public async Task UpdateWorkflowStepAsync(int id, UpdateWorkflowStepDto updateWorkflowStepDto, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepRepository.GetByIdAsync(id, cancellationToken);
            if (workflowStep == null) return;

            _mapper.Map(updateWorkflowStepDto, workflowStep);

            await _workflowStepRepository.UpdateAsync(workflowStep, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteWorkflowStepAsync(int id, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepRepository.GetByIdAsync(id, cancellationToken);
            if (workflowStep != null)
            {
                await _workflowStepRepository.RemoveAsync(workflowStep, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }

        public async Task<WorkflowStepDto> GetCurrentStepAsync(int workflowId, int currentStepId, CancellationToken cancellationToken = default)
        {
            var workflowStep = await _workflowStepRepository.GetAll(cancellationToken)
                .FirstOrDefaultAsync(ws => ws.WorkflowId == workflowId && ws.Id == currentStepId, cancellationToken);
            return _mapper.Map<WorkflowStepDto>(workflowStep);
        }

        public async Task<WorkflowStepDto> GetNextStepAsync(int workflowId, int currentStepId, CancellationToken cancellationToken = default)
        {
            var currentStep = await _workflowStepRepository.GetByIdAsync(currentStepId, cancellationToken);
            if (currentStep == null) return null;

            var nextStep = await _workflowStepRepository.GetAll(cancellationToken)
                .Where(ws => ws.WorkflowId == workflowId && ws.StepNumber > currentStep.StepNumber)
                .OrderBy(ws => ws.StepNumber)
                .FirstOrDefaultAsync(cancellationToken);

            return _mapper.Map<WorkflowStepDto>(nextStep);
        }
    }
}
