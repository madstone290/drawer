using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class ChangeOnDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spots_Zones_ZoneId",
                table: "Spots");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones");

            migrationBuilder.AddForeignKey(
                name: "FK_Spots_Zones_ZoneId",
                table: "Spots",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones",
                column: "WorkPlaceId",
                principalTable: "WorkPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spots_Zones_ZoneId",
                table: "Spots");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones");

            migrationBuilder.AddForeignKey(
                name: "FK_Spots_Zones_ZoneId",
                table: "Spots",
                column: "ZoneId",
                principalTable: "Zones",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones",
                column: "WorkPlaceId",
                principalTable: "WorkPlaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
