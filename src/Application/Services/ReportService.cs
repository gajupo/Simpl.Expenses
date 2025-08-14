using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Simpl.Expenses.Application.Dtos.Report;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using Simpl.Expenses.Application.Dtos.ReportState;
using Simpl.Expenses.Domain.Enums;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IGenericRepository<Report> _reportRepository;
        private readonly IGenericRepository<ReportType> _reportTypeRepository;
        private readonly IReportStateService _reportStateService;
        private readonly IMapper _mapper;

        public ReportService(
            IGenericRepository<Report> reportRepository,
            IGenericRepository<ReportType> reportTypeRepository,
            IReportStateService reportStateService,
            IMapper mapper)
        {
            _reportRepository = reportRepository;
            _reportTypeRepository = reportTypeRepository;
            _reportStateService = reportStateService;
            _mapper = mapper;
        }

        public async Task<ReportDto> CreateReportAsync(CreateReportDto createReportDto)
        {
            var report = _mapper.Map<Report>(createReportDto);
            report.ReportNumber = await GenerateReportNumberAsync();

            UpdateReportDetails(report, createReportDto);

            await _reportRepository.AddAsync(report);

            var reportTypeWorkflowInfo = await _reportTypeRepository.GetAll()
                .Where(rt => rt.Id == report.ReportTypeId).FirstOrDefaultAsync();

            if (reportTypeWorkflowInfo?.DefaultWorkflowId == null) throw new InvalidOperationException("Report type does not have a default workflow.");

            var workflowInfo = await _reportTypeRepository.GetAll()
                .Where(rt => rt.Id == report.ReportTypeId)
                .Select(rt => new
                {
                    WorkflowId = rt.DefaultWorkflow.Id,
                    Steps = rt.DefaultWorkflow.Steps.OrderBy(s => s.StepNumber).Select(s => new { s.Id }).ToList()
                })
                .FirstOrDefaultAsync();

            if (workflowInfo?.Steps.Any() == true)
            {
                var firstStepId = workflowInfo.Steps.First().Id;
                var createReportStateDto = new CreateReportStateDto
                {
                    ReportId = report.Id,
                    WorkflowId = workflowInfo.WorkflowId,
                    CurrentStepId = firstStepId,
                    Status = ReportStatus.Submitted
                };
                await _reportStateService.CreateReportStateAsync(createReportStateDto);
            }

            return _mapper.Map<ReportDto>(report);
        }

        public async Task DeleteReportAsync(int id)
        {
            var report = await _reportRepository.GetByIdAsync(id);
            if (report != null)
            {
                await _reportRepository.RemoveAsync(report);
            }
        }

        public async Task<IEnumerable<ReportDto>> GetAllReportsAsync()
        {
            return await _reportRepository.GetAll()
                .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<ReportDto> GetReportByIdAsync(int id)
        {
            return await _reportRepository.GetAll()
                .Where(r => r.Id == id)
                .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ReportOverviewDto>> GetReportOverviewByUserIdAsync(int userId)
        {
            return await _reportRepository.GetAll()
                .Where(r => r.UserId == userId)
                .Select(r => new ReportOverviewDto
                {
                    Id = r.Id,
                    ReportNumber = r.ReportNumber,
                    Name = r.Name,
                    Amount = r.Amount,
                    Currency = r.Currency,
                    UserId = r.UserId,
                    ReportTypeId = r.ReportTypeId,
                    ReportTypeName = r.ReportType.Name,
                    PlantId = r.PlantId,
                    PlantName = r.Plant.Name,
                    CategoryId = r.CategoryId,
                    CategoryName = r.Category.Name,
                    CreatedAt = r.CreatedAt,
                    AccountProjectId = r.AccountProjectId,
                    AccountProjectName = r.AccountProject != null ? r.AccountProject.Name : null
                })
                .ToListAsync();
        }

        public async Task UpdateReportAsync(int id, UpdateReportDto updateReportDto)
        {
            var report = await _reportRepository.GetAll()
                .AsTracking() // ensure the graph is tracked for updates
                .Include(r => r.PurchaseOrderDetail)
                .Include(r => r.ReimbursementDetail)
                .Include(r => r.AdvancePaymentDetail)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (report != null)
            {
                // map report data except ReimbursementDetail, PurchaseOrderDetail, AdvancePaymentDetail
                _mapper.Map(updateReportDto, report);
                UpdateReportDetails(report, updateReportDto);
                await _reportRepository.UpdateAsync(report);
            }
        }

        private void UpdateReportDetails(Report report, CreateReportDto createReportDto)
        {
            report.PurchaseOrderDetail = null;
            report.ReimbursementDetail = null;
            report.AdvancePaymentDetail = null;

            if (createReportDto.PurchaseOrderDetail != null)
            {
                report.PurchaseOrderDetail = _mapper.Map<PurchaseOrderDetail>(createReportDto.PurchaseOrderDetail);
                report.PurchaseOrderDetail.ReportId = report.Id; // Ensure the ReportId is set
            }
            else if (createReportDto.ReimbursementDetail != null)
            {
                report.ReimbursementDetail = _mapper.Map<ReimbursementDetail>(createReportDto.ReimbursementDetail);
                report.ReimbursementDetail.ReportId = report.Id; // Ensure the ReportId is set
            }
            else if (createReportDto.AdvancePaymentDetail != null)
            {
                report.AdvancePaymentDetail = _mapper.Map<AdvancePaymentDetail>(createReportDto.AdvancePaymentDetail);
                report.AdvancePaymentDetail.ReportId = report.Id; // Ensure the ReportId is set
            }
        }

        private void UpdateReportDetails(Report report, UpdateReportDto updateReportDto)
        {
            // Clear existing details that don't match the update type
            if (updateReportDto.PurchaseOrderDetail != null)
            {
                // Clear other detail types
                report.ReimbursementDetail = null;
                report.AdvancePaymentDetail = null;

                if (report.PurchaseOrderDetail != null)
                {
                    _mapper.Map(updateReportDto.PurchaseOrderDetail, report.PurchaseOrderDetail);
                }
                else
                {
                    report.PurchaseOrderDetail = _mapper.Map<PurchaseOrderDetail>(updateReportDto.PurchaseOrderDetail);
                    report.PurchaseOrderDetail.ReportId = report.Id;
                }
            }
            else if (updateReportDto.ReimbursementDetail != null)
            {
                // Clear other detail types
                report.PurchaseOrderDetail = null;
                report.AdvancePaymentDetail = null;

                if (report.ReimbursementDetail != null)
                {
                    _mapper.Map(updateReportDto.ReimbursementDetail, report.ReimbursementDetail);
                }
                else
                {
                    report.ReimbursementDetail = _mapper.Map<ReimbursementDetail>(updateReportDto.ReimbursementDetail);
                    report.ReimbursementDetail.ReportId = report.Id;
                }
            }
            else if (updateReportDto.AdvancePaymentDetail != null)
            {
                // Clear other detail types
                report.PurchaseOrderDetail = null;
                report.ReimbursementDetail = null;

                if (report.AdvancePaymentDetail != null)
                {
                    _mapper.Map(updateReportDto.AdvancePaymentDetail, report.AdvancePaymentDetail);
                }
                else
                {
                    report.AdvancePaymentDetail = _mapper.Map<AdvancePaymentDetail>(updateReportDto.AdvancePaymentDetail);
                    report.AdvancePaymentDetail.ReportId = report.Id;
                }
            }
            else
            {
                // If no details are provided, clear all
                report.PurchaseOrderDetail = null;
                report.ReimbursementDetail = null;
                report.AdvancePaymentDetail = null;
            }
        }

        private async Task<string> GenerateReportNumberAsync()
        {
            var year = DateTime.UtcNow.ToString("yy");
            var lastReport = await _reportRepository.GetAll().OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var nextId = (lastReport?.Id ?? 0) + 1;
            return $"{year}-{nextId:D5}";
        }
    }
}
