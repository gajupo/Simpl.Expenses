using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Simpl.Expenses.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderNumberToReimbursementDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrderNumber",
                table: "ReimbursementDetails",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderNumber",
                table: "ReimbursementDetails");
        }
    }
}
