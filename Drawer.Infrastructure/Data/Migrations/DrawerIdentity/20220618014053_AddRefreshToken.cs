using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations.DrawerIdentity
{
    public partial class AddRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefreshToken",
                table: "__Identity__AspNetUsers");

            migrationBuilder.CreateTable(
                name: "__Identity__RefreshToken",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: false),
                    Token = table.Column<string>(type: "text", nullable: false),
                    Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Revoked = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___Identity__RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK___Identity__RefreshToken___Identity__AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "__Identity__AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX___Identity__RefreshToken_UserId",
                table: "__Identity__RefreshToken",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__Identity__RefreshToken");

            migrationBuilder.AddColumn<string>(
                name: "RefreshToken",
                table: "__Identity__AspNetUsers",
                type: "text",
                nullable: true);
        }
    }
}
