using Simpl.Expenses.Application.Dtos.ReportState;
using Simpl.Expenses.Domain.Enums;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Simpl.Expenses.Application.Dtos.Report;
using Simpl.Expenses.Domain.Entities;
using Xunit;
using System.Collections.Generic;
using Core.WebApi;
using System.Linq;

namespace Simpl.Expenses.WebAPI.Tests
{
    public class ReportsControllerTests : IntegrationTestBase
    {
        public ReportsControllerTests(CustomWebApplicationFactory<Program> factory)
            : base(factory)
        {
        }

        public override async Task InitializeAsync()
        {
            await base.InitializeAsync();
            await LoginAsAdminAsync();
        }

        [Fact]
        public async Task CreateReport_WithPurchaseOrder_CreatesReportSuccessfully()
        {
            // Arrange
            var user = await FindAsync<User>(u => u.Username == "padmin");
            var plant = await FindAsync<Plant>(p => p.Name == "Plant 1");
            var category = await FindAsync<Category>(c => c.Name == "Travel");
            var costCenter = await FindAsync<CostCenter>(cc => cc.Code == "CC1");
            var usoCfdi = await FindAsync<UsoCFDI>(u => u.Clave == "G01");
            var incoterm = await FindAsync<Incoterm>(i => i.Clave == "FOB");

            var workflow = await AddAsync(new Workflow { Name = "PO Workflow", Description = "Workflow for Purchase Orders" });
            var step1 = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Step 1 PO", StepNumber = 1, ApproverRoleId = 1 });
            await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Step 2 PO", StepNumber = 2, ApproverRoleId = 1 });

            var reportType = await AddAsync(new ReportType { Name = "PO Report Type", DefaultWorkflowId = workflow.Id });

            var createReportDto = new CreateReportDto
            {
                Name = "Test Report",
                Amount = 100.50m,
                Currency = "USD",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                PurchaseOrderDetail = new CreatePurchaseOrderDetailDto
                {
                    UsoCfdiId = usoCfdi.Id,
                    IncotermId = incoterm.Id
                }
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/reports", createReportDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var reportDto = await response.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(reportDto);
            Assert.Equal(createReportDto.Name, reportDto.Name);
            Assert.NotNull(reportDto.PurchaseOrderDetail);
        }

        [Fact]
        public async Task GetReportById_WhenReportExists_ReturnsOk()
        {
            // Arrange
            var report = await CreateTestReport();

            // Act
            var response = await _client.GetAsync($"/api/reports/{report.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var reportDto = await response.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(reportDto);
            Assert.Equal(report.Id, reportDto.Id);
        }

        [Fact]
        public async Task GetAllReports_ReturnsOk()
        {
            // Arrange
            await CreateTestReport();

            // Act
            var response = await _client.GetAsync("/api/reports");

            // Assert
            response.EnsureSuccessStatusCode();
            var reports = await response.Content.ReadFromJsonAsync<List<ReportDto>>();
            Assert.NotNull(reports);
            Assert.True(reports.Count > 0);
        }

        [Fact]
        public async Task UpdateReport_WithValidData_UpdatesReport()
        {
            // Arrange
            var report = await CreateTestReport();
            var plant = await FindAsync<Plant>(p => p.Name == "Plant 2");
            var category = await FindAsync<Category>(c => c.Name == "Office Supplies");
            var costCenter = await FindAsync<CostCenter>(cc => cc.Code == "CC2");

            var updateDto = new UpdateReportDto
            {
                Name = "Updated Report Name",
                Amount = 300.00m,
                Currency = "GBP",
                UserId = report.UserId,
                ReportTypeId = report.ReportTypeId,
                PlantId = plant.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                BankName = report.BankName,
                AccountNumber = report.AccountNumber,
                Clabe = report.Clabe,
                ReimbursementDetail = new UpdateReimbursementDetailDto
                {
                    EmployeeName = "Updated Employee",
                    EmployeeNumber = "E456"
                }
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/reports/{report.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify update
            var getResponse = await _client.GetAsync($"/api/reports/{report.Id}");
            getResponse.EnsureSuccessStatusCode();
            var updatedReport = await getResponse.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(updatedReport);
            Assert.Equal(updateDto.Name, updatedReport.Name);
            Assert.Equal(updateDto.Amount, updatedReport.Amount);
            Assert.Equal(updateDto.Currency, updatedReport.Currency);
            Assert.Equal(updateDto.BankName, updatedReport.BankName);
            Assert.Equal(updateDto.AccountNumber, updatedReport.AccountNumber);
            Assert.Equal(updateDto.Clabe, updatedReport.Clabe);
            Assert.Equal(updateDto.ReimbursementDetail.EmployeeName, updatedReport.ReimbursementDetail.EmployeeName);
            Assert.Equal(updateDto.ReimbursementDetail.EmployeeNumber, updatedReport.ReimbursementDetail.EmployeeNumber);
        }

        [Fact]
        public async Task UpdateReport_WithValidDataOfPurchaseOrder_UpdatesReport()
        {
            // Arrange
            var report = await CreateTestReport();
            var usoCfdi = await FindAsync<UsoCFDI>(u => u.Clave == "G01");
            var incoterm = await FindAsync<Incoterm>(i => i.Clave == "CIF");
            var accountProject = await FindAsync<AccountProject>(ap => ap.Code == "AP1");

            var updateDto = new UpdateReportDto
            {
                Name = "Updated Purchase Order Report",
                Amount = 500.00m,
                Currency = "USD",
                UserId = report.UserId,
                ReportTypeId = report.ReportTypeId,
                PlantId = report.PlantId,
                CategoryId = report.CategoryId,
                CostCenterId = report.CostCenterId,
                BankName = report.BankName,
                AccountNumber = report.AccountNumber,
                Clabe = report.Clabe,
                AccountProjectId = accountProject.Id,
                PurchaseOrderDetail = new UpdatePurchaseOrderDetailDto
                {
                    UsoCfdiId = usoCfdi.Id,
                    IncotermId = incoterm.Id
                }
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/reports/{report.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify update
            var getResponse = await _client.GetAsync($"/api/reports/{report.Id}");
            getResponse.EnsureSuccessStatusCode();
            var updatedReport = await getResponse.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(updatedReport);
            Assert.Equal(updateDto.Name, updatedReport.Name);
            Assert.Equal(updateDto.Amount, updatedReport.Amount);
            Assert.Equal(updateDto.Currency, updatedReport.Currency);
            Assert.NotNull(updatedReport.PurchaseOrderDetail);
            Assert.Equal(usoCfdi.Id, updatedReport.PurchaseOrderDetail.UsoCfdiId);
            Assert.Equal(incoterm.Id, updatedReport.PurchaseOrderDetail.IncotermId);
            Assert.Equal(accountProject.Id, updatedReport.AccountProjectId);
        }

        [Fact]
        public async Task UpdateReport_WithValidDataOfAdvancePayment_UpdatesReport()
        {
            // Arrange
            var report = await CreateTestReport();
            var accountProject = await FindAsync<AccountProject>(ap => ap.Code == "AP2");

            var updateDto = new UpdateReportDto
            {
                Name = "Updated Advance Payment Report",
                Amount = 700.00m,
                Currency = "USD",
                UserId = report.UserId,
                ReportTypeId = report.ReportTypeId,
                PlantId = report.PlantId,
                CategoryId = report.CategoryId,
                CostCenterId = report.CostCenterId,
                BankName = report.BankName,
                AccountNumber = report.AccountNumber,
                Clabe = report.Clabe,
                AccountProjectId = accountProject.Id,
                AdvancePaymentDetail = new UpdateAdvancePaymentDetailDto
                {
                    OrderNumber = "AP-12345"
                }
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/reports/{report.Id}", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify update
            var getResponse = await _client.GetAsync($"/api/reports/{report.Id}");
            getResponse.EnsureSuccessStatusCode();
            var updatedReport = await getResponse.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(updatedReport);
            Assert.Equal(updateDto.Name, updatedReport.Name);
            Assert.Equal(updateDto.Amount, updatedReport.Amount);
            Assert.Equal(updateDto.Currency, updatedReport.Currency);
            Assert.NotNull(updatedReport.AdvancePaymentDetail);
            Assert.Equal(updateDto.AdvancePaymentDetail.OrderNumber, updatedReport.AdvancePaymentDetail.OrderNumber);
        }

        [Fact]
        public async Task DeleteReport_WhenReportExists_DeletesReport()
        {
            // Arrange
            var report = await CreateTestReport();

            // Act
            var response = await _client.DeleteAsync($"/api/reports/{report.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            // Verify deletion
            var getResponse = await _client.GetAsync($"/api/reports/{report.Id}");
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
        }

        private async Task<Report> CreateTestReport()
        {
            var user = await FindAsync<User>(u => u.Username == "testuser");
            var reportType = await FindAsync<ReportType>(rt => rt.Name == "Reimbursement");
            var plant = await FindAsync<Plant>(p => p.Name == "Plant 1");
            var category = await FindAsync<Category>(c => c.Name == "Travel");
            var costCenter = await FindAsync<CostCenter>(cc => cc.Code == "CC1");

            var report = new Report
            {
                Name = "Test Report for Get",
                Amount = 200.00m,
                Currency = "EUR",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "24-00001",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReimbursementDetail = new ReimbursementDetail
                {
                    EmployeeName = "Test Employee",
                    EmployeeNumber = "E123"
                }
            };
            await AddAsync(report);
            return report;
        }

        [Fact]
        public async Task CreateReport_ShouldCreateInitialReportState()
        {
            // Arrange
            var user = await FindAsync<User>(u => u.Username == "padmin");
            var plant = await FindAsync<Plant>(p => p.Name == "Plant 1");
            var category = await FindAsync<Category>(c => c.Name == "Travel");
            var costCenter = await FindAsync<CostCenter>(cc => cc.Code == "CC1");

            var workflow = await AddAsync(new Workflow { Name = "Test Workflow for Report State", Description = "Test Workflow Description" });
            var step1 = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Step 1", StepNumber = 1, ApproverRoleId = 1 });

            var reportType = await AddAsync(new ReportType { Name = "Test Report Type with Workflow", DefaultWorkflowId = workflow.Id });

            var createReportDto = new CreateReportDto
            {
                Name = "Test Report for State",
                Amount = 150,
                Currency = "EUR",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                AccountProjectId = 1,
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/reports", createReportDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var report = await response.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(report);

            // Now, verify the state was created, /state returns the report state now
            var stateResponse = await _client.GetAsync($"/api/reports/{report.Id}/state");
            stateResponse.EnsureSuccessStatusCode();
            var reportState = await stateResponse.Content.ReadFromJsonAsync<ReportStateDto>();

            Assert.NotNull(reportState);
            Assert.Equal(report.Id, reportState.ReportId);
            Assert.Equal(ReportStatus.Submitted, reportState.Status);
            Assert.Equal(workflow.Id, reportState.WorkflowId);
            Assert.Equal(step1.Id, reportState.CurrentStepId);
        }

        [Fact]
        public async Task CreateReportState_ShouldUpdateReportState()
        {
            // Arrange
            var report = await CreateTestReportWithWorkflow();
            var workflow = await FindAsync<Workflow>(w => w.Name == "PO Workflow");
            var step2 = await FindAsync<WorkflowStep>(s => s.Name == "Step 2 PO");

            var createReportStateDto = new CreateReportStateDto
            {
                ReportId = report.Item1.Id,
                WorkflowId = workflow.Id,
                CurrentStepId = step2.Id,
                Status = ReportStatus.Approved
            };

            // Act
            var response = await _client.PostAsJsonAsync($"/api/reports/{report.Item1.Id}/state", createReportStateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var reportState = await response.Content.ReadFromJsonAsync<ReportStateDto>();

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(reportState);
            Assert.Equal(report.Item1.Id, reportState.ReportId);
            Assert.Equal(ReportStatus.Approved, reportState.Status);
            Assert.NotEqual(report.Item2.CurrentStepId, reportState.CurrentStepId); // Ensure step has changed
            Assert.Equal(step2.Id, reportState.CurrentStepId);
        }

        private async Task<(Report, ReportStateDto)> CreateTestReportWithWorkflow()
        {
            var user = await FindAsync<User>(u => u.Username == "testuser");
            var plant = await FindAsync<Plant>(p => p.Name == "Plant 1");
            var category = await FindAsync<Category>(c => c.Name == "Travel");
            var costCenter = await FindAsync<CostCenter>(cc => cc.Code == "CC1");

            var workflow = await AddAsync(new Workflow { Name = "PO Workflow", Description = "Workflow for Purchase Orders" });
            var step1 = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Step 1 PO", StepNumber = 1, ApproverRoleId = 1 });
            await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Step 2 PO", StepNumber = 2, ApproverRoleId = 1 });

            var reportType = await AddAsync(new ReportType { Name = "PO Report Type", DefaultWorkflowId = workflow.Id });

            var report = new Report
            {
                Name = "Test Report for State Update",
                Amount = 200.00m,
                Currency = "EUR",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "24-00002",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                AccountProjectId = 1,
                Clabe = "123456789012345678"
            };
            await AddAsync(report);

            // Create initial state
            var createReportStateDto = new CreateReportStateDto
            {
                ReportId = report.Id,
                WorkflowId = workflow.Id,
                CurrentStepId = step1.Id,
                Status = ReportStatus.Submitted
            };
            var createStateResponse = await _client.PostAsJsonAsync($"/api/reports/{report.Id}/state", createReportStateDto);
            createStateResponse.EnsureSuccessStatusCode();
            var createdReportState = await createStateResponse.Content.ReadFromJsonAsync<ReportStateDto>();

            return (report, createdReportState);
        }
    }
}
