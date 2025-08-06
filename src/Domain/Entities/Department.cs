
namespace Simpl.Expenses.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? CostCenterId { get; set; }
        public CostCenter CostCenter { get; set; }
    }
}
