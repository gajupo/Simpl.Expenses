namespace Simpl.Expenses.Application.Dtos
{
    public class UpdateWorkflowStepDto
    {
        public int StepNumber { get; set; }
        public string Name { get; set; }
        public int ApproverRoleId { get; set; }
    }
}
