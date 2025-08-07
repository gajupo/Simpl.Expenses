
using System.ComponentModel.DataAnnotations;

namespace Simpl.Expenses.Application.Dtos.User
{
    public class UpdateUserDto
    {
        [StringLength(50)]
        public string Username { get; set; }

        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(100, MinimumLength = 8)]
        public string? Password { get; set; }

        public int? RoleId { get; set; }

        public int? DepartmentId { get; set; }

        public int? ReportsToId { get; set; }

        public bool? IsActive { get; set; }
    }
}
