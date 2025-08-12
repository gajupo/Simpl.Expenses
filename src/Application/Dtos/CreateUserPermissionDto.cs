namespace Simpl.Expenses.Application.Dtos
{
    public class CreateUserPermissionDto
    {
        public int UserId { get; set; }
        public int PermissionId { get; set; }
        public bool IsGranted { get; set; } = true;
    }
}
