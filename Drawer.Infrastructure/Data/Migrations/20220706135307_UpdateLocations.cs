using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class UpdateLocations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_ZoneTypes_ZoneTypeId",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "Positions");

            migrationBuilder.DropTable(
                name: "ZoneTypes");

            migrationBuilder.DropIndex(
                name: "IX_Zones_ZoneTypeId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "ZoneTypeId",
                table: "Zones");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "WorkPlaces",
                newName: "Note");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Zones",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "WorkPlaceId",
                table: "Zones",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Spots",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    ZoneId = table.Column<long>(type: "bigint", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Spots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Spots_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_WorkPlaceId",
                table: "Zones",
                column: "WorkPlaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Spots_ZoneId",
                table: "Spots",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones",
                column: "WorkPlaceId",
                principalTable: "WorkPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones");

            migrationBuilder.DropTable(
                name: "Spots");

            migrationBuilder.DropIndex(
                name: "IX_Zones_WorkPlaceId",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Zones");

            migrationBuilder.DropColumn(
                name: "WorkPlaceId",
                table: "Zones");

            migrationBuilder.RenameColumn(
                name: "Note",
                table: "WorkPlaces",
                newName: "Description");

            migrationBuilder.AddColumn<long>(
                name: "ZoneTypeId",
                table: "Zones",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Positions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ZoneId = table.Column<long>(type: "bigint", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Positions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Positions_Zones_ZoneId",
                        column: x => x.ZoneId,
                        principalTable: "Zones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZoneTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<string>(type: "text", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    LastModified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "text", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZoneTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Zones_ZoneTypeId",
                table: "Zones",
                column: "ZoneTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Positions_ZoneId",
                table: "Positions",
                column: "ZoneId");

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_ZoneTypes_ZoneTypeId",
                table: "Zones",
                column: "ZoneTypeId",
                principalTable: "ZoneTypes",
                principalColumn: "Id");
        }
    }
}
