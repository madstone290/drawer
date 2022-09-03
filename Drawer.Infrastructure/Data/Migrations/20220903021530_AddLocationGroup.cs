using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class AddLocationGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationGroups",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Note = table.Column<string>(type: "text", nullable: true),
                    ParentGroupId = table.Column<long>(type: "bigint", nullable: true),
                    Depth = table.Column<int>(type: "integer", nullable: false),
                    RootGroupIdDBValue = table.Column<long>(type: "bigint", nullable: true),
                    AuditId = table.Column<Guid>(type: "uuid", nullable: false),
                    CompanyId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationGroups_LocationGroups_ParentGroupId",
                        column: x => x.ParentGroupId,
                        principalTable: "LocationGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LocationGroups_LocationGroups_RootGroupIdDBValue",
                        column: x => x.RootGroupIdDBValue,
                        principalTable: "LocationGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationGroups_Name_CompanyId",
                table: "LocationGroups",
                columns: new[] { "Name", "CompanyId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationGroups_ParentGroupId",
                table: "LocationGroups",
                column: "ParentGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationGroups_RootGroupIdDBValue",
                table: "LocationGroups",
                column: "RootGroupIdDBValue");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationGroups");
        }
    }
}
