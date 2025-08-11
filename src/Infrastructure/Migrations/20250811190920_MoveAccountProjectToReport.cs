using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MoveAccountProjectToReport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_AccountProjects_AccountProjectId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseOrderDetails_AccountProjectId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropColumn(
                name: "AccountProjectId",
                table: "PurchaseOrderDetails");

            migrationBuilder.AddColumn<int>(
                name: "AccountProjectId",
                table: "Reports",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reports_AccountProjectId",
                table: "Reports",
                column: "AccountProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reports_AccountProjects_AccountProjectId",
                table: "Reports",
                column: "AccountProjectId",
                principalTable: "AccountProjects",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reports_AccountProjects_AccountProjectId",
                table: "Reports");

            migrationBuilder.DropIndex(
                name: "IX_Reports_AccountProjectId",
                table: "Reports");

            migrationBuilder.DropColumn(
                name: "AccountProjectId",
                table: "Reports");

            migrationBuilder.AddColumn<int>(
                name: "AccountProjectId",
                table: "PurchaseOrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderDetails_AccountProjectId",
                table: "PurchaseOrderDetails",
                column: "AccountProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_AccountProjects_AccountProjectId",
                table: "PurchaseOrderDetails",
                column: "AccountProjectId",
                principalTable: "AccountProjects",
                principalColumn: "Id");
        }
    }
}
