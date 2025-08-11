using System;

namespace Simpl.Expenses.Domain.Entities
{
    public class BudgetConsumption
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public Budget Budget { get; set; }
        public int ReportId { get; set; }
        public Report Report { get; set; }
        public decimal Amount { get; set; }
        public DateTime ConsumptionDate { get; set; }
    }
}