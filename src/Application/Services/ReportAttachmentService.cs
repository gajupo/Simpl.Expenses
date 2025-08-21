using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Simpl.Expenses.Application.Configuration;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Dtos.Common;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Services
{
    public class ReportAttachmentService : IReportAttachmentService
    {
        private readonly IGenericRepository<ReportAttachment> _attachmentRepository;
        private readonly IGenericRepository<Report> _reportRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly FileStorageSettings _fileStorageSettings;
        private readonly string[] _allowedExtensions = { ".pdf", ".docx", ".xmls", ".doc", ".xlsx", ".xls", ".png", ".jpg", ".jpeg", ".tiff" };

        public ReportAttachmentService(
            IGenericRepository<ReportAttachment> attachmentRepository,
            IGenericRepository<Report> reportRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IOptions<FileStorageSettings> fileStorageSettings)
        {
            _attachmentRepository = attachmentRepository;
            _reportRepository = reportRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _fileStorageSettings = fileStorageSettings.Value;
        }

        public async Task<ReportAttachmentDto> GetAttachmentByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _attachmentRepository.GetAll(cancellationToken)
                .Where(a => a.Id == id)
                .ProjectTo<ReportAttachmentDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<ReportAttachmentDto>> GetAttachmentsForReportAsync(int reportId, CancellationToken cancellationToken = default)
        {
            return await _attachmentRepository.GetAll(cancellationToken)
                .Where(a => a.ReportId == reportId)
                .ProjectTo<ReportAttachmentDto>(_mapper.ConfigurationProvider)
                .ToListAsync(cancellationToken);
        }

        public async Task<(Stream fileStream, string contentType, string fileName)> GetAttachmentFileAsync(int id, CancellationToken cancellationToken = default)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id, cancellationToken);
            if (attachment == null || !File.Exists(attachment.FilePath))
            {
                throw new FileNotFoundException("Attachment not found.");
            }

            var stream = new FileStream(attachment.FilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
            return (stream, attachment.MimeType, attachment.FileName);
        }

        public async Task<IEnumerable<ReportAttachmentDto>> CreateAttachmentsAsync(int reportId, IEnumerable<FileAttachmentDto> files, int userId, CancellationToken cancellationToken = default)
        {
            var report = await _reportRepository.GetByIdAsync(reportId, cancellationToken);
            if (report == null)
            {
                throw new ArgumentException("Report not found.");
            }

            var directoryPath = Path.Combine(_fileStorageSettings.BasePath, report.ReportNumber);
            Directory.CreateDirectory(directoryPath);

            var createdAttachments = new List<ReportAttachmentDto>();

            await _unitOfWork.BeginTransactionAsync(cancellationToken);
            try
            {
                foreach (var file in files)
                {
                    var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
                    if (!_allowedExtensions.Contains(extension))
                    {
                        throw new ArgumentException($"File type not allowed: {file.FileName}");
                    }

                    var filePath = Path.Combine(directoryPath, file.FileName);

                    var existingAttachment = await _attachmentRepository.GetAll()
                        .FirstOrDefaultAsync(a => a.ReportId == reportId && a.FileName == file.FileName, cancellationToken);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.Content.CopyToAsync(stream, cancellationToken);
                    }

                    if (existingAttachment != null)
                    {
                        existingAttachment.FileSizeKb = (int)(file.Length / 1024);
                        existingAttachment.MimeType = file.ContentType;
                        existingAttachment.UploadedAt = DateTime.UtcNow;
                        existingAttachment.UploadedByUserId = userId;
                        await _attachmentRepository.UpdateAsync(existingAttachment, cancellationToken);
                        createdAttachments.Add(_mapper.Map<ReportAttachmentDto>(existingAttachment));
                    }
                    else
                    {
                        var newAttachment = new ReportAttachment
                        {
                            ReportId = reportId,
                            UploadedByUserId = userId,
                            FileName = file.FileName,
                            FilePath = filePath,
                            FileSizeKb = (int)(file.Length / 1024),
                            MimeType = file.ContentType,
                            UploadedAt = DateTime.UtcNow
                        };

                        await _attachmentRepository.AddAsync(newAttachment, cancellationToken);
                        createdAttachments.Add(_mapper.Map<ReportAttachmentDto>(newAttachment));
                    }
                }
                await _unitOfWork.CommitTransactionAsync(cancellationToken);
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync(cancellationToken);
                throw;
            }

            return createdAttachments;
        }

        public async Task DeleteAttachmentAsync(int id, CancellationToken cancellationToken = default)
        {
            var attachment = await _attachmentRepository.GetByIdAsync(id, cancellationToken);
            if (attachment != null)
            {
                if (File.Exists(attachment.FilePath))
                {
                    File.Delete(attachment.FilePath);
                }
                await _attachmentRepository.RemoveAsync(attachment, cancellationToken);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
