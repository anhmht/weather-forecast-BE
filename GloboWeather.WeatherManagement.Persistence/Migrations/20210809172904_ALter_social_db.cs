using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class ALter_social_db : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SharePosts");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserId",
                table: "Comments");

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserName",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ApprovedByUserName",
                table: "Comments",
                nullable: true);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovedByUserName",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ApprovedByUserName",
                table: "Comments");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "SharePosts",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedByUserId",
                table: "Posts",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApprovedByUserId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true);
        }
    }
}
