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
            var user = await GetFirstAsync<User>(u => u.Username == "padmin");
            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");
            var category = await GetFirstAsync<Category>(c => c.Name == "Travel");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC1");
            var usoCfdi = await GetFirstAsync<UsoCFDI>(u => u.Clave == "G01");
            var incoterm = await GetFirstAsync<Incoterm>(i => i.Clave == "FOB");

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
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow,
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
            Assert.Equal(report.Plant.Name, reportDto.PlantName);
            Assert.Equal(report.Category.Name, reportDto.CategoryName);
            Assert.Equal(report.CostCenter.Name, reportDto.CostCenterName);
            Assert.Equal(report.ReportNumber, reportDto.ReportNumber);
            Assert.Equal(report.Supplier.Name, reportDto.SupplierName);
            Assert.Equal(report.ReportType.Name, reportDto.ReportTypeName);
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
            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 2");
            var category = await GetFirstAsync<Category>(c => c.Name == "Office Supplies");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC2");

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
                ReportDescription = report.ReportDescription,
                ReportDate = report.ReportDate,
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
            var usoCfdi = await GetFirstAsync<UsoCFDI>(u => u.Clave == "G01");
            var incoterm = await GetFirstAsync<Incoterm>(i => i.Clave == "CIF");
            var accountProject = await GetFirstAsync<AccountProject>(ap => ap.Code == "AP1");

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
                ReportDescription = report.ReportDescription,
                ReportDate = report.ReportDate,
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
            var accountProject = await GetFirstAsync<AccountProject>(ap => ap.Code == "AP2");

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
                ReportDescription = report.ReportDescription,
                ReportDate = report.ReportDate,
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
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        }

        private async Task<Report> CreateTestReport()
        {
            var user = await GetFirstAsync<User>(u => u.Username == "testuser");
            var reportType = await GetFirstAsync<ReportType>(rt => rt.Name == "Reimbursement");
            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");
            var category = await GetFirstAsync<Category>(c => c.Name == "Travel");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC1");
            var supplier = await GetFirstAsync<Supplier>(s => s.Name == "Supplier 1");


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
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow,
                ReimbursementDetail = new ReimbursementDetail
                {
                    EmployeeName = "Test Employee",
                    EmployeeNumber = "E123"
                },
                SupplierId = supplier.Id
            };
            await AddAsync(report);
            return report;
        }

        [Fact]
        public async Task CreateReport_ShouldCreateInitialReportState()
        {
            // Arrange
            var user = await GetFirstAsync<User>(u => u.Username == "padmin");
            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");
            var category = await GetFirstAsync<Category>(c => c.Name == "Travel");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC1");

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
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
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
            var workflow = await GetFirstAsync<Workflow>(w => w.Name == "PO Workflow");
            var step2 = await GetFirstAsync<WorkflowStep>(s => s.Name == "Step 2 PO");

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
            var user = await GetFirstAsync<User>(u => u.Username == "testuser");
            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");
            var category = await GetFirstAsync<Category>(c => c.Name == "Travel");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC1");

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
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
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

        [Fact]
        public async Task GetReportOverviewByUserId_WhenUserHasReports_ReturnsReportsOverview()
        {
            // Arrange
            var user = await GetFirstAsync<User>(u => u.Username == "testuser");

            // Create a report with workflow and state
            var reportWithState = await CreateTestReportWithWorkflow();
            var report1 = reportWithState.Item1;
            var reportState = reportWithState.Item2;

            // Create another simple report
            var report2 = await CreateTestReport();

            // Act
            var response = await _client.GetAsync($"/api/reports/overview_by_user/{user.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var reportsOverview = await response.Content.ReadFromJsonAsync<List<ReportOverviewDto>>();
            Assert.NotNull(reportsOverview);
            Assert.True(reportsOverview.Count >= 2);

            // Test report with state
            var reportOverviewWithState = reportsOverview.FirstOrDefault(r => r.Id == report1.Id);
            Assert.NotNull(reportOverviewWithState);
            Assert.Equal(report1.Id, reportOverviewWithState.Id);
            Assert.Equal(report1.ReportNumber, reportOverviewWithState.ReportNumber);
            Assert.Equal(report1.Name, reportOverviewWithState.Name);
            Assert.Equal(report1.Amount, reportOverviewWithState.Amount);
            Assert.Equal(report1.Currency, reportOverviewWithState.Currency);
            Assert.Equal(report1.UserId, reportOverviewWithState.UserId);
            Assert.Equal(report1.ReportTypeId, reportOverviewWithState.ReportTypeId);
            Assert.NotNull(reportOverviewWithState.ReportTypeName);
            Assert.Equal(report1.PlantId, reportOverviewWithState.PlantId);
            Assert.NotNull(reportOverviewWithState.PlantName);
            Assert.Equal(report1.CategoryId, reportOverviewWithState.CategoryId);
            Assert.NotNull(reportOverviewWithState.CategoryName);
            Assert.Equal(report1.CreatedAt.Date, reportOverviewWithState.CreatedAt.Date);

            // Test Report State information
            Assert.NotNull(reportOverviewWithState.Status);
            Assert.Equal(reportState.Status.ToString(), reportOverviewWithState.Status);
            Assert.NotNull(reportOverviewWithState.CurrentStepId);
            Assert.Equal(reportState.CurrentStepId, reportOverviewWithState.CurrentStepId);
            Assert.NotNull(reportOverviewWithState.CurrentStepName);

            // Test report without state
            var reportOverviewWithoutState = reportsOverview.FirstOrDefault(r => r.Id == report2.Id);
            Assert.NotNull(reportOverviewWithoutState);
            Assert.Equal(report2.Id, reportOverviewWithoutState.Id);
            Assert.Null(reportOverviewWithoutState.Status);
            Assert.Null(reportOverviewWithoutState.CurrentStepId);
            Assert.Null(reportOverviewWithoutState.CurrentStepName);
        }

        [Fact]
        public async Task GetReportOverviewByUserId_WhenUserHasNoReports_ReturnsEmptyList()
        {
            // Arrange
            var user = await AddAsync(new User
            {
                Username = "testuser2",
                Name = "Test User 2",
                Email = "test2@example.com",
                PasswordHash = "hash",
                RoleId = 1,
                DepartmentId = 1,
                IsActive = true
            });

            // Act
            var response = await _client.GetAsync($"/api/reports/overview_by_user/{user.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var reportsOverview = await response.Content.ReadFromJsonAsync<List<ReportOverviewDto>>();
            Assert.NotNull(reportsOverview);
            Assert.Empty(reportsOverview);
        }

        [Fact]
        public async Task GetPendingApprovalCount_WhenUserHasPendingReports_ReturnsCorrectCount()
        {
            // Arrange
            var removedReports = await DeleteAllAsync<Report>();
            var user = await GetFirstAsync<User>(u => u.Username == "testuser");
            var plant1 = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");
            var plant2 = await GetFirstAsync<Plant>(p => p.Name == "Plant 2");
            var category = await GetFirstAsync<Category>(c => c.Name == "Travel");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC1");

            // Create workflow and report type
            var workflow = await AddAsync(new Workflow { Name = "Approval Workflow", Description = "Test Approval Workflow" });
            var step1 = await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Initial Step", StepNumber = 1, ApproverRoleId = 1 });
            var reportType = await AddAsync(new ReportType { Name = "Test Report Type for Approval", DefaultWorkflowId = workflow.Id });

            // Create 3 reports for user in plant1 with Submitted status (pending approval)
            var report1 = await AddAsync(new Report
            {
                Name = "Pending Report 1",
                Amount = 100m,
                Currency = "USD",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant1.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "25-00001",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
            });

            var report2 = await AddAsync(new Report
            {
                Name = "Pending Report 2",
                Amount = 200m,
                Currency = "USD",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant1.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "25-00002",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
            });

            var report3 = await AddAsync(new Report
            {
                Name = "Pending Report 3",
                Amount = 300m,
                Currency = "USD",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant2.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "25-00003",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
            });

            // Create 1 report for user in plant1 with Approved status (not pending)
            var report4 = await AddAsync(new Report
            {
                Name = "Approved Report",
                Amount = 400m,
                Currency = "USD",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant1.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "25-00004",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
            });

            // Create report states - 3 with Submitted status, 1 with Approved status
            await AddAsync(new ReportState { ReportId = report1.Id, WorkflowId = workflow.Id, CurrentStepId = step1.Id, Status = ReportStatus.Submitted, UpdatedAt = DateTime.UtcNow });
            await AddAsync(new ReportState { ReportId = report2.Id, WorkflowId = workflow.Id, CurrentStepId = step1.Id, Status = ReportStatus.Submitted, UpdatedAt = DateTime.UtcNow });
            await AddAsync(new ReportState { ReportId = report3.Id, WorkflowId = workflow.Id, CurrentStepId = step1.Id, Status = ReportStatus.Submitted, UpdatedAt = DateTime.UtcNow });
            await AddAsync(new ReportState { ReportId = report4.Id, WorkflowId = workflow.Id, CurrentStepId = step1.Id, Status = ReportStatus.Approved, UpdatedAt = DateTime.UtcNow });

            // Create report for different user (should not be counted)
            var otherUser = await AddAsync(new User
            {
                Username = "otheruser",
                Name = "Other User",
                Email = "other@example.com",
                PasswordHash = "hash",
                RoleId = 1,
                DepartmentId = 1,
                IsActive = true
            });

            var reportOtherUser = await AddAsync(new Report
            {
                Name = "Other User Report",
                Amount = 500m,
                Currency = "USD",
                UserId = otherUser.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant1.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                ReportNumber = "25-00005",
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
            });

            await AddAsync(new ReportState { ReportId = reportOtherUser.Id, WorkflowId = workflow.Id, CurrentStepId = step1.Id, Status = ReportStatus.Submitted, UpdatedAt = DateTime.UtcNow });

            // Act & Assert - Test with plant1 only (should return 2)
            var plant1Ids = new int[] { plant1.Id };
            var response1 = await _client.GetAsync($"/api/reports/pending_approval_count/{user.Id}?plantIds={plant1.Id}");
            response1.EnsureSuccessStatusCode();
            var count1 = await response1.Content.ReadFromJsonAsync<int>();
            Assert.Equal(2, count1); // report1 and report2 are pending in plant1

            // Act & Assert - Test with plant2 only (should return 1)
            var plant2Ids = new int[] { plant2.Id };
            var response2 = await _client.GetAsync($"/api/reports/pending_approval_count/{user.Id}?plantIds={plant2.Id}");
            response2.EnsureSuccessStatusCode();
            var count2 = await response2.Content.ReadFromJsonAsync<int>();
            Assert.Equal(1, count2); // report3 is pending in plant2

            // Act & Assert - Test with both plants (should return 3)
            var bothPlantIds = new int[] { plant1.Id, plant2.Id };
            var response3 = await _client.GetAsync($"/api/reports/pending_approval_count/{user.Id}?plantIds={plant1.Id}&plantIds={plant2.Id}");
            response3.EnsureSuccessStatusCode();
            var count3 = await response3.Content.ReadFromJsonAsync<int>();
            Assert.Equal(3, count3); // report1, report2, and report3 are pending

            // Act & Assert - Test with non-existent plant (should return 0)
            var nonExistentPlantId = 99999;
            var response4 = await _client.GetAsync($"/api/reports/pending_approval_count/{user.Id}?plantIds={nonExistentPlantId}");
            response4.EnsureSuccessStatusCode();
            var count4 = await response4.Content.ReadFromJsonAsync<int>();
            Assert.Equal(0, count4); // no reports in non-existent plant

            // Act & Assert - Test with empty plant array (should return 0)
            var response5 = await _client.GetAsync($"/api/reports/pending_approval_count/{user.Id}");
            response5.EnsureSuccessStatusCode();
            var count5 = await response5.Content.ReadFromJsonAsync<int>();
            Assert.Equal(0, count5); // no plantIds provided should return 0

            // revert deleted reports, others test might be affected
            foreach (var report in removedReports)
            {
                await AddAsync(report);
            }
        }

        [Fact]
        public async Task GetPendingApprovalCount_WhenUserHasNoReports_ReturnsZero()
        {
            // Arrange
            var user = await AddAsync(new User
            {
                Username = "userwithnoreports",
                Name = "User With No Reports",
                Email = "noreports@example.com",
                PasswordHash = "hash",
                RoleId = 1,
                DepartmentId = 1,
                IsActive = true
            });

            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");

            // Act
            var response = await _client.GetAsync($"/api/reports/pending_approval_count/{user.Id}?plantIds={plant.Id}");

            // Assert
            response.EnsureSuccessStatusCode();
            var count = await response.Content.ReadFromJsonAsync<int>();
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task SubmitReport_AfterChanges_UpdatesReportAndSetsStatusToSubmitted()
        {
            // Arrange
            var (report, initialReportState) = await CreateTestReportWithWorkflow();

            var reportStateToUpdate = await GetFirstAsync<ReportState>(rs => rs.ReportId == report.Id);
            reportStateToUpdate.Status = ReportStatus.RequestChanges;
            await UpdateAsync(reportStateToUpdate);

            var usoCfdi = await GetFirstAsync<UsoCFDI>(u => u.Clave == "G01");
            var incoterm = await GetFirstAsync<Incoterm>(i => i.Clave == "CIF");

            var updateDto = new UpdateReportDto
            {
                Name = "Updated Report After Changes Requested",
                Amount = 450.00m,
                Currency = "EUR",
                UserId = report.UserId,
                ReportTypeId = report.ReportTypeId,
                PlantId = report.PlantId,
                CategoryId = report.CategoryId,
                CostCenterId = report.CostCenterId,
                BankName = "Updated Bank Name",
                AccountNumber = "9876543210",
                Clabe = "987654321098765432",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow,
                PurchaseOrderDetail = new UpdatePurchaseOrderDetailDto
                {
                    UsoCfdiId = usoCfdi.Id,
                    IncotermId = incoterm.Id
                }
            };

            // Act
            var response = await _client.PutAsJsonAsync($"/api/reports/{report.Id}/submit", updateDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

            var getReportResponse = await _client.GetAsync($"/api/reports/{report.Id}");
            getReportResponse.EnsureSuccessStatusCode();
            var updatedReport = await getReportResponse.Content.ReadFromJsonAsync<ReportDto>();

            Assert.NotNull(updatedReport);
            Assert.Equal(updateDto.Name, updatedReport.Name);
            Assert.Equal(updateDto.Amount, updatedReport.Amount);
            Assert.Equal(updateDto.PlantId, updatedReport.PlantId);
            Assert.NotNull(updatedReport.PurchaseOrderDetail);
            Assert.Equal(updateDto.PurchaseOrderDetail.IncotermId, updatedReport.PurchaseOrderDetail.IncotermId);

            var getStateResponse = await _client.GetAsync($"/api/reports/{report.Id}/state");
            getStateResponse.EnsureSuccessStatusCode();
            var updatedReportState = await getStateResponse.Content.ReadFromJsonAsync<ReportStateDto>();

            Assert.NotNull(updatedReportState);
            Assert.Equal(ReportStatus.Submitted, updatedReportState.Status);
        }

        [Fact]
        public async Task CreateReport_ShouldCreateApprovalLog_WithSubmittedAction()
        {
            // Arrange
            var user = await GetFirstAsync<User>(u => u.Username == "padmin");
            var plant = await GetFirstAsync<Plant>(p => p.Name == "Plant 1");
            var category = await GetFirstAsync<Category>(c => c.Name == "Travel");
            var costCenter = await GetFirstAsync<CostCenter>(cc => cc.Code == "CC1");

            var workflow = await AddAsync(new Workflow { Name = "Approval Log Test Workflow", Description = "Workflow for testing approval log creation" });
            await AddAsync(new WorkflowStep { WorkflowId = workflow.Id, Name = "Approval Log Test Step", StepNumber = 1, ApproverRoleId = 1 });

            var reportType = await AddAsync(new ReportType { Name = "Approval Log Test Report Type", DefaultWorkflowId = workflow.Id });

            var createReportDto = new CreateReportDto
            {
                Name = "Test Report for Approval Log",
                Amount = 250.75m,
                Currency = "USD",
                UserId = user.Id,
                ReportTypeId = reportType.Id,
                PlantId = plant.Id,
                CategoryId = category.Id,
                CostCenterId = costCenter.Id,
                BankName = "Test Bank",
                AccountNumber = "1234567890",
                Clabe = "123456789012345678",
                ReportDescription = "Test Description",
                ReportDate = DateTime.UtcNow
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/reports", createReportDto);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var reportDto = await response.Content.ReadFromJsonAsync<ReportDto>();
            Assert.NotNull(reportDto);

            var approvalLog = await GetFirstAsync<ApprovalLog>(al => al.ReportId == reportDto.Id);
            Assert.NotNull(approvalLog);
            Assert.Equal(user.Id, approvalLog.UserId);
            Assert.Equal(ApprovalAction.Submitted, approvalLog.Action);
            Assert.Equal("Enviado para aprovación", approvalLog.Comment);
        }
    }
}
