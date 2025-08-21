using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos.ReportState;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Services
{
    public class ReportStateService : IReportStateService
    {
        private readonly IGenericRepository<ReportState> _reportStateRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportStateService(IGenericRepository<ReportState> reportStateRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _reportStateRepository = reportStateRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ReportStateDto> CreateReportStateAsync(CreateReportStateDto createReportStateDto)
        {
            var existingState = await _reportStateRepository.GetAll()
                .Where(rs => rs.ReportId == createReportStateDto.ReportId).FirstOrDefaultAsync();

            ReportState reportStateToSave;

            if (existingState != null)
            {
                reportStateToSave = _mapper.Map(createReportStateDto, existingState);
                await _reportStateRepository.UpdateAsync(reportStateToSave);
            }
            else
            {
                reportStateToSave = _mapper.Map<ReportState>(createReportStateDto);
                await _reportStateRepository.AddAsync(reportStateToSave);
            }
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReportStateDto>(reportStateToSave);
        }

        public async Task<ReportStateDto> GetReportStateByReportIdAsync(int reportId)
        {
            var reportState = await _reportStateRepository.GetAll()
                .Where(rs => rs.ReportId == reportId).FirstOrDefaultAsync();
            return _mapper.Map<ReportStateDto>(reportState);
        }
    }
}
