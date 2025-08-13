
namespace Simpl.Expenses.Application.Dtos.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public int? ReportsToId { get; set; }
        public bool IsActive { get; set; }
    }
}
