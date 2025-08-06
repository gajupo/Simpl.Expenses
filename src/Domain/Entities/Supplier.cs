
namespace Simpl.Expenses.Domain.Entities
{
    public class Supplier
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Rfc { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
