using Microsoft.EntityFrameworkCore.Migrations;

namespace KibiLights.Migrations
{
    public partial class AddFacilityUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Facilities",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Facilities_UserId",
                table: "Facilities",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Facilities_Users_UserId",
                table: "Facilities",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Facilities_Users_UserId",
                table: "Facilities");

            migrationBuilder.DropIndex(
                name: "IX_Facilities_UserId",
                table: "Facilities");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Facilities");
        }
    }
}
