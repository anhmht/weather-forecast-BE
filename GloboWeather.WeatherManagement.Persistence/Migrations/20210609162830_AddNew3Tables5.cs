using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class AddNew3Tables5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "WeatherInformations");

            migrationBuilder.AddColumn<string>(
                name: "StationId",
                table: "WeatherInformations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationId",
                table: "WeatherInformations");

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "WeatherInformations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
