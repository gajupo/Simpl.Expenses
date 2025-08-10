using System;

namespace Simpl.Expenses.Application.Dtos
{
    public class BudgetDto
    {
        public int Id { get; set; }
        public int? CostCenterId { get; set; }
        public int? AccountProjectId { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
