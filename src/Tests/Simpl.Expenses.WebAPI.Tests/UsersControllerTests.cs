
using Core.WebApi;
using Simpl.Expenses.Application.Dtos.User;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class UsersControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public UsersControllerTests(CustomWebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task Get_ReturnsSuccessStatusCode()
        {
            var response = await _client.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
        }

        [Fact]
        public async Task Post_CreatesUser_And_ReturnsCreatedUser()
        {
            // Arrange
            var newUser = new CreateUserDto
            {
                Username = "testuser",
                Email = "test@user.com",
                Password = "password",
                RoleId = 1,
                DepartmentId = 1
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/users", newUser);
            response.EnsureSuccessStatusCode();

            var createdUser = await response.Content.ReadFromJsonAsync<UserDto>();

            // Assert
            Assert.NotNull(createdUser);
            Assert.Equal(newUser.Username, createdUser.Username);
        }
    }
}
