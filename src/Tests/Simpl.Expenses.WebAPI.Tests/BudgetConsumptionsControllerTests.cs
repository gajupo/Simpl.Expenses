using System;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using Core.WebApi;
using System.Linq;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class BudgetConsumptionsControllerTests : IntegrationTestBase
    {
        public BudgetConsumptionsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();
        }

        [Fact]
        public async Task GetBudgetConsumptionById_WhenExists_ReturnsOk()
        {
            // Arrange
            var reportType = await AddAsync(new ReportType { Name = "Test Type" });
            var plant = await AddAsync(new Plant { Name = "Test Plant" });
            var category = await AddAsync(new Category { Name = "Test Category", Icon = "test-icon" });
            var budget = await AddAsync(new Budget { Amount = 1000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) });
            var report = await AddAsync(new Report { ReportNumber = "R001", Name = "Report 1", Amount = 100, Currency = "USD", BankName = "Test Bank", AccountNumber = "12345", Clabe = "123456789012345678", ReportTypeId = reportType.Id, PlantId = plant.Id, CategoryId = category.Id });
            var consumption = await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 50, ConsumptionDate = DateTime.UtcNow });

            // Act
            var response = await _client.GetAsync($"/api/budget-consumptions/{consumption.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<BudgetConsumptionDto>();
            Assert.NotNull(dto);
            Assert.Equal(consumption.Id, dto.Id);
            Assert.Equal(reportType.Name, dto.ReportType);

            // clean up budgetconsumption
            await DeleteAllAsync<BudgetConsumption>();
        }

        [Fact]
        public async Task GetAllBudgetConsumptions_ReturnsOk()
        {
            // Arrange
            var reportType = await AddAsync(new ReportType { Name = "Test Type" });
            var plant = await AddAsync(new Plant { Name = "Test Plant" });
            var category = await AddAsync(new Category { Name = "Test Category", Icon = "test-icon" });
            var budget = await AddAsync(new Budget { Amount = 1000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) });
            var report = await AddAsync(new Report { ReportNumber = "R001", Name = "Report 1", Amount = 100, Currency = "USD", BankName = "Test Bank", AccountNumber = "12345", Clabe = "123456789012345678", ReportTypeId = reportType.Id, PlantId = plant.Id, CategoryId = category.Id });
            await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 50, ConsumptionDate = DateTime.UtcNow });
            await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 75, ConsumptionDate = DateTime.UtcNow });

            // Act
            var response = await _client.GetAsync("/api/budget-consumptions");

            // Assert
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<BudgetConsumptionDto>>();
            Assert.NotNull(dtos);
            Assert.Equal(2, dtos.Count);

            // clean up budgetconsumption
            await DeleteAllAsync<BudgetConsumption>();
        }

        [Fact]
        public async Task GetByDateRange_ReturnsFilteredConsumptions()
        {
            // Arrange
            var reportType = await AddAsync(new ReportType { Name = "Test Type" });
            var plant = await AddAsync(new Plant { Name = "Test Plant" });
            var category = await AddAsync(new Category { Name = "Test Category", Icon = "test-icon" });
            var budget = await AddAsync(new Budget { Amount = 1000, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) });
            var report = await AddAsync(new Report { ReportNumber = "R001", Name = "Report 1", Amount = 100, Currency = "USD", BankName = "Test Bank", AccountNumber = "12345", Clabe = "123456789012345678", ReportTypeId = reportType.Id, PlantId = plant.Id, CategoryId = category.Id });
            await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 50, ConsumptionDate = DateTime.UtcNow.AddDays(-5) });
            await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 75, ConsumptionDate = DateTime.UtcNow });

            // Act
            var response = await _client.GetAsync($"/api/budget-consumptions/range?startDate={DateTime.UtcNow.AddDays(-1):yyyy-MM-dd}&endDate={DateTime.UtcNow.AddDays(1):yyyy-MM-dd}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<BudgetConsumptionDto>>();
            Assert.NotNull(dtos);
            Assert.Single(dtos);

            // clean up budgetconsumption
            await DeleteAllAsync<BudgetConsumption>();
        }

        [Fact]
        public async Task GetByDateRangeAndCostCenter_ReturnsFilteredConsumptions()
        {
            // Arrange
            var reportType = await AddAsync(new ReportType { Name = "Test Type" });
            var plant = await AddAsync(new Plant { Name = "Test Plant" });
            var category = await AddAsync(new Category { Name = "Test Category", Icon = "test-icon" });
            var costCenter = await AddAsync(new CostCenter { Name = "CC Test", Code = "CCT" });
            var budget = await AddAsync(new Budget { Amount = 1000, CostCenterId = costCenter.Id, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) });
            var report = await AddAsync(new Report { ReportNumber = "R001", Name = "Report 1", Amount = 100, Currency = "USD", BankName = "Test Bank", AccountNumber = "12345", Clabe = "123456789012345678", ReportTypeId = reportType.Id, PlantId = plant.Id, CategoryId = category.Id });
            await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 50, ConsumptionDate = DateTime.UtcNow });

            // Act
            var response = await _client.GetAsync($"/api/budget-consumptions/cost-center?startDate={DateTime.UtcNow.AddDays(-1):yyyy-MM-dd}&endDate={DateTime.UtcNow.AddDays(1):yyyy-MM-dd}&costCenterId={costCenter.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<BudgetConsumptionDto>>();
            Assert.NotNull(dtos);
            Assert.Single(dtos);

            // clean up budgetconsumption
            await DeleteAllAsync<BudgetConsumption>();
        }

        [Fact]
        public async Task GetByDateRangeAndAccountProject_ReturnsFilteredConsumptions()
        {
            // Arrange
            var reportType = await AddAsync(new ReportType { Name = "Test Type" });
            var plant = await AddAsync(new Plant { Name = "Test Plant" });
            var category = await AddAsync(new Category { Name = "Test Category", Icon = "test-icon" });
            var project = await AddAsync(new AccountProject { Name = "AP Test", Code = "APT" });
            var budget = await AddAsync(new Budget { Amount = 1000, AccountProjectId = project.Id, StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(30) });
            var report = await AddAsync(new Report { ReportNumber = "R001", Name = "Report 1", Amount = 100, Currency = "USD", BankName = "Test Bank", AccountNumber = "12345", Clabe = "123456789012345678", ReportTypeId = reportType.Id, PlantId = plant.Id, CategoryId = category.Id });
            await AddAsync(new BudgetConsumption { BudgetId = budget.Id, ReportId = report.Id, Amount = 50, ConsumptionDate = DateTime.UtcNow });

            // Act
            var response = await _client.GetAsync($"/api/budget-consumptions/project?startDate={DateTime.UtcNow.AddDays(-1):yyyy-MM-dd}&endDate={DateTime.UtcNow.AddDays(1):yyyy-MM-dd}&accountProjectId={project.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<BudgetConsumptionDto>>();
            Assert.NotNull(dtos);
            Assert.Single(dtos);

            // clean up budgetconsumption
            await DeleteAllAsync<BudgetConsumption>();
        }
    }
}
