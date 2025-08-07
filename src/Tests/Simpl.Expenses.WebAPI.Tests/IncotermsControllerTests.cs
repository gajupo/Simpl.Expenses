using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class IncotermsControllerTests : IntegrationTestBase
    {
        public IncotermsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetIncotermById_WhenIncotermExists_ReturnsOk()
        {
            // Arrange
            var incoterm = new Incoterm { Clave = "TEST", Description = "Test Incoterm" };
            await AddAsync(incoterm);

            // Act
            var response = await _client.GetAsync($"/api/incoterms/{incoterm.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var incotermDto = await response.Content.ReadFromJsonAsync<IncotermDto>();
            Assert.NotNull(incotermDto);
            Assert.Equal(incoterm.Id, incotermDto.Id);
        }

        [Fact]
        public async Task CreateIncoterm_WithValidData_CreatesIncoterm()
        {
            // Arrange
            var createIncotermDto = new CreateIncotermDto { Clave = "FOB", Description = "Free On Board" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/incoterms", createIncotermDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var incotermDto = await response.Content.ReadFromJsonAsync<IncotermDto>();
            Assert.NotNull(incotermDto);
            Assert.Equal(createIncotermDto.Clave, incotermDto.Clave);
        }

        [Fact]
        public async Task UpdateIncoterm_WithValidData_UpdatesIncoterm()
        {
            // Arrange
            var incoterm = new Incoterm { Clave = "UPD", Description = "Update Incoterm" };
            await AddAsync(incoterm);
            var updateIncotermDto = new UpdateIncotermDto { Clave = "UPD", Description = "Updated Incoterm" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/incoterms/{incoterm.Id}", updateIncotermDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteIncoterm_WhenIncotermExists_DeletesIncoterm()
        {
            // Arrange
            var incoterm = new Incoterm { Clave = "DEL", Description = "Delete Incoterm" };
            await AddAsync(incoterm);

            // Act
            var response = await _client.DeleteAsync($"/api/incoterms/{incoterm.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
