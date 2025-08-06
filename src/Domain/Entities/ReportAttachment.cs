
using System;

namespace Simpl.Expenses.Domain.Entities
{
    public class ReportAttachment
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public int UploadedByUserId { get; set; }
        public User UploadedByUser { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int FileSizeKb { get; set; }
        public string MimeType { get; set; }
        public DateTime UploadedAt { get; set; }
    }
}
