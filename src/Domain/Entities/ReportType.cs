
namespace Simpl.Expenses.Domain.Entities
{
    public class ReportType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int? DefaultWorkflowId { get; set; }
        public Workflow? DefaultWorkflow { get; set; }
    }
}
