using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Application.Dtos.Common;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Simpl.Expenses.Application.Interfaces
{
    public interface IReportAttachmentService
    {
        Task<ReportAttachmentDto> GetAttachmentByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReportAttachmentDto>> GetAttachmentsForReportAsync(int reportId, CancellationToken cancellationToken = default);
        Task<(Stream fileStream, string contentType, string fileName)> GetAttachmentFileAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<ReportAttachmentDto>> CreateAttachmentsAsync(int reportId, IEnumerable<FileAttachmentDto> files, int userId, CancellationToken cancellationToken = default);
        Task DeleteAttachmentAsync(int id, CancellationToken cancellationToken = default);
    }
}
