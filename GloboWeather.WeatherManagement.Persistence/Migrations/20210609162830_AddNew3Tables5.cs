using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class AddNew3Tables5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "WeatherInfomations");

            migrationBuilder.AddColumn<string>(
                name: "StationId",
                table: "WeatherInfomations",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationId",
                table: "WeatherInfomations");

            migrationBuilder.AddColumn<string>(
                name: "PositionId",
                table: "WeatherInfomations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
