using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class UsoCFDIsControllerTests : IntegrationTestBase
    {
        public UsoCFDIsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();
        }

        [Fact]
        public async Task GetUsoCFDIById_WhenUsoCFDIExists_ReturnsOk()
        {
            // Arrange
            var usoCFDI = new UsoCFDI { Clave = "TEST", Description = "Test UsoCFDI" };
            await AddAsync(usoCFDI);

            // Act
            var response = await _client.GetAsync($"/api/usocfdis/{usoCFDI.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var usoCFDIDto = await response.Content.ReadFromJsonAsync<UsoCFDIDto>();
            Assert.NotNull(usoCFDIDto);
            Assert.Equal(usoCFDI.Id, usoCFDIDto.Id);
        }

        [Fact]
        public async Task CreateUsoCFDI_WithValidData_CreatesUsoCFDI()
        {
            // Arrange
            var createUsoCFDIDto = new CreateUsoCFDIDto { Clave = "G02", Description = "Devoluciones, descuentos o bonificaciones" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/usocfdis", createUsoCFDIDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var usoCFDIDto = await response.Content.ReadFromJsonAsync<UsoCFDIDto>();
            Assert.NotNull(usoCFDIDto);
            Assert.Equal(createUsoCFDIDto.Clave, usoCFDIDto.Clave);
        }

        [Fact]
        public async Task UpdateUsoCFDI_WithValidData_UpdatesUsoCFDI()
        {
            // Arrange
            var usoCFDI = new UsoCFDI { Clave = "UPD", Description = "Update UsoCFDI" };
            await AddAsync(usoCFDI);
            var updateUsoCFDIDto = new UpdateUsoCFDIDto { Clave = "UPD", Description = "Updated UsoCFDI" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/usocfdis/{usoCFDI.Id}", updateUsoCFDIDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteUsoCFDI_WhenUsoCFDIExists_DeletesUsoCFDI()
        {
            // Arrange
            var usoCFDI = new UsoCFDI { Clave = "DEL", Description = "Delete UsoCFDI" };
            await AddAsync(usoCFDI);

            // Act
            var response = await _client.DeleteAsync($"/api/usocfdis/{usoCFDI.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
