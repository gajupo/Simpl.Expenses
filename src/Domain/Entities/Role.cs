
using System.Collections.Generic;

namespace Simpl.Expenses.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RolePermission> RolePermissions { get; set; }

    }
}
