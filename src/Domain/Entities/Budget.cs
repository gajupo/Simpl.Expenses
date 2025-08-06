
using System;

namespace Simpl.Expenses.Domain.Entities
{
    public class Budget
    {
        public int Id { get; set; }
        public int? CostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }
        public int? AccountProjectId { get; set; }
        public AccountProject AccountProject { get; set; }
        public decimal Amount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
