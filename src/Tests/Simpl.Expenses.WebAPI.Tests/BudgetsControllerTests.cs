using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using Core.WebApi;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class BudgetsControllerTests : IntegrationTestBase
    {
        public BudgetsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();
        }

        [Fact]
        public async Task GetBudgetById_WhenBudgetExists_ReturnsOk()
        {
            // Arrange
            var budget = new Budget
            {
                Amount = 1000,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };
            var addedBudget = await AddAsync(budget);

            // Act
            var response = await _client.GetAsync($"/api/budgets/{addedBudget.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var budgetDto = await response.Content.ReadFromJsonAsync<BudgetDto>();
            Assert.NotNull(budgetDto);
            Assert.Equal(budget.Id, budgetDto.Id);
        }

        [Fact]
        public async Task GetAllBudgets_ReturnsOk()
        {
            // Arrange
            var budget = new Budget
            {
                Amount = 1000,
                StartDate = DateTime.UtcNow,
                EndDate = DateTime.UtcNow.AddMonths(1)
            };
            await AddAsync(budget);

            // Act
            var response = await _client.GetAsync("/api/budgets");
            var budgetsDto = await response.Content.ReadFromJsonAsync<List<BudgetDto>>();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(budgetsDto);
            Assert.NotEmpty(budgetsDto);
        }

        [Fact]
        public async Task CreateBudget_WithValidData_CreatesBudget()
        {
            // Arrange
            var createBudgetDto = new CreateBudgetDto { Amount = 2000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1) };

            // Act
            var response = await _client.PostAsJsonAsync("/api/budgets", createBudgetDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var budgetDto = await response.Content.ReadFromJsonAsync<BudgetDto>();
            Assert.NotNull(budgetDto);
            Assert.Equal(createBudgetDto.Amount, budgetDto.Amount);
        }

        [Fact]
        public async Task UpdateBudget_WithValidData_UpdatesBudget()
        {
            // Arrange
            var budget = new Budget { Amount = 1000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1) };
            await AddAsync(budget);
            var updateBudgetDto = new UpdateBudgetDto { Amount = 3000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1) };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/budgets/{budget.Id}", updateBudgetDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteBudget_WhenBudgetExists_DeletesBudget()
        {
            // Arrange
            var budget = new Budget { Amount = 1000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddMonths(1) };
            await AddAsync(budget);

            // Act
            var response = await _client.DeleteAsync($"/api/budgets/{budget.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
