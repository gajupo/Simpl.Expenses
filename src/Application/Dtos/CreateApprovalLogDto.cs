using System.ComponentModel.DataAnnotations;
using Simpl.Expenses.Domain.Enums;

namespace Simpl.Expenses.Application.Dtos
{
    public class CreateApprovalLogDto
    {
        [Required]
        public int ReportId { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public ApprovalAction Action { get; set; }
        public string Comment { get; set; }
    }
}
