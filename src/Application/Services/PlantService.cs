using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class PlantService : IPlantService
    {
        private readonly IGenericRepository<Plant> _plantRepository;
        private readonly IMapper _mapper;

        public PlantService(IGenericRepository<Plant> plantRepository, IMapper mapper)
        {
            _plantRepository = plantRepository;
            _mapper = mapper;
        }

        public async Task<PlantDto> GetPlantByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var plant = await _plantRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<PlantDto>(plant);
        }

        public async Task<IEnumerable<PlantDto>> GetAllPlantsAsync(CancellationToken cancellationToken = default)
        {
            var plants = await _plantRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<PlantDto>>(plants);
        }

        public async Task<PlantDto> CreatePlantAsync(CreatePlantDto createPlantDto, CancellationToken cancellationToken = default)
        {
            var plant = _mapper.Map<Plant>(createPlantDto);
            await _plantRepository.AddAsync(plant, cancellationToken);
            return _mapper.Map<PlantDto>(plant);
        }

        public async Task UpdatePlantAsync(int id, UpdatePlantDto updatePlantDto, CancellationToken cancellationToken = default)
        {
            var plant = await _plantRepository.GetByIdAsync(id, cancellationToken);
            if (plant == null) return;

            _mapper.Map(updatePlantDto, plant);

            await _plantRepository.UpdateAsync(plant, cancellationToken);
        }

        public async Task DeletePlantAsync(int id, CancellationToken cancellationToken = default)
        {
            var plant = await _plantRepository.GetByIdAsync(id, cancellationToken);
            if (plant != null)
            {
                await _plantRepository.RemoveAsync(plant, cancellationToken);
            }
        }
    }
}
