using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class add_videoUrl_Ios : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VideoUrlsIos",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrlsIos",
                table: "Comments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VideoUrlsIos",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "VideoUrlsIos",
                table: "Comments");
        }
    }
}
