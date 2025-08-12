using System;
using Simpl.Expenses.Domain.Enums;

namespace Simpl.Expenses.Domain.Entities
{
    public class ApprovalLog
    {
        public int Id { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public ApprovalAction Action { get; set; }
        public string Comment { get; set; }
        public DateTime LogDate { get; set; }
    }
}
