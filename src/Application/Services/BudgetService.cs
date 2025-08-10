using AutoMapper;
using AutoMapper.QueryableExtensions;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace Simpl.Expenses.Application.Services
{
    public class BudgetService : IBudgetService
    {
        private readonly IGenericRepository<Budget> _budgetRepository;
        private readonly IMapper _mapper;

        public BudgetService(IGenericRepository<Budget> budgetRepository, IMapper mapper)
        {
            _budgetRepository = budgetRepository;
            _mapper = mapper;
        }

        public async Task<BudgetDto> GetBudgetByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _budgetRepository.GetAll(cancellationToken)
                .Where(b => b.Id == id)
                .ProjectTo<BudgetDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetDto>> GetAllBudgetsAsync(CancellationToken cancellationToken = default)
        {
            return await _budgetRepository.GetAll(cancellationToken)
                .ProjectTo<BudgetDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<BudgetDto> CreateBudgetAsync(CreateBudgetDto createBudgetDto, CancellationToken cancellationToken = default)
        {
            var budget = _mapper.Map<Budget>(createBudgetDto);
            await _budgetRepository.AddAsync(budget, cancellationToken);
            return _mapper.Map<BudgetDto>(budget);
        }

        public async Task UpdateBudgetAsync(int id, UpdateBudgetDto updateBudgetDto, CancellationToken cancellationToken = default)
        {
            var budget = await _budgetRepository.GetByIdAsync(id, cancellationToken);
            if (budget == null) return;

            _mapper.Map(updateBudgetDto, budget);

            await _budgetRepository.UpdateAsync(budget, cancellationToken);
        }

        public async Task DeleteBudgetAsync(int id, CancellationToken cancellationToken = default)
        {
            var budget = await _budgetRepository.GetByIdAsync(id, cancellationToken);
            if (budget != null)
            {
                await _budgetRepository.RemoveAsync(budget, cancellationToken);
            }
        }
    }
}
