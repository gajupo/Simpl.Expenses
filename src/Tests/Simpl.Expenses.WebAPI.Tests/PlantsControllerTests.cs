using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class PlantsControllerTests : IntegrationTestBase
    {
        public PlantsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();
        }

        [Fact]
        public async Task GetPlantById_WhenPlantExists_ReturnsOk()
        {
            // Arrange
            var plant = new Plant { Name = "Test Plant" };
            await AddAsync(plant);

            // Act
            var response = await _client.GetAsync($"/api/plants/{plant.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var plantDto = await response.Content.ReadFromJsonAsync<PlantDto>();
            Assert.NotNull(plantDto);
            Assert.Equal(plant.Id, plantDto.Id);
        }

        [Fact]
        public async Task CreatePlant_WithValidData_CreatesPlant()
        {
            // Arrange
            var createPlantDto = new CreatePlantDto { Name = "New Plant" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/plants", createPlantDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var plantDto = await response.Content.ReadFromJsonAsync<PlantDto>();
            Assert.NotNull(plantDto);
            Assert.Equal(createPlantDto.Name, plantDto.Name);
        }

        [Fact]
        public async Task UpdatePlant_WithValidData_UpdatesPlant()
        {
            // Arrange
            var plant = new Plant { Name = "Update Plant" };
            await AddAsync(plant);
            var updatePlantDto = new UpdatePlantDto { Name = "Updated Plant" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/plants/{plant.Id}", updatePlantDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeletePlant_WhenPlantExists_DeletesPlant()
        {
            // Arrange
            var plant = new Plant { Name = "Delete Plant" };
            await AddAsync(plant);

            // Act
            var response = await _client.DeleteAsync($"/api/plants/{plant.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
