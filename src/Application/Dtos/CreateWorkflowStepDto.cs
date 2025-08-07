namespace Simpl.Expenses.Application.Dtos
{
    public class CreateWorkflowStepDto
    {
        public int WorkflowId { get; set; }
        public int StepNumber { get; set; }
        public string Name { get; set; }
        public int ApproverRoleId { get; set; }
    }
}
