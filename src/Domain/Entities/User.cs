
using System.Collections.Generic;

namespace Simpl.Expenses.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int? ReportsToId { get; set; }
        public User ReportsTo { get; set; }
        public bool IsActive { get; set; }
        public ICollection<UserPermission> UserPermissions { get; set; }
    }
}
