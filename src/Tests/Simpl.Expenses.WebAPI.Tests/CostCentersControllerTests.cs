using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class CostCentersControllerTests : IntegrationTestBase
    {
        public CostCentersControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetCostCenterById_WhenCostCenterExists_ReturnsOk()
        {
            // Arrange
            var costCenter = new CostCenter { Name = "Test Cost Center", Code = "TCC1", IsActive = true };
            await AddAsync(costCenter);

            // Act
            var response = await _client.GetAsync($"/api/costcenters/{costCenter.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var costCenterDto = await response.Content.ReadFromJsonAsync<CostCenterDto>();
            Assert.NotNull(costCenterDto);
            Assert.Equal(costCenter.Id, costCenterDto.Id);
        }

        [Fact]
        public async Task CreateCostCenter_WithValidData_CreatesCostCenter()
        {
            // Arrange
            var createCostCenterDto = new CreateCostCenterDto { Name = "New Cost Center", Code = "NCC", IsActive = true };

            // Act
            var response = await _client.PostAsJsonAsync("/api/costcenters", createCostCenterDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var costCenterDto = await response.Content.ReadFromJsonAsync<CostCenterDto>();
            Assert.NotNull(costCenterDto);
            Assert.Equal(createCostCenterDto.Name, costCenterDto.Name);
        }

        [Fact]
        public async Task UpdateCostCenter_WithValidData_UpdatesCostCenter()
        {
            // Arrange
            var costCenter = new CostCenter { Name = "Update Cost Center", Code = "UCC1", IsActive = true };
            await AddAsync(costCenter);
            var updateCostCenterDto = new UpdateCostCenterDto { Name = "Updated Cost Center", Code = "UCC2", IsActive = false };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/costcenters/{costCenter.Id}", updateCostCenterDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCostCenter_WhenCostCenterExists_DeletesCostCenter()
        {
            // Arrange
            var costCenter = new CostCenter { Name = "Delete Cost Center", Code = "DCC1", IsActive = true };
            await AddAsync(costCenter);

            // Act
            var response = await _client.DeleteAsync($"/api/costcenters/{costCenter.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
