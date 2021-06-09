using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class AddNew3Tables4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BackgroundServiceTrackings",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    LastDownload = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BackgroundServiceTrackings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    GoogleX = table.Column<float>(nullable: false),
                    GoogleY = table.Column<float>(nullable: false),
                    IsSystem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "WeatherInformations",
                columns: table => new
                {
                    ID = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    PositionId = table.Column<string>(nullable: false),
                    RefDate = table.Column<DateTime>(nullable: false),
                    Humidity = table.Column<string>(nullable: true),
                    WindLevel = table.Column<string>(nullable: true),
                    WindDirection = table.Column<string>(nullable: true),
                    WindSpeed = table.Column<string>(nullable: true),
                    RainAmount = table.Column<string>(nullable: true),
                    Temperature = table.Column<string>(nullable: true),
                    Weather = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherInformations", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BackgroundServiceTrackings");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "WeatherInformations");
        }
    }
}
