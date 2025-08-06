
using System;

namespace Simpl.Expenses.Domain.Entities
{
    public class ReportState
    {
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        public int CurrentStepId { get; set; }
        public WorkflowStep CurrentStep { get; set; }
        public string Status { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
