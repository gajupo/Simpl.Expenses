using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class AccountProjectsControllerTests : IntegrationTestBase
    {
        public AccountProjectsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();
        }

        [Fact]
        public async Task GetAccountProjectById_WhenAccountProjectExists_ReturnsOk()
        {
            // Arrange
            var accountProject = new AccountProject { Name = "Test Project", Code = "TP1", IsActive = true };
            await AddAsync(accountProject);

            // Act
            var response = await _client.GetAsync($"/api/accountprojects/{accountProject.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var accountProjectDto = await response.Content.ReadFromJsonAsync<AccountProjectDto>();
            Assert.NotNull(accountProjectDto);
            Assert.Equal(accountProject.Id, accountProjectDto.Id);
        }

        [Fact]
        public async Task CreateAccountProject_WithValidData_CreatesAccountProject()
        {
            // Arrange
            var createAccountProjectDto = new CreateAccountProjectDto { Name = "New Account Project", Code = "NAP", IsActive = true };

            // Act
            var response = await _client.PostAsJsonAsync("/api/accountprojects", createAccountProjectDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var accountProjectDto = await response.Content.ReadFromJsonAsync<AccountProjectDto>();
            Assert.NotNull(accountProjectDto);
            Assert.Equal(createAccountProjectDto.Name, accountProjectDto.Name);
        }

        [Fact]
        public async Task UpdateAccountProject_WithValidData_UpdatesAccountProject()
        {
            // Arrange
            var accountProject = new AccountProject { Name = "Update Project", Code = "UP1", IsActive = true };
            await AddAsync(accountProject);
            var updateAccountProjectDto = new UpdateAccountProjectDto { Name = "Updated Project", Code = "UP2", IsActive = false };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/accountprojects/{accountProject.Id}", updateAccountProjectDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteAccountProject_WhenAccountProjectExists_DeletesAccountProject()
        {
            // Arrange
            var accountProject = new AccountProject { Name = "Delete Project", Code = "DP1", IsActive = true };
            await AddAsync(accountProject);

            // Act
            var response = await _client.DeleteAsync($"/api/accountprojects/{accountProject.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
