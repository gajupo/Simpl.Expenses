using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CascadeDeleteForReportRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancePaymentDetails_Reports_ReportId",
                table: "AdvancePaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLogs_Reports_ReportId",
                table: "ApprovalLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetConsumptions_Reports_ReportId",
                table: "BudgetConsumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_Reports_ReportId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReimbursementDetails_Reports_ReportId",
                table: "ReimbursementDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttachments_Reports_ReportId",
                table: "ReportAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportStates_Reports_ReportId",
                table: "ReportStates");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancePaymentDetails_Reports_ReportId",
                table: "AdvancePaymentDetails",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLogs_Reports_ReportId",
                table: "ApprovalLogs",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetConsumptions_Reports_ReportId",
                table: "BudgetConsumptions",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_Reports_ReportId",
                table: "PurchaseOrderDetails",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReimbursementDetails_Reports_ReportId",
                table: "ReimbursementDetails",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttachments_Reports_ReportId",
                table: "ReportAttachments",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReportStates_Reports_ReportId",
                table: "ReportStates",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AdvancePaymentDetails_Reports_ReportId",
                table: "AdvancePaymentDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ApprovalLogs_Reports_ReportId",
                table: "ApprovalLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_BudgetConsumptions_Reports_ReportId",
                table: "BudgetConsumptions");

            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseOrderDetails_Reports_ReportId",
                table: "PurchaseOrderDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReimbursementDetails_Reports_ReportId",
                table: "ReimbursementDetails");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportAttachments_Reports_ReportId",
                table: "ReportAttachments");

            migrationBuilder.DropForeignKey(
                name: "FK_ReportStates_Reports_ReportId",
                table: "ReportStates");

            migrationBuilder.AddForeignKey(
                name: "FK_AdvancePaymentDetails_Reports_ReportId",
                table: "AdvancePaymentDetails",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApprovalLogs_Reports_ReportId",
                table: "ApprovalLogs",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BudgetConsumptions_Reports_ReportId",
                table: "BudgetConsumptions",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseOrderDetails_Reports_ReportId",
                table: "PurchaseOrderDetails",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReimbursementDetails_Reports_ReportId",
                table: "ReimbursementDetails",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportAttachments_Reports_ReportId",
                table: "ReportAttachments",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportStates_Reports_ReportId",
                table: "ReportStates",
                column: "ReportId",
                principalTable: "Reports",
                principalColumn: "Id");
        }
    }
}
