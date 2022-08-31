using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class AddRootId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RootGroupId",
                table: "Locations",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_RootGroupId",
                table: "Locations",
                column: "RootGroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_RootGroupId",
                table: "Locations",
                column: "RootGroupId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_RootGroupId",
                table: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_Locations_RootGroupId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "RootGroupId",
                table: "Locations");
        }
    }
}
