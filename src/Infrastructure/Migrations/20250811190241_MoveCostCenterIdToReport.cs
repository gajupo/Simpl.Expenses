using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveCostCenterIdToReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_CostCenters_CostCenterId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDetails_CostCenterId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                table: "PurchaseOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                table: "Reports",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CostCenterId",
                table: "Reports",
                column: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_CostCenters_CostCenterId",
                table: "Reports",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_CostCenters_CostCenterId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_CostCenterId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "CostCenterId",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "CostCenterId",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_CostCenterId",
                table: "PurchaseOrderDetails",
                column: "CostCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_CostCenters_CostCenterId",
                table: "PurchaseOrderDetails",
                column: "CostCenterId",
                principalTable: "CostCenters",
                principalColumn: "Id");
        }
    }
}
