using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Simpl.Expenses.Application.Dtos;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using Core.WebApi;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class ReportAttachmentsControllerTests : IntegrationTestBase, IDisposable
    {
        private readonly string _tempUploadPath;
        private User testUser;
        private Report testReport;

        public ReportAttachmentsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
            _tempUploadPath = Path.Combine(Path.GetTempPath(), "simple-expenses-tests", Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempUploadPath);

            // Override appsettings.json to use the temp path
            _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, conf) =>
                {
                    conf.AddInMemoryCollection(new Dictionary<string, string>
                    {
                        {"FileStorage:BasePath", _tempUploadPath}
                    });
                });
            });
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

        public void Dispose()
        {
            if (Directory.Exists(_tempUploadPath))
            {
                //Directory.Delete(_tempUploadPath, true);
            }
        }

        [Fact]
        public async Task UploadAttachment_WithValidFile_UploadsAndReturnsDto()
        {
            // Arrange
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3 });
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            content.Add(fileContent, "files", "test.pdf");

            // Act
            var response = await _client.PostAsync($"/api/report-attachments/report/{testReport.Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dtos = await response.Content.ReadFromJsonAsync<List<ReportAttachmentDto>>();
            Assert.NotNull(dtos);
            Assert.Single(dtos);
            Assert.Equal("test.pdf", dtos[0].FileName);
        }

        [Fact]
        public async Task UploadAttachments_WithMultipleFiles_UploadsAndReturnsDtos()
        {
            // Arrange
            var content = new MultipartFormDataContent();

            var fileContent1 = new ByteArrayContent(new byte[] { 1, 2, 3 });
            fileContent1.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            content.Add(fileContent1, "files", "test1.pdf");

            var fileContent2 = new ByteArrayContent(new byte[] { 4, 5, 6 });
            fileContent2.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            content.Add(fileContent2, "files", "test2.png");

            // Act
            var response = await _client.PostAsync($"/api/report-attachments/report/{testReport.Id}", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var dtos = await response.Content.ReadFromJsonAsync<List<ReportAttachmentDto>>();
            Assert.NotNull(dtos);
            Assert.Equal(2, dtos.Count);
        }

        [Fact]
        public async Task DownloadAttachment_WhenExists_ReturnsFileStream()
        {
            // Arrange
            var attachment = await UploadTestFileAsync("download-test.pdf");

            // Act
            var response = await _client.GetAsync($"/api/report-attachments/{attachment.Id}/download");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal("application/pdf", response.Content.Headers.ContentType.ToString());
            Assert.Equal("attachment; filename=download-test.pdf; filename*=UTF-8''download-test.pdf", response.Content.Headers.ContentDisposition.ToString());
        }

        private async Task<ReportAttachmentDto> UploadTestFileAsync(string fileName)
        {
            var content = new MultipartFormDataContent();
            var fileContent = new ByteArrayContent(new byte[] { 1, 2, 3, 4, 5 });
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            content.Add(fileContent, "files", fileName);

            var response = await _client.PostAsync($"/api/report-attachments/report/{testReport.Id}", content);
            response.EnsureSuccessStatusCode();
            var dtos = await response.Content.ReadFromJsonAsync<List<ReportAttachmentDto>>();
            return dtos[0];
        }
    }
}
