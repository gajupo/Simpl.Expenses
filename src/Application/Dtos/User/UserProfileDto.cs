using System;
using System.Collections.Generic;

namespace Simpl.Expenses.Application.Dtos.User
{
    public class UserProfileDto
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

        public int? RoleId { get; set; }
        public string RoleName { get; set; }

        public int? DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int? CostCenterId { get; set; }
        public string CostCenterName { get; set; }

        public List<UserPlantDto> UserPlants { get; set; }

        public int? ReportsToId { get; set; }

        public string[] Permissions { get; set; }
    }

    public class UserPlantDto
    {
        public int PlantId { get; set; }
        public string PlantName { get; set; }
    }
}
