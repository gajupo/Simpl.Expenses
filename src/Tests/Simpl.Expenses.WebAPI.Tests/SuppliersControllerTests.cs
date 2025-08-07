using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class SuppliersControllerTests : IntegrationTestBase
    {
        public SuppliersControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetSupplierById_WhenSupplierExists_ReturnsOk()
        {
            // Arrange
            var supplier = new Supplier
            {
                Name = "Test Supplier",
                IsActive = true,
                Email = "test@supplier.com",
                Address = "123 Test St",
                Rfc = "TESTRFC"
            };
            await AddAsync(supplier);

            // Act
            var response = await _client.GetAsync($"/api/suppliers/{supplier.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var supplierDto = await response.Content.ReadFromJsonAsync<SupplierDto>();
            Assert.NotNull(supplierDto);
            Assert.Equal(supplier.Id, supplierDto.Id);
        }

        [Fact]
        public async Task CreateSupplier_WithValidData_CreatesSupplier()
        {
            // Arrange
            var createSupplierDto = new CreateSupplierDto
            {
                Name = "New Supplier",
                IsActive = true,
                Email = "new@supplier.com",
                Address = "456 New St",
                Rfc = "NEWRFC"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/suppliers", createSupplierDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var supplierDto = await response.Content.ReadFromJsonAsync<SupplierDto>();
            Assert.NotNull(supplierDto);
            Assert.Equal(createSupplierDto.Name, supplierDto.Name);
        }

        [Fact]
        public async Task UpdateSupplier_WithValidData_UpdatesSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                Name = "Update Supplier",
                IsActive = true,
                Email = "update@supplier.com",
                Address = "789 Update St",
                Rfc = "UPDATERC"
            };
            await AddAsync(supplier);
            var updateSupplierDto = new UpdateSupplierDto
            {
                Name = "Update Supplier",
                IsActive = true,
                Email = "update@supplier.com",
                Address = "789 Update St",
                Rfc = "UPDATERC"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/suppliers/{supplier.Id}", updateSupplierDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteSupplier_WhenSupplierExists_DeletesSupplier()
        {
            // Arrange
            var supplier = new Supplier
            {
                Name = "Delete Supplier",
                IsActive = true,
                Email = "delete@supplier.com",
                Address = "123 Delete St",
                Rfc = "DELETERFC"
            };
            await AddAsync(supplier);

            // Act
            var response = await _client.DeleteAsync($"/api/suppliers/{supplier.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
