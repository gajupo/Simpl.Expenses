using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class CostCenterService : ICostCenterService
    {
        private readonly IGenericRepository<CostCenter> _costCenterRepository;
        private readonly IMapper _mapper;

        public CostCenterService(IGenericRepository<CostCenter> costCenterRepository, IMapper mapper)
        {
            _costCenterRepository = costCenterRepository;
            _mapper = mapper;
        }

        public async Task<CostCenterDto> GetCostCenterByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var costCenter = await _costCenterRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<CostCenterDto>(costCenter);
        }

        public async Task<IEnumerable<CostCenterDto>> GetAllCostCentersAsync(CancellationToken cancellationToken = default)
        {
            var costCenters = await _costCenterRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<CostCenterDto>>(costCenters);
        }

        public async Task<CostCenterDto> CreateCostCenterAsync(CreateCostCenterDto createCostCenterDto, CancellationToken cancellationToken = default)
        {
            var costCenter = _mapper.Map<CostCenter>(createCostCenterDto);
            await _costCenterRepository.AddAsync(costCenter, cancellationToken);
            return _mapper.Map<CostCenterDto>(costCenter);
        }

        public async Task UpdateCostCenterAsync(int id, UpdateCostCenterDto updateCostCenterDto, CancellationToken cancellationToken = default)
        {
            var costCenter = await _costCenterRepository.GetByIdAsync(id, cancellationToken);
            if (costCenter == null) return;

            _mapper.Map(updateCostCenterDto, costCenter);

            await _costCenterRepository.UpdateAsync(costCenter, cancellationToken);
        }

        public async Task DeleteCostCenterAsync(int id, CancellationToken cancellationToken = default)
        {
            var costCenter = await _costCenterRepository.GetByIdAsync(id, cancellationToken);
            if (costCenter != null)
            {
                await _costCenterRepository.RemoveAsync(costCenter, cancellationToken);
            }
        }
    }
}
