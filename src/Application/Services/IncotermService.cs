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
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public IncotermService(IGenericRepository<Incoterm> incotermRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _incotermRepository = incotermRepository;
            _unitOfWork = unitOfWork;
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
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return _mapper.Map<IncotermDto>(incoterm);
        }

        public async Task UpdateIncotermAsync(int id, UpdateIncotermDto updateIncotermDto, CancellationToken cancellationToken = default)
        {
            var incoterm = await _incotermRepository.GetByIdAsync(id, cancellationToken);
            if (incoterm == null) return;

            _mapper.Map(updateIncotermDto, incoterm);

            await _incotermRepository.UpdateAsync(incoterm, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteIncotermAsync(int id, CancellationToken cancellationToken = default)
        {
            var incoterm = await _incotermRepository.GetByIdAsync(id, cancellationToken);
            if (incoterm != null)
            {
                await _incotermRepository.RemoveAsync(incoterm, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
