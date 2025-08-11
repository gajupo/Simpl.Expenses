
using System;

namespace Simpl.Expenses.Domain.Entities
{
    public class Report
    {
        public int Id { get; set; }
        public string ReportNumber { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ReportTypeId { get; set; }
        public ReportType ReportType { get; set; }
        public int PlantId { get; set; }
        public Plant Plant { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int? SupplierId { get; set; }
        public Supplier Supplier { get; set; }
        public string BankName { get; set; }
        public string AccountNumber { get; set; }
        public string Clabe { get; set; }
        public DateTime CreatedAt { get; set; }

        public int CostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }

        public int? AccountProjectId { get; set; }
        public AccountProject AccountProject { get; set; }
    }
}
