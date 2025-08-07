using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos.User;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class DepartmentsControllerTests : IntegrationTestBase
    {
        public DepartmentsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetDepartmentById_WhenDepartmentExists_ReturnsOk()
        {
            // Arrange
            var department = new Department { Name = "Test Department" };
            await AddAsync(department);

            // Act
            var response = await _client.GetAsync($"/api/departments/{department.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var departmentDto = await response.Content.ReadFromJsonAsync<DepartmentDto>();
            Assert.NotNull(departmentDto);
            Assert.Equal(department.Id, departmentDto.Id);
        }

        [Fact]
        public async Task CreateDepartment_WithValidData_CreatesDepartment()
        {
            // Arrange
            var createDepartmentDto = new CreateDepartmentDto { Name = "New Department" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/departments", createDepartmentDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var departmentDto = await response.Content.ReadFromJsonAsync<DepartmentDto>();
            Assert.NotNull(departmentDto);
            Assert.Equal(createDepartmentDto.Name, departmentDto.Name);
        }

        [Fact]
        public async Task UpdateDepartment_WithValidData_UpdatesDepartment()
        {
            // Arrange
            var department = new Department { Name = "Update Department" };
            await AddAsync(department);
            var updateDepartmentDto = new UpdateDepartmentDto { Name = "Updated Department" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/departments/{department.Id}", updateDepartmentDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteDepartment_WhenDepartmentExists_DeletesDepartment()
        {
            // Arrange
            var department = new Department { Name = "Delete Department" };
            await AddAsync(department);

            // Act
            var response = await _client.DeleteAsync($"/api/departments/{department.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
