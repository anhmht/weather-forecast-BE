using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class add_deletefile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DeleteFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TableName = table.Column<string>(nullable: true),
                    DeleteId = table.Column<Guid>(nullable: false),
                    ContainerName = table.Column<string>(nullable: true),
                    FileUrl = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeleteFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventViewCounts",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    ViewCount = table.Column<int>(nullable: false),
                    LastTimeView = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventViewCounts", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeleteFiles");

            migrationBuilder.DropTable(
                name: "EventViewCounts");
        }
    }
}
