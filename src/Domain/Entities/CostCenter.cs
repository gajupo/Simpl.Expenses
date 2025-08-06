
namespace Simpl.Expenses.Domain.Entities
{
    public class CostCenter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public bool IsActive { get; set; }
    }
}
