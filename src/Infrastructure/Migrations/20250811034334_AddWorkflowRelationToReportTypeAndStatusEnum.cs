using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkflowRelationToReportTypeAndStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DefaultWorkflowId",
                table: "ReportTypes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "ReportStates",
                type: "int",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.CreateIndex(
                name: "IX_ReportTypes_DefaultWorkflowId",
                table: "ReportTypes",
                column: "DefaultWorkflowId");

            migrationBuilder.AddForeignKey(
                name: "FK_ReportTypes_Workflows_DefaultWorkflowId",
                table: "ReportTypes",
                column: "DefaultWorkflowId",
                principalTable: "Workflows",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReportTypes_Workflows_DefaultWorkflowId",
                table: "ReportTypes");

            migrationBuilder.DropIndex(
                name: "IX_ReportTypes_DefaultWorkflowId",
                table: "ReportTypes");

            migrationBuilder.DropColumn(
                name: "DefaultWorkflowId",
                table: "ReportTypes");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "ReportStates",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);
        }
    }
}
