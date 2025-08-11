using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ImproveBudgetConsumptionModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetConsumptions_AccountProjects_AccountProjectId",
                table: "BudgetConsumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetConsumptions_CostCenters_CostCenterId",
                table: "BudgetConsumptions");

            migrationBuilder.DropIndex(
                name: "IX_BudgetConsumptions_AccountProjectId",
                table: "BudgetConsumptions");

            migrationBuilder.DropIndex(
                name: "IX_BudgetConsumptions_CostCenterId_AccountProjectId_ReportId",
                table: "BudgetConsumptions");

            migrationBuilder.DropColumn(
                name: "AccountProjectId",
                table: "BudgetConsumptions");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                table: "BudgetConsumptions");

            migrationBuilder.AlterColumn<int>(
                name: "AccountProjectId",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "BudgetConsumptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_BudgetConsumptions_BudgetId_ReportId",
                table: "BudgetConsumptions",
                columns: new[] { "BudgetId", "ReportId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetConsumptions_Budgets_BudgetId",
                table: "BudgetConsumptions",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BudgetConsumptions_Budgets_BudgetId",
                table: "BudgetConsumptions");

            migrationBuilder.DropIndex(
                name: "IX_BudgetConsumptions_BudgetId_ReportId",
                table: "BudgetConsumptions");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "BudgetConsumptions");

            migrationBuilder.AlterColumn<int>(
                name: "AccountProjectId",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountProjectId",
                table: "BudgetConsumptions",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                table: "BudgetConsumptions",
                type: "int",
                nullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetConsumptions_AccountProjects_AccountProjectId",
                table: "BudgetConsumptions",
                column: "AccountProjectId",
                principalTable: "AccountProjects",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetConsumptions_CostCenters_CostCenterId",
                table: "BudgetConsumptions",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }
    }
}
