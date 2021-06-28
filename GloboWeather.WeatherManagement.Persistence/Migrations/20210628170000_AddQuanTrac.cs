using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class AddQuanTrac : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Districts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Code = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Districts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HydrologicalForeCasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StationId = table.Column<string>(nullable: false),
                    RefDate = table.Column<DateTime>(nullable: false),
                    MinValue = table.Column<float>(nullable: true),
                    MaxValue = table.Column<float>(nullable: true),
                    Type = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HydrologicalForeCasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hydrologicals",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StationId = table.Column<string>(nullable: false),
                    RefDate = table.Column<DateTime>(nullable: false),
                    Rain = table.Column<float>(nullable: true),
                    WaterLevel = table.Column<float>(nullable: true),
                    Accumulated = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hydrologicals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MeteorologicalStations",
                columns: table => new
                {
                    StationId = table.Column<string>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    GoogleX = table.Column<float>(nullable: true),
                    GoogleY = table.Column<float>(nullable: true),
                    MeteorologicalStationTypeId = table.Column<int>(nullable: false),
                    GoverningBody = table.Column<string>(nullable: true),
                    ProvinceId = table.Column<int>(nullable: true),
                    Address = table.Column<string>(nullable: true),
                    Hong = table.Column<int>(nullable: true),
                    Regime = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteorologicalStations", x => x.StationId);
                });

            migrationBuilder.CreateTable(
                name: "MeteorologicalStationTypes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeteorologicalStationTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RainLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    StationId = table.Column<string>(nullable: false),
                    RefDate = table.Column<DateTime>(nullable: false),
                    Value = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RainLevels", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Districts");

            migrationBuilder.DropTable(
                name: "HydrologicalForeCasts");

            migrationBuilder.DropTable(
                name: "Hydrologicals");

            migrationBuilder.DropTable(
                name: "MeteorologicalStations");

            migrationBuilder.DropTable(
                name: "MeteorologicalStationTypes");

            migrationBuilder.DropTable(
                name: "RainLevels");
        }
    }
}
