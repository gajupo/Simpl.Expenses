namespace Simpl.Expenses.Application.Dtos
{
    public class CreateSupplierDto
    {
        public string Name { get; set; }
        public string Rfc { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
