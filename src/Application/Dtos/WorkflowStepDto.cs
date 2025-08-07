namespace Simpl.Expenses.Application.Dtos
{
    public class WorkflowStepDto
    {
        public int Id { get; set; }
        public int WorkflowId { get; set; }
        public int StepNumber { get; set; }
        public string Name { get; set; }
        public int ApproverRoleId { get; set; }
    }
}
