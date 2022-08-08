using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class UpdateLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_UpperLocationId",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "UpperLocationId",
                table: "Locations",
                newName: "ParentGroupId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_UpperLocationId",
                table: "Locations",
                newName: "IX_Locations_ParentGroupId");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "Locations",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_ParentGroupId",
                table: "Locations",
                column: "ParentGroupId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Locations_ParentGroupId",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "Locations");

            migrationBuilder.RenameColumn(
                name: "ParentGroupId",
                table: "Locations",
                newName: "UpperLocationId");

            migrationBuilder.RenameIndex(
                name: "IX_Locations_ParentGroupId",
                table: "Locations",
                newName: "IX_Locations_UpperLocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Locations_UpperLocationId",
                table: "Locations",
                column: "UpperLocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
