using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountProjects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountProjects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Icon = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CostCenters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CostCenters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Incoterms",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incoterms", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Plants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ReportTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Suppliers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Rfc = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Suppliers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UsoCFDIs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Clave = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsoCFDIs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Workflows",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workflows", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Budgets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    AccountProjectId = table.Column<int>(type: "int", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Budgets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Budgets_AccountProjects_AccountProjectId",
                        column: x => x.AccountProjectId,
                        principalTable: "AccountProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Budgets_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Departments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Departments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Departments_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowProjects",
                columns: table => new
                {
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowProjects", x => new { x.WorkflowId, x.ProjectId });
                    table.ForeignKey(
                        name: "FK_WorkflowProjects_AccountProjects_ProjectId",
                        column: x => x.ProjectId,
                        principalTable: "AccountProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowProjects_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkflowSteps",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    StepNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ApproverRoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkflowSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_Roles_ApproverRoleId",
                        column: x => x.ApproverRoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkflowSteps_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<int>(type: "int", nullable: false),
                    ReportsToId = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Users_Users_ReportsToId",
                        column: x => x.ReportsToId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false, defaultValue: "MXN"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ReportTypeId = table.Column<int>(type: "int", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SupplierId = table.Column<int>(type: "int", nullable: true),
                    BankName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Clabe = table.Column<string>(type: "nvarchar(18)", maxLength: 18, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_ReportTypes_ReportTypeId",
                        column: x => x.ReportTypeId,
                        principalTable: "ReportTypes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Suppliers_SupplierId",
                        column: x => x.SupplierId,
                        principalTable: "Suppliers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Reports_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserPlants",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlantId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlants", x => new { x.UserId, x.PlantId });
                    table.ForeignKey(
                        name: "FK_UserPlants_Plants_PlantId",
                        column: x => x.PlantId,
                        principalTable: "Plants",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserPlants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "AdvancePaymentDetails",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdvancePaymentDetails", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_AdvancePaymentDetails_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ApprovalLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Action = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LogDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApprovalLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApprovalLogs_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ApprovalLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "BudgetConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CostCenterId = table.Column<int>(type: "int", nullable: true),
                    AccountProjectId = table.Column<int>(type: "int", nullable: true),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumptionDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BudgetConsumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BudgetConsumptions_AccountProjects_AccountProjectId",
                        column: x => x.AccountProjectId,
                        principalTable: "AccountProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BudgetConsumptions_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_BudgetConsumptions_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PurchaseOrderDetails",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    CostCenterId = table.Column<int>(type: "int", nullable: false),
                    AccountProjectId = table.Column<int>(type: "int", nullable: false),
                    UsoCfdiId = table.Column<int>(type: "int", nullable: false),
                    IncotermId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderDetails", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_AccountProjects_AccountProjectId",
                        column: x => x.AccountProjectId,
                        principalTable: "AccountProjects",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_CostCenters_CostCenterId",
                        column: x => x.CostCenterId,
                        principalTable: "CostCenters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Incoterms_IncotermId",
                        column: x => x.IncotermId,
                        principalTable: "Incoterms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PurchaseOrderDetails_UsoCFDIs_UsoCfdiId",
                        column: x => x.UsoCfdiId,
                        principalTable: "UsoCFDIs",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReimbursementDetails",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    EmployeeNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReimbursementDetails", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReimbursementDetails_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    UploadedByUserId = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FileSizeKb = table.Column<int>(type: "int", nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UploadedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportAttachments_Users_UploadedByUserId",
                        column: x => x.UploadedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ReportStates",
                columns: table => new
                {
                    ReportId = table.Column<int>(type: "int", nullable: false),
                    WorkflowId = table.Column<int>(type: "int", nullable: false),
                    CurrentStepId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportStates", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_ReportStates_Reports_ReportId",
                        column: x => x.ReportId,
                        principalTable: "Reports",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportStates_WorkflowSteps_CurrentStepId",
                        column: x => x.CurrentStepId,
                        principalTable: "WorkflowSteps",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReportStates_Workflows_WorkflowId",
                        column: x => x.WorkflowId,
                        principalTable: "Workflows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLogs_ReportId",
                table: "ApprovalLogs",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ApprovalLogs_UserId",
                table: "ApprovalLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetConsumptions_AccountProjectId",
                table: "BudgetConsumptions",
                column: "AccountProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetConsumptions_CostCenterId_AccountProjectId_ReportId",
                table: "BudgetConsumptions",
                columns: new[] { "CostCenterId", "AccountProjectId", "ReportId" },
                unique: true,
                filter: "[CostCenterId] IS NOT NULL AND [AccountProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_BudgetConsumptions_ReportId",
                table: "BudgetConsumptions",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_AccountProjectId",
                table: "Budgets",
                column: "AccountProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_CostCenterId_AccountProjectId_StartDate_EndDate",
                table: "Budgets",
                columns: new[] { "CostCenterId", "AccountProjectId", "StartDate", "EndDate" },
                unique: true,
                filter: "[CostCenterId] IS NOT NULL AND [AccountProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Departments_CostCenterId",
                table: "Departments",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_AccountProjectId",
                table: "PurchaseOrderDetails",
                column: "AccountProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_CostCenterId",
                table: "PurchaseOrderDetails",
                column: "CostCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_IncotermId",
                table: "PurchaseOrderDetails",
                column: "IncotermId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_UsoCfdiId",
                table: "PurchaseOrderDetails",
                column: "UsoCfdiId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_ReportId",
                table: "ReportAttachments",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportAttachments_UploadedByUserId",
                table: "ReportAttachments",
                column: "UploadedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CategoryId",
                table: "Reports",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PlantId",
                table: "Reports",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportNumber",
                table: "Reports",
                column: "ReportNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ReportTypeId",
                table: "Reports",
                column: "ReportTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_SupplierId",
                table: "Reports",
                column: "SupplierId");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_UserId",
                table: "Reports",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportStates_CurrentStepId",
                table: "ReportStates",
                column: "CurrentStepId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportStates_WorkflowId",
                table: "ReportStates",
                column: "WorkflowId");

            migrationBuilder.CreateIndex(
                name: "IX_ReportTypes_Name",
                table: "ReportTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_Name",
                table: "Roles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Suppliers_Rfc",
                table: "Suppliers",
                column: "Rfc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserPlants_PlantId",
                table: "UserPlants",
                column: "PlantId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DepartmentId",
                table: "Users",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_ReportsToId",
                table: "Users",
                column: "ReportsToId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowProjects_ProjectId",
                table: "WorkflowProjects",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Workflows_Name",
                table: "Workflows",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_ApproverRoleId",
                table: "WorkflowSteps",
                column: "ApproverRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkflowSteps_WorkflowId",
                table: "WorkflowSteps",
                column: "WorkflowId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdvancePaymentDetails");

            migrationBuilder.DropTable(
                name: "ApprovalLogs");

            migrationBuilder.DropTable(
                name: "BudgetConsumptions");

            migrationBuilder.DropTable(
                name: "Budgets");

            migrationBuilder.DropTable(
                name: "PurchaseOrderDetails");

            migrationBuilder.DropTable(
                name: "ReimbursementDetails");

            migrationBuilder.DropTable(
                name: "ReportAttachments");

            migrationBuilder.DropTable(
                name: "ReportStates");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "UserPlants");

            migrationBuilder.DropTable(
                name: "WorkflowProjects");

            migrationBuilder.DropTable(
                name: "Incoterms");

            migrationBuilder.DropTable(
                name: "UsoCFDIs");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "WorkflowSteps");

            migrationBuilder.DropTable(
                name: "AccountProjects");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Plants");

            migrationBuilder.DropTable(
                name: "ReportTypes");

            migrationBuilder.DropTable(
                name: "Suppliers");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Workflows");

            migrationBuilder.DropTable(
                name: "Departments");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "CostCenters");
        }
    }
}
