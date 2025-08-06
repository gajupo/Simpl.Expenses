
using System;

namespace Simpl.Expenses.Domain.Entities
{
    public class BudgetConsumption
    {
        public int Id { get; set; }
        public int? CostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }
        public int? AccountProjectId { get; set; }
        public AccountProject AccountProject { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public decimal Amount { get; set; }
        public DateTime ConsumptionDate { get; set; }
    }
}
