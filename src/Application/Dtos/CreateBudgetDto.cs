using System;
using System.ComponentModel.DataAnnotations;

namespace Simpl.Expenses.Application.Dtos
{
    public class CreateBudgetDto
    {
        public int? CostCenterId { get; set; }
        public int? AccountProjectId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
