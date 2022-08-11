using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations
{
    public partial class AddNoteProp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spots_Zones_ZoneId",
                table: "Spots");

            migrationBuilder.DropForeignKey(
                name: "FK_Zones_WorkPlaces_WorkPlaceId",
                table: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zones",
                table: "Zones");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkPlaces",
                table: "WorkPlaces");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spots",
                table: "Spots");

            migrationBuilder.RenameTable(
                name: "Zones",
                newName: "Zone");

            migrationBuilder.RenameTable(
                name: "WorkPlaces",
                newName: "Workplace");

            migrationBuilder.RenameTable(
                name: "Spots",
                newName: "Spot");

            migrationBuilder.RenameIndex(
                name: "IX_Zones_WorkPlaceId",
                table: "Zone",
                newName: "IX_Zone_WorkPlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Spots_ZoneId",
                table: "Spot",
                newName: "IX_Spot_ZoneId");

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Receipts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Note",
                table: "Issues",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zone",
                table: "Zone",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Workplace",
                table: "Workplace",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spot",
                table: "Spot",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Spot_Zone_ZoneId",
                table: "Spot",
                column: "ZoneId",
                principalTable: "Zone",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Zone_Workplace_WorkPlaceId",
                table: "Zone",
                column: "WorkPlaceId",
                principalTable: "Workplace",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Spot_Zone_ZoneId",
                table: "Spot");

            migrationBuilder.DropForeignKey(
                name: "FK_Zone_Workplace_WorkPlaceId",
                table: "Zone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Zone",
                table: "Zone");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Workplace",
                table: "Workplace");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Spot",
                table: "Spot");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "Note",
                table: "Issues");

            migrationBuilder.RenameTable(
                name: "Zone",
                newName: "Zones");

            migrationBuilder.RenameTable(
                name: "Workplace",
                newName: "WorkPlaces");

            migrationBuilder.RenameTable(
                name: "Spot",
                newName: "Spots");

            migrationBuilder.RenameIndex(
                name: "IX_Zone_WorkPlaceId",
                table: "Zones",
                newName: "IX_Zones_WorkPlaceId");

            migrationBuilder.RenameIndex(
                name: "IX_Spot_ZoneId",
                table: "Spots",
                newName: "IX_Spots_ZoneId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Zones",
                table: "Zones",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkPlaces",
                table: "WorkPlaces",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Spots",
                table: "Spots",
                column: "Id");

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
    }
}
