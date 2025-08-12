using System.ComponentModel.DataAnnotations;
using Simpl.Expenses.Domain.Enums;

namespace Simpl.Expenses.Application.Dtos
{
    public class UpdateApprovalLogDto
    {
        [Required]
        public ApprovalAction Action { get; set; }
        public string Comment { get; set; }
    }
}
