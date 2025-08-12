using AutoMapper;
using AutoMapper.QueryableExtensions;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Simpl.Expenses.Application.Services
{
    public class ReportTypeService : IReportTypeService
    {
        private readonly IGenericRepository<ReportType> _reportTypeRepository;
        private readonly IMapper _mapper;

        public ReportTypeService(IGenericRepository<ReportType> reportTypeRepository, IMapper mapper)
        {
            _reportTypeRepository = reportTypeRepository;
            _mapper = mapper;
        }

        public async Task<ReportTypeDto> GetReportTypeByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _reportTypeRepository.GetAll(cancellationToken)
                .Where(rt => rt.Id == id)
                .ProjectTo<ReportTypeDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReportTypeDto>> GetAllReportTypesAsync(CancellationToken cancellationToken = default)
        {
            return await _reportTypeRepository.GetAll(cancellationToken)
                .ProjectTo<ReportTypeDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<ReportTypeDto> CreateReportTypeAsync(CreateReportTypeDto createReportTypeDto, CancellationToken cancellationToken = default)
        {
            var reportType = _mapper.Map<ReportType>(createReportTypeDto);
            await _reportTypeRepository.AddAsync(reportType, cancellationToken);

            var createdDto = await _reportTypeRepository.GetAll(cancellationToken)
                .Include(rt => rt.DefaultWorkflow)
                .Where(rt => rt.Id == reportType.Id)
                .ProjectTo<ReportTypeDto>(_mapper.ConfigurationProvider)
                .FirstAsync(cancellationToken);

            return createdDto;
        }

        public async Task UpdateReportTypeAsync(int id, UpdateReportTypeDto updateReportTypeDto, CancellationToken cancellationToken = default)
        {
            var reportType = await _reportTypeRepository.GetByIdAsync(id, cancellationToken);
            if (reportType == null) return;

            _mapper.Map(updateReportTypeDto, reportType);

            await _reportTypeRepository.UpdateAsync(reportType, cancellationToken);
        }

        public async Task DeleteReportTypeAsync(int id, CancellationToken cancellationToken = default)
        {
            var reportType = await _reportTypeRepository.GetByIdAsync(id, cancellationToken);
            if (reportType != null)
            {
                await _reportTypeRepository.RemoveAsync(reportType, cancellationToken);
            }
        }
    }
}
