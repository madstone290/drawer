using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class RemoveSoftdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name_CompanyId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Items_Name_CompanyId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "deleted_at",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "DeletedBy",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LastModifiedAt",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "Companies");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name_CompanyId",
                table: "Locations",
                columns: new[] { "Name", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name_CompanyId",
                table: "Items",
                columns: new[] { "Name", "CompanyId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Locations_Name_CompanyId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Items_Name_CompanyId",
                table: "Items");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Zones",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Zones",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Zones",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Zones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Zones",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Zones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "WorkPlaces",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkPlaces",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "WorkPlaces",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "WorkPlaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "WorkPlaces",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "WorkPlaces",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Spots",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Spots",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Spots",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Spots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Spots",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Spots",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Locations",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Locations",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Locations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Locations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "Locations",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Items",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Items",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Items",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "deleted_at",
                table: "Items",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "InventoryDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "InventoryDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "InventoryDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "InventoryDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "InventoryDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "InventoryDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "CompanyMembers",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "CompanyMembers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "CompanyMembers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "CompanyMembers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "CompanyMembers",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "CompanyMembers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DeletedBy",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModifiedAt",
                table: "Companies",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "Companies",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Name_CompanyId",
                table: "Locations",
                columns: new[] { "Name", "CompanyId" },
                unique: true,
                filter: "deleted_at IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Items_Name_CompanyId",
                table: "Items",
                columns: new[] { "Name", "CompanyId" },
                unique: true,
                filter: "deleted_at IS NULL");
        }
    }
}
