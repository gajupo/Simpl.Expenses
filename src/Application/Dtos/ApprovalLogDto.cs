using System;
using Simpl.Expenses.Domain.Enums;

namespace Simpl.Expenses.Application.Dtos
{
    public class ApprovalLogDto
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public int UserId { get; set; }
        public ApprovalAction Action { get; set; }
        public string Comment { get; set; }
        public DateTime LogDate { get; set; }
    }
}
