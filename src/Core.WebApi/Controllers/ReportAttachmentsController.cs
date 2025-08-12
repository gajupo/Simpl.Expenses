using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Simpl.Expenses.Application.Interfaces;
using Simpl.Expenses.Domain.Constants;
using System.Threading.Tasks;
using System.Security.Claims;
using Simpl.Expenses.Application.Dtos.Common;
using System.Collections.Generic;

namespace Simpl.Expenses.WebAPI.Controllers
{
    [Authorize]
    [Route("api/report-attachments")]
    [ApiController]
    public class ReportAttachmentsController : ControllerBase
    {
        private readonly IReportAttachmentService _attachmentService;

        public ReportAttachmentsController(IReportAttachmentService attachmentService)
        {
            _attachmentService = attachmentService;
        }

        [HttpGet("{id}")]
        [Authorize(Policy = PermissionCatalog.ReportAttachmentRead)]
        public async Task<IActionResult> GetAttachmentById(int id)
        {
            var attachment = await _attachmentService.GetAttachmentByIdAsync(id);
            if (attachment == null)
            {
                return NotFound();
            }
            return Ok(attachment);
        }

        [HttpGet("report/{reportId}")]
        [Authorize(Policy = PermissionCatalog.ReportAttachmentRead)]
        public async Task<IActionResult> GetAttachmentsForReport(int reportId)
        {
            var attachments = await _attachmentService.GetAttachmentsForReportAsync(reportId);
            return Ok(attachments);
        }

        [HttpGet("{id}/download")]
        [Authorize(Policy = PermissionCatalog.ReportAttachmentRead)]
        public async Task<IActionResult> DownloadAttachment(int id)
        {
            var (fileStream, contentType, fileName) = await _attachmentService.GetAttachmentFileAsync(id);
            return File(fileStream, contentType, fileName);
        }

        [HttpPost("report/{reportId}")]
        [Authorize(Policy = PermissionCatalog.ReportAttachmentCreate)]
        public async Task<IActionResult> UploadAttachments(int reportId, IFormFileCollection files)
        {
            if (files == null || files.Count == 0)
            {
                return BadRequest("Files are not provided or are empty.");
            }

            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

            var fileDtos = new List<FileAttachmentDto>();
            foreach (var file in files)
            {
                fileDtos.Add(new FileAttachmentDto
                {
                    Content = file.OpenReadStream(),
                    FileName = file.FileName,
                    ContentType = file.ContentType,
                    Length = file.Length
                });
            }

            var attachmentDtos = await _attachmentService.CreateAttachmentsAsync(reportId, fileDtos, userId);
            return Ok(attachmentDtos);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = PermissionCatalog.ReportAttachmentDelete)]
        public async Task<IActionResult> DeleteAttachment(int id)
        {
            await _attachmentService.DeleteAttachmentAsync(id);
            return NoContent();
        }
    }
}
