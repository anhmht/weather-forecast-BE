using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class Add_2_column_new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsProvince",
                table: "ScenarioActionDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PlaceId",
                table: "ScenarioActionDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsProvince",
                table: "ScenarioActionDetails");

            migrationBuilder.DropColumn(
                name: "PlaceId",
                table: "ScenarioActionDetails");
        }
    }
}
