using System;

namespace Simpl.Expenses.Application.Dtos
{
    public class BudgetConsumptionDto
    {
        public int Id { get; set; }
        public int BudgetId { get; set; }
        public int ReportId { get; set; }
        public decimal Amount { get; set; }
        public DateTime ConsumptionDate { get; set; }

        // Report Details
        public string ReportType { get; set; }
        public decimal ReportAmount { get; set; }
        public string PlantName { get; set; }
        public string CategoryName { get; set; }
    }
}
