using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class AddTransactionNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiptTime",
                table: "Receipts",
                newName: "ReceiptDateTime");

            migrationBuilder.RenameColumn(
                name: "IssueTime",
                table: "Issues",
                newName: "IssueDateTime");

            migrationBuilder.AddColumn<string>(
                name: "TransactionNumber",
                table: "Receipts",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TransactionNumber",
                table: "Issues",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionNumber",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "TransactionNumber",
                table: "Issues");

            migrationBuilder.RenameColumn(
                name: "ReceiptDateTime",
                table: "Receipts",
                newName: "ReceiptTime");

            migrationBuilder.RenameColumn(
                name: "IssueDateTime",
                table: "Issues",
                newName: "IssueTime");
        }
    }
}
