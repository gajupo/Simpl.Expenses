namespace Simpl.Expenses.Application.Dtos.Auth
{
    public class AuthProjectionDto
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string[] Roles { get; set; } = System.Array.Empty<string>();
        public string[] Permissions { get; set; } = System.Array.Empty<string>();
    }
}
