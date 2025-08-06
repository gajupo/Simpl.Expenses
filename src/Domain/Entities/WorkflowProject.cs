
namespace Simpl.Expenses.Domain.Entities
{
    public class WorkflowProject
    {
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        public int ProjectId { get; set; }
        public AccountProject Project { get; set; }
    }
}
