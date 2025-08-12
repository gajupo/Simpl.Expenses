namespace Simpl.Expenses.Application.Dtos
{
    public class ReportTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? DefaultWorkflowId { get; set; }
        public string DefaultWorkflowName { get; set; }
    }
}
