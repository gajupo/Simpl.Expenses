using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class CategoriesControllerTests : IntegrationTestBase
    {
        public CategoriesControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetCategoryById_WhenCategoryExists_ReturnsOk()
        {
            // Arrange
            var category = new Category { Name = "Test Category", Icon = "test" };
            await AddAsync(category);

            // Act
            var response = await _client.GetAsync($"/api/categories/{category.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var categoryDto = await response.Content.ReadFromJsonAsync<CategoryDto>();
            Assert.NotNull(categoryDto);
            Assert.Equal(category.Id, categoryDto.Id);
        }

        [Fact]
        public async Task CreateCategory_WithValidData_CreatesCategory()
        {
            // Arrange
            var createCategoryDto = new CreateCategoryDto { Name = "New Category", Icon = "new" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/categories", createCategoryDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var categoryDto = await response.Content.ReadFromJsonAsync<CategoryDto>();
            Assert.NotNull(categoryDto);
            Assert.Equal(createCategoryDto.Name, categoryDto.Name);
        }

        [Fact]
        public async Task UpdateCategory_WithValidData_UpdatesCategory()
        {
            // Arrange
            var category = new Category { Name = "Update Category", Icon = "update" };
            await AddAsync(category);
            var updateCategoryDto = new UpdateCategoryDto { Name = "Updated Category", Icon = "updated" };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/categories/{category.Id}", updateCategoryDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryExists_DeletesCategory()
        {
            // Arrange
            var category = new Category { Name = "Delete Category", Icon = "delete" };
            await AddAsync(category);

            // Act
            var response = await _client.DeleteAsync($"/api/categories/{category.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
