using Microsoft.EntityFrameworkCore.Migrations;

namespace KibiLights.Migrations
{
    public partial class AddBeaconDescription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Beacons",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Beacons");
        }
    }
}
