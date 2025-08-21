using AutoMapper;
using AutoMapper.QueryableExtensions;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Services
{
    public class WorkflowService : IWorkflowService
    {
        private readonly IGenericRepository<Workflow> _workflowRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorkflowService(IGenericRepository<Workflow> workflowRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _workflowRepository = workflowRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<WorkflowDto> GetWorkflowByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _workflowRepository.GetAll(cancellationToken)
                .Where(w => w.Id == id)
                .ProjectTo<WorkflowDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<WorkflowDto>> GetAllWorkflowsAsync(CancellationToken cancellationToken = default)
        {
            return await _workflowRepository.GetAll(cancellationToken)
                .ProjectTo<WorkflowDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<WorkflowDto> CreateWorkflowAsync(CreateWorkflowDto createWorkflowDto, CancellationToken cancellationToken = default)
        {
            var workflow = _mapper.Map<Workflow>(createWorkflowDto);
            await _workflowRepository.AddAsync(workflow, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<WorkflowDto>(workflow);
        }

        public async Task UpdateWorkflowAsync(int id, UpdateWorkflowDto updateWorkflowDto, CancellationToken cancellationToken = default)
        {
            var workflow = await _workflowRepository.GetByIdAsync(id, cancellationToken);
            if (workflow == null) return;

            _mapper.Map(updateWorkflowDto, workflow);

            await _workflowRepository.UpdateAsync(workflow, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteWorkflowAsync(int id, CancellationToken cancellationToken = default)
        {
            var workflow = await _workflowRepository.GetByIdAsync(id, cancellationToken);
            if (workflow != null)
            {
                await _workflowRepository.RemoveAsync(workflow, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
