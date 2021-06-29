using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class ChangeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RefDate",
                table: "Hydrologicals");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                table: "MeteorologicalStations",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Hydrologicals",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Meteorologicals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StationId = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    Evaporation = table.Column<float>(nullable: true),
                    Radiation = table.Column<float>(nullable: true),
                    Humidity = table.Column<float>(nullable: true),
                    WindDirection = table.Column<float>(nullable: true),
                    Barometric = table.Column<float>(nullable: true),
                    Hga10 = table.Column<float>(nullable: true),
                    Hgm60 = table.Column<float>(nullable: true),
                    Rain = table.Column<float>(nullable: true),
                    Temperature = table.Column<float>(nullable: true),
                    Tdga10 = table.Column<float>(nullable: true),
                    Tdgm60 = table.Column<float>(nullable: true),
                    WindSpeed = table.Column<float>(nullable: true),
                    SunnyTime = table.Column<float>(nullable: true),
                    ZluyKe = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meteorologicals", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Meteorologicals");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Hydrologicals");

            migrationBuilder.AlterColumn<int>(
                name: "ProvinceId",
                table: "MeteorologicalStations",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<DateTime>(
                name: "RefDate",
                table: "Hydrologicals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
