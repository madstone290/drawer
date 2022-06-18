using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations.DrawerIdentity
{
    public partial class SeperateRefreshToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK___Identity__RefreshToken___Identity__AspNetUsers_UserId",
                table: "__Identity__RefreshToken");

            migrationBuilder.DropPrimaryKey(
                name: "PK___Identity__RefreshToken",
                table: "__Identity__RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX___Identity__RefreshToken_UserId",
                table: "__Identity__RefreshToken");

            migrationBuilder.RenameTable(
                name: "__Identity__RefreshToken",
                newName: "__Identity__RefreshTokens");

            migrationBuilder.AddPrimaryKey(
                name: "PK___Identity__RefreshTokens",
                table: "__Identity__RefreshTokens",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK___Identity__RefreshTokens",
                table: "__Identity__RefreshTokens");

            migrationBuilder.RenameTable(
                name: "__Identity__RefreshTokens",
                newName: "__Identity__RefreshToken");

            migrationBuilder.AddPrimaryKey(
                name: "PK___Identity__RefreshToken",
                table: "__Identity__RefreshToken",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX___Identity__RefreshToken_UserId",
                table: "__Identity__RefreshToken",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK___Identity__RefreshToken___Identity__AspNetUsers_UserId",
                table: "__Identity__RefreshToken",
                column: "UserId",
                principalTable: "__Identity__AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
