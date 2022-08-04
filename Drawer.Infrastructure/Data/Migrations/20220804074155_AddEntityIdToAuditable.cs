using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class AddEntityIdToAuditable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Zones",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "WorkPlaces",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Spots",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Locations",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Items",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "InventoryDetails",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "CompanyMembers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "Companies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "WorkPlaces");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Spots");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Items");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "InventoryDetails");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "CompanyMembers");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "Companies");
        }
    }
}
