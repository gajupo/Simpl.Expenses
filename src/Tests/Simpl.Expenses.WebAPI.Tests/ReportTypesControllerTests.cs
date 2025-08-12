using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using System.Collections.Generic;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class ReportTypesControllerTests : IntegrationTestBase
    {
        private Workflow testWorkflow;

        public ReportTypesControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();

            testWorkflow = new Workflow { Name = "Test Workflow", Description = "Test Description" };
            await AddAsync(testWorkflow);
        }

        [Fact]
        public async Task GetReportTypeById_WhenReportTypeExists_ReturnsOk()
        {
            // Arrange
            var reportType = new ReportType { Name = "GetReportTypeById_WhenReportTypeExists_ReturnsOk", DefaultWorkflowId = testWorkflow.Id };
            await AddAsync(reportType);

            // Act
            var response = await _client.GetAsync($"/api/reporttypes/{reportType.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var reportTypeDto = await response.Content.ReadFromJsonAsync<ReportTypeDto>();
            Assert.NotNull(reportTypeDto);
            Assert.Equal(reportType.Id, reportTypeDto.Id);
            Assert.Equal(testWorkflow.Id, reportTypeDto.DefaultWorkflowId);
            Assert.Equal(testWorkflow.Name, reportTypeDto.DefaultWorkflowName);
        }

        [Fact]
        public async Task CreateReportType_WithValidData_CreatesReportType()
        {
            // Arrange
            var createReportTypeDto = new CreateReportTypeDto { Name = "New Report Type", DefaultWorkflowId = testWorkflow.Id };

            // Act
            var response = await _client.PostAsJsonAsync("/api/reporttypes", createReportTypeDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var reportTypeDto = await response.Content.ReadFromJsonAsync<ReportTypeDto>();
            Assert.NotNull(reportTypeDto);
            Assert.Equal(createReportTypeDto.Name, reportTypeDto.Name);
            Assert.Equal(testWorkflow.Id, reportTypeDto.DefaultWorkflowId);
            Assert.Equal(testWorkflow.Name, reportTypeDto.DefaultWorkflowName);
        }

        [Fact]
        public async Task UpdateReportType_WithValidData_UpdatesReportType()
        {
            // Arrange
            var reportType = new ReportType { Name = "Update Report Type", DefaultWorkflowId = testWorkflow.Id };
            await AddAsync(reportType);
            var updateReportTypeDto = new UpdateReportTypeDto { Name = "Updated Report Type", DefaultWorkflowId = null };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/reporttypes/{reportType.Id}", updateReportTypeDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        [Fact]
        public async Task DeleteReportType_WhenReportTypeExists_DeletesReportType()
        {
            // Arrange
            var reportType = new ReportType { Name = "Delete Report Type", DefaultWorkflowId = testWorkflow.Id };
            await AddAsync(reportType);

            // Act
            var response = await _client.DeleteAsync($"/api/reporttypes/{reportType.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
