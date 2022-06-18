using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Drawer.Infrastructure.Data.Migrations.DrawerIdentity
{
    public partial class RenameUtcExpires : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Expires",
                table: "__Identity__RefreshToken",
                newName: "UtcExpires");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UtcExpires",
                table: "__Identity__RefreshToken",
                newName: "Expires");
        }
    }
}
