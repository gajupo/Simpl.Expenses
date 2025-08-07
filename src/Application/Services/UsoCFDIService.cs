using AutoMapper;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class UsoCFDIService : IUsoCFDIService
    {
        private readonly IGenericRepository<UsoCFDI> _usoCFDIRepository;
        private readonly IMapper _mapper;

        public UsoCFDIService(IGenericRepository<UsoCFDI> usoCFDIRepository, IMapper mapper)
        {
            _usoCFDIRepository = usoCFDIRepository;
            _mapper = mapper;
        }

        public async Task<UsoCFDIDto> GetUsoCFDIByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var usoCFDI = await _usoCFDIRepository.GetByIdAsync(id, cancellationToken);
            return _mapper.Map<UsoCFDIDto>(usoCFDI);
        }

        public async Task<IEnumerable<UsoCFDIDto>> GetAllUsoCFDIsAsync(CancellationToken cancellationToken = default)
        {
            var usoCFDIs = await _usoCFDIRepository.GetAll(cancellationToken).ToListAsync(cancellationToken);
            return _mapper.Map<IEnumerable<UsoCFDIDto>>(usoCFDIs);
        }

        public async Task<UsoCFDIDto> CreateUsoCFDIAsync(CreateUsoCFDIDto createUsoCFDIDto, CancellationToken cancellationToken = default)
        {
            var usoCFDI = _mapper.Map<UsoCFDI>(createUsoCFDIDto);
            await _usoCFDIRepository.AddAsync(usoCFDI, cancellationToken);
            return _mapper.Map<UsoCFDIDto>(usoCFDI);
        }

        public async Task UpdateUsoCFDIAsync(int id, UpdateUsoCFDIDto updateUsoCFDIDto, CancellationToken cancellationToken = default)
        {
            var usoCFDI = await _usoCFDIRepository.GetByIdAsync(id, cancellationToken);
            if (usoCFDI == null) return;

            _mapper.Map(updateUsoCFDIDto, usoCFDI);

            await _usoCFDIRepository.UpdateAsync(usoCFDI, cancellationToken);
        }

        public async Task DeleteUsoCFDIAsync(int id, CancellationToken cancellationToken = default)
        {
            var usoCFDI = await _usoCFDIRepository.GetByIdAsync(id, cancellationToken);
            if (usoCFDI != null)
            {
                await _usoCFDIRepository.RemoveAsync(usoCFDI, cancellationToken);
            }
        }
    }
}
