using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Core.WebApi;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class ReportTypesControllerTests : IntegrationTestBase
    {
        public ReportTypesControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        [Fact]
        public async Task GetReportTypeById_WhenReportTypeExists_ReturnsOk()
        {
            // Arrange
            var reportType = new ReportType { Name = "GetReportTypeById_WhenReportTypeExists_ReturnsOk" };
            await AddAsync(reportType);

            // Act
            var response = await _client.GetAsync($"/api/reporttypes/{reportType.Id}");
            var allReportsResponse = await _client.GetAsync("/api/reporttypes");
            var allReportTypes = await allReportsResponse.Content.ReadFromJsonAsync<List<ReportTypeDto>>();


            // Assert
            response.EnsureSuccessStatusCode();
            var reportTypeDto = await response.Content.ReadFromJsonAsync<ReportTypeDto>();
            Assert.NotNull(reportTypeDto);
            Assert.Equal(reportType.Id, reportTypeDto.Id);
        }

        [Fact]
        public async Task CreateReportType_WithValidData_CreatesReportType()
        {
            // Arrange
            var createReportTypeDto = new CreateReportTypeDto { Name = "New Report Type" };

            // Act
            var response = await _client.PostAsJsonAsync("/api/reporttypes", createReportTypeDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var reportTypeDto = await response.Content.ReadFromJsonAsync<ReportTypeDto>();
            Assert.NotNull(reportTypeDto);
            Assert.Equal(createReportTypeDto.Name, reportTypeDto.Name);
        }

        [Fact]
        public async Task UpdateReportType_WithValidData_UpdatesReportType()
        {
            // Arrange
            var reportType = new ReportType { Name = "Update Report Type" };
            await AddAsync(reportType);
            var updateReportTypeDto = new UpdateReportTypeDto { Name = "Updated Report Type" };

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
            var reportType = new ReportType { Name = "Delete Report Type" };
            await AddAsync(reportType);

            // Act
            var response = await _client.DeleteAsync($"/api/reporttypes/{reportType.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }
    }
}
