using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class UpdateCompanyResource : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Companies");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "WorkPlaceZoneTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "WorkPlaceZoneTypes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkPlaceZoneTypes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "WorkPlaceZoneTypes",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "WorkPlaceZoneTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "WorkPlaceZones",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "WorkPlaceZones",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CreatedBy",
                table: "WorkPlaceZones",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "WorkPlaceZones",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "WorkPlaceZones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "WorkPlaces",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
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
                name: "LastModified",
                table: "WorkPlaces",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastModifiedBy",
                table: "WorkPlaces",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WorkPlaceZoneTypes");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "WorkPlaceZoneTypes");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkPlaceZoneTypes");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "WorkPlaceZoneTypes");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "WorkPlaceZoneTypes");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WorkPlaceZones");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "WorkPlaceZones");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkPlaceZones");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "WorkPlaceZones");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "WorkPlaceZones");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "LastModifiedBy",
                table: "WorkPlaces");

            migrationBuilder.AddColumn<string>(
                name: "CompanyId",
                table: "Companies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
