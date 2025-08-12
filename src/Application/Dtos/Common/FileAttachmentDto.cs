using System.IO;

namespace Simpl.Expenses.Application.Dtos.Common
{
    public class FileAttachmentDto
    {
        public Stream Content { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Length { get; set; }
    }
}
