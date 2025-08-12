using Simpl.Expenses.Domain.Enums;

namespace Simpl.Expenses.Application.Dtos.ReportState
{
    public class CreateReportStateDto
    {
        public int ReportId { get; set; }
        public int WorkflowId { get; set; }
        public int CurrentStepId { get; set; }
        public ReportStatus Status { get; set; }
    }
}
