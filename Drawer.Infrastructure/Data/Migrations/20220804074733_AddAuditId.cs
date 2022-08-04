using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class AddAuditId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Zones",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "WorkPlaces",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Spots",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Locations",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Items",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "InventoryDetails",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "CompanyMembers",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "Companies",
                newName: "AuditId");

            migrationBuilder.RenameColumn(
                name: "EntityId",
                table: "__AuditEvents",
                newName: "EntityAuditId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Zones",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "WorkPlaces",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Spots",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Locations",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Items",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "InventoryDetails",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "CompanyMembers",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "AuditId",
                table: "Companies",
                newName: "EntityId");

            migrationBuilder.RenameColumn(
                name: "EntityAuditId",
                table: "__AuditEvents",
                newName: "EntityId");
        }
    }
}
