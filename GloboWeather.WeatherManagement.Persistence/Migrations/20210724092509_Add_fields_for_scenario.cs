using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class Add_fields_for_scenario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Bottom",
                table: "ScenarioActions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnableIcon",
                table: "ScenarioActions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnableLayer",
                table: "ScenarioActions",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Left",
                table: "ScenarioActions",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Right",
                table: "ScenarioActions",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Top",
                table: "ScenarioActions",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnableIcon",
                table: "ScenarioActionDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bottom",
                table: "ScenarioActions");

            migrationBuilder.DropColumn(
                name: "IsEnableIcon",
                table: "ScenarioActions");

            migrationBuilder.DropColumn(
                name: "IsEnableLayer",
                table: "ScenarioActions");

            migrationBuilder.DropColumn(
                name: "Left",
                table: "ScenarioActions");

            migrationBuilder.DropColumn(
                name: "Right",
                table: "ScenarioActions");

            migrationBuilder.DropColumn(
                name: "Top",
                table: "ScenarioActions");

            migrationBuilder.DropColumn(
                name: "IsEnableIcon",
                table: "ScenarioActionDetails");
        }
    }
}
