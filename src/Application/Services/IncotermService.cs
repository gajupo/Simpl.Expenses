using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class IncotermService : IIncotermService
    {
        private readonly IGenericRepository<Incoterm> _incotermRepository;
        private readonly IMapper _mapper;

        public IncotermService(IGenericRepository<Incoterm> incotermRepository, IMapper mapper)
        {
            _incotermRepository = incotermRepository;
            _mapper = mapper;
        }

        public async Task<IncotermDto> GetIncotermByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var incoterm = await _incotermRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<IncotermDto>(incoterm);
        }

        public async Task<IEnumerable<IncotermDto>> GetAllIncotermsAsync(CancellationToken cancellationToken = default)
        {
            var incoterms = await _incotermRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<IncotermDto>>(incoterms);
        }

        public async Task<IncotermDto> CreateIncotermAsync(CreateIncotermDto createIncotermDto, CancellationToken cancellationToken = default)
        {
            var incoterm = _mapper.Map<Incoterm>(createIncotermDto);
            await _incotermRepository.AddAsync(incoterm, cancellationToken);
            return _mapper.Map<IncotermDto>(incoterm);
        }

        public async Task UpdateIncotermAsync(int id, UpdateIncotermDto updateIncotermDto, CancellationToken cancellationToken = default)
        {
            var incoterm = await _incotermRepository.GetByIdAsync(id, cancellationToken);
            if (incoterm == null) return;

            _mapper.Map(updateIncotermDto, incoterm);

            await _incotermRepository.UpdateAsync(incoterm, cancellationToken);
        }

        public async Task DeleteIncotermAsync(int id, CancellationToken cancellationToken = default)
        {
            var incoterm = await _incotermRepository.GetByIdAsync(id, cancellationToken);
            if (incoterm != null)
            {
                await _incotermRepository.RemoveAsync(incoterm, cancellationToken);
            }
        }
    }
}
