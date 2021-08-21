using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class Add_Comment_ParentField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ParentCommentId",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParentCommentId",
                table: "Comments");
        }
    }
}
