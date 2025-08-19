using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System;

namespace Simpl.Expenses.Application.Services
{
    public class ApprovalLogService : IApprovalLogService
    {
        private readonly IGenericRepository<ApprovalLog> _approvalLogRepository;
        private readonly IMapper _mapper;

        public ApprovalLogService(IGenericRepository<ApprovalLog> approvalLogRepository, IMapper mapper)
        {
            _approvalLogRepository = approvalLogRepository;
            _mapper = mapper;
        }

        public async Task<ApprovalLogDto> GetApprovalLogByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _approvalLogRepository.GetAll(cancellationToken)
                .Where(a => a.Id == id)
                .ProjectTo<ApprovalLogDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ApprovalLogDto>> GetAllApprovalLogsAsync(CancellationToken cancellationToken = default)
        {
            return await _approvalLogRepository.GetAll(cancellationToken)
                .ProjectTo<ApprovalLogDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<ApprovalLogDto> CreateApprovalLogAsync(CreateApprovalLogDto createApprovalLogDto, CancellationToken cancellationToken = default)
        {
            var approvalLog = _mapper.Map<ApprovalLog>(createApprovalLogDto);
            approvalLog.LogDate = DateTime.UtcNow;
            await _approvalLogRepository.AddAsync(approvalLog, cancellationToken);
            return _mapper.Map<ApprovalLogDto>(approvalLog);
        }

        public async Task UpdateApprovalLogAsync(int id, UpdateApprovalLogDto updateApprovalLogDto, CancellationToken cancellationToken = default)
        {
            var approvalLog = await _approvalLogRepository.GetByIdAsync(id, cancellationToken);
            if (approvalLog == null) return;

            _mapper.Map(updateApprovalLogDto, approvalLog);

            await _approvalLogRepository.UpdateAsync(approvalLog, cancellationToken);
        }

        public async Task DeleteApprovalLogAsync(int id, CancellationToken cancellationToken = default)
        {
            var approvalLog = await _approvalLogRepository.GetByIdAsync(id, cancellationToken);
            if (approvalLog != null)
            {
                await _approvalLogRepository.RemoveAsync(approvalLog, cancellationToken);
            }
        }

        public async Task<IEnumerable<ApprovalLogHistoryDto>> GetApprovalLogsByReportIdAsync(int reportId, CancellationToken cancellationToken = default)
        {
            return await _approvalLogRepository.GetAll(cancellationToken)
                .Where(a => a.ReportId == reportId)
                .OrderByDescending(a => a.LogDate)
                .ProjectTo<ApprovalLogHistoryDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }
    }
}
