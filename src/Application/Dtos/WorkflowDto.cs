using System.Collections.Generic;

namespace Simpl.Expenses.Application.Dtos
{
    public class WorkflowDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<WorkflowStepDto> Steps { get; set; }
    }
}
