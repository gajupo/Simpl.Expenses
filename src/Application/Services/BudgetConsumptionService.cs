using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Services
{
    public class BudgetConsumptionService : IBudgetConsumptionService
    {
        private readonly IGenericRepository<BudgetConsumption> _budgetConsumptionRepository;
        private readonly IMapper _mapper;

        public BudgetConsumptionService(IGenericRepository<BudgetConsumption> budgetConsumptionRepository, IMapper mapper)
        {
            _budgetConsumptionRepository = budgetConsumptionRepository;
            _mapper = mapper;
        }

        public async Task<BudgetConsumptionDto?> GetBudgetConsumptionByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _budgetConsumptionRepository.GetAll(cancellationToken)
                .Where(bc => bc.Id == id)
                .ProjectTo<BudgetConsumptionDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetConsumptionDto>> GetAllBudgetConsumptionsAsync(CancellationToken cancellationToken = default)
        {
            return await _budgetConsumptionRepository.GetAll(cancellationToken)
                .ProjectTo<BudgetConsumptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetConsumptionDto>> GetBudgetConsumptionsByDateRangeAsync(DateTime startDate, DateTime endDate, CancellationToken cancellationToken = default)
        {
            return await _budgetConsumptionRepository.GetAll(cancellationToken)
                .Where(bc => bc.ConsumptionDate >= startDate && bc.ConsumptionDate <= endDate)
                .ProjectTo<BudgetConsumptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetConsumptionDto>> GetBudgetConsumptionsByDateRangeAndCostCenterAsync(DateTime startDate, DateTime endDate, int costCenterId, CancellationToken cancellationToken = default)
        {
            return await _budgetConsumptionRepository.GetAll(cancellationToken)
                .Where(bc => bc.ConsumptionDate >= startDate && bc.ConsumptionDate <= endDate && bc.Budget.CostCenterId == costCenterId)
                .ProjectTo<BudgetConsumptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<BudgetConsumptionDto>> GetBudgetConsumptionsByDateRangeAndAccountProjectAsync(DateTime startDate, DateTime endDate, int accountProjectId, CancellationToken cancellationToken = default)
        {
            return await _budgetConsumptionRepository.GetAll(cancellationToken)
                .Where(bc => bc.ConsumptionDate >= startDate && bc.ConsumptionDate <= endDate && bc.Budget.AccountProjectId == accountProjectId)
                .ProjectTo<BudgetConsumptionDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
