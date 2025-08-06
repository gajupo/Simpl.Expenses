
namespace Simpl.Expenses.Domain.Entities
{
    public class WorkflowStep
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public Workflow Workflow { get; set; }
        public int StepNumber { get; set; }
        public string Name { get; set; }
        public int ApproverRoleId { get; set; }
        public Role ApproverRole { get; set; }
    }
}
