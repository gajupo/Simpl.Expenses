using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class RolesControllerTests : IntegrationTestBase
    {
        public RolesControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetRoleById_WhenRoleExists_ReturnsOk()
        {
            // Arrange
            var role = new Role { Name = "Test Role" };
            await AddAsync(role);

            // Act
            var response = await _client.GetAsync($"/api/roles/{role.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var roleDto = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.NotNull(roleDto);
            Assert.Equal(role.Id, roleDto.Id);
        }

        [Fact]
        public async Task CreateRole_WithValidData_CreatesRole()
        {
            // Arrange
            var createRoleDto = new CreateRoleDto { Name = "New Role" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/roles", createRoleDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var roleDto = await response.Content.ReadFromJsonAsync<RoleDto>();
            Assert.NotNull(roleDto);
            Assert.Equal(createRoleDto.Name, roleDto.Name);
        }

        [Fact]
        public async Task UpdateRole_WithValidData_UpdatesRole()
        {
            // Arrange
            var role = new Role { Name = "Update Role" };
            await AddAsync(role);
            var updateRoleDto = new UpdateRoleDto { Name = "Updated Role" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/roles/{role.Id}", updateRoleDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteRole_WhenRoleExists_DeletesRole()
        {
            // Arrange
            var role = new Role { Name = "Delete Role" };
            await AddAsync(role);

            // Act
            var response = await _client.DeleteAsync($"/api/roles/{role.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
