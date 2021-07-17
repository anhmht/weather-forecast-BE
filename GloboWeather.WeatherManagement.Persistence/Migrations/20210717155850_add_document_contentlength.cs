using Microsoft.EntityFrameworkCore.Migrations;

namespace GloboWeather.WeatherManagement.Persistence.Migrations
{
    public partial class add_document_contentlength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContentLength",
                table: "EventDocuments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentLength",
                table: "EventDocuments");
        }
    }
}
