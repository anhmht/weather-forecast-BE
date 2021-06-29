using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class ChangeRainLevel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RainLevels",
                table: "RainLevels");

            migrationBuilder.RenameTable(
                name: "RainLevels",
                newName: "RainQuantities");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RainQuantities",
                table: "RainQuantities",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RainQuantities",
                table: "RainQuantities");

            migrationBuilder.RenameTable(
                name: "RainQuantities",
                newName: "RainLevels");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RainLevels",
                table: "RainLevels",
                column: "Id");
        }
    }
}
