using Microsoft.EntityFrameworkCore.Migrations;

namespace KibiLights.Migrations
{
    public partial class AddRouteStepsFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_RouteSteps_RouteId",
                table: "RouteSteps",
                column: "RouteId");

            migrationBuilder.AddForeignKey(
                name: "FK_RouteSteps_Routes_RouteId",
                table: "RouteSteps",
                column: "RouteId",
                principalTable: "Routes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RouteSteps_Routes_RouteId",
                table: "RouteSteps");

            migrationBuilder.DropIndex(
                name: "IX_RouteSteps_RouteId",
                table: "RouteSteps");
        }
    }
}
