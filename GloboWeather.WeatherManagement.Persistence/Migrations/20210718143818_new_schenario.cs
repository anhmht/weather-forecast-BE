using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class new_schenario : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CommonLookups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    NameSpace = table.Column<string>(nullable: false),
                    ValueId = table.Column<int>(nullable: false),
                    ValueText = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommonLookups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioActionDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    ActionId = table.Column<Guid>(nullable: false),
                    ScenarioActionTypeId = table.Column<int>(nullable: false),
                    ActionTypeId = table.Column<int>(nullable: true),
                    MethodId = table.Column<int>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: true),
                    Time = table.Column<int>(nullable: true),
                    PositionId = table.Column<int>(nullable: true),
                    CustomPosition = table.Column<bool>(nullable: true),
                    Left = table.Column<int>(nullable: true),
                    Top = table.Column<int>(nullable: true),
                    IsDisplay = table.Column<bool>(nullable: true),
                    StartTime = table.Column<int>(nullable: true),
                    Width = table.Column<int>(nullable: true),
                    IconUrls = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioActionDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ScenarioActions",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateBy = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    LastModifiedBy = table.Column<string>(nullable: true),
                    LastModifiedDate = table.Column<DateTime>(nullable: true),
                    ScenarioId = table.Column<Guid>(nullable: false),
                    ActionTypeId = table.Column<int>(nullable: true),
                    MethodId = table.Column<int>(nullable: true),
                    AreaTypeId = table.Column<int>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    Duration = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScenarioActions", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommonLookups");

            migrationBuilder.DropTable(
                name: "ScenarioActionDetails");

            migrationBuilder.DropTable(
                name: "ScenarioActions");
        }
    }
}
