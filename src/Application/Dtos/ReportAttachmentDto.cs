using System;

namespace Simpl.Expenses.Application.Dtos
{
    public class ReportAttachmentDto
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int UploadedByUserId { get; set; }
        public string FileName { get; set; }
        public int FileSizeKb { get; set; }
        public string MimeType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
