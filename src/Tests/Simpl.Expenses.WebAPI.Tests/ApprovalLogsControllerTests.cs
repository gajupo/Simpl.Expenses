using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Simpl.Expenses.Domain.Enums;
using Xunit;
using System.Collections.Generic;
using Core.WebApi;
using System;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class ApprovalLogsControllerTests : IntegrationTestBase
    {
        private User testUser;
        private Report testReport;

        public ApprovalLogsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();

            testUser = new User { Username = "testuser", Email = "test@user.com", PasswordHash = "test" };
            await AddAsync(testUser);

            var reportType = new ReportType { Name = "Test Report Type" };
            await AddAsync(reportType);

            var plant = new Plant { Name = "Test Plant" };
            await AddAsync(plant);

            var category = new Category { Name = "Test Category", Icon = "test-icon" };
            await AddAsync(category);

            testReport = new Report
            {
                Name = "Test Report",
                Amount = 100,
                UserId = testUser.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant.Id,
                CategoryId = category.Id,
                ReportNumber = "25-00001",
                AccountNumber = "1234567890",
                BankName = "Test Bank",
                Currency = "USD",
                Clabe = "123456789012345678"
            };
            await AddAsync(testReport);
        }

        [Fact]
        public async Task GetApprovalLogById_WhenExists_ReturnsOk()
        {
            // Arrange
            var approvalLog = new ApprovalLog
            {
                ReportId = testReport.Id,
                UserId = testUser.Id,
                Action = ApprovalAction.Approved,
                Comment = "Test Comment",
                LogDate = DateTime.UtcNow
            };
            await AddAsync(approvalLog);

            // Act
            var response = await _client.GetAsync($"/api/approval-logs/{approvalLog.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var dto = await response.Content.ReadFromJsonAsync<ApprovalLogDto>();
            Assert.NotNull(dto);
            Assert.Equal(approvalLog.Id, dto.Id);
        }

        [Fact]
        public async Task GetAllApprovalLogs_ReturnsOk()
        {
            // Arrange
            var approvalLog = new ApprovalLog
            {
                ReportId = testReport.Id,
                UserId = testUser.Id,
                Action = ApprovalAction.Approved,
                Comment = "Test Comment",
                LogDate = DateTime.UtcNow
            };
            await AddAsync(approvalLog);

            // Act
            var response = await _client.GetAsync("/api/approval-logs");

            // Assert
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<ApprovalLogDto>>();
            Assert.NotNull(dtos);
            Assert.NotEmpty(dtos);
        }

        [Fact]
        public async Task CreateApprovalLog_WithValidData_CreatesAndReturnsDto()
        {
            // Arrange
            var createDto = new CreateApprovalLogDto
            {
                ReportId = testReport.Id,
                UserId = testUser.Id,
                Action = ApprovalAction.Submitted,
                Comment = "New Log"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/approval-logs", createDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var dto = await response.Content.ReadFromJsonAsync<ApprovalLogDto>();
            Assert.NotNull(dto);
            Assert.Equal(createDto.Comment, dto.Comment);
        }

        [Fact]
        public async Task UpdateApprovalLog_WithValidData_UpdatesAndReturnsNoContent()
        {
            // Arrange
            var approvalLog = new ApprovalLog
            {
                ReportId = testReport.Id,
                UserId = testUser.Id,
                Action = ApprovalAction.Approved,
                Comment = "Original Comment",
                LogDate = DateTime.UtcNow
            };
            await AddAsync(approvalLog);
            var updateDto = new UpdateApprovalLogDto
            {
                Action = ApprovalAction.Rejected,
                Comment = "Updated Comment"
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/approval-logs/{approvalLog.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteApprovalLog_WhenExists_DeletesAndReturnsNoContent()
        {
            // Arrange
            var approvalLog = new ApprovalLog
            {
                ReportId = testReport.Id,
                UserId = testUser.Id,
                Action = ApprovalAction.Approved,
                Comment = "To Be Deleted",
                LogDate = DateTime.UtcNow
            };
            await AddAsync(approvalLog);

            // Act
            var response = await _client.DeleteAsync($"/api/approval-logs/{approvalLog.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
