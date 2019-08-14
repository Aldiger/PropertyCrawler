using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class addretrytoprocesstable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Retry",
                table: "Processes",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Retry",
                table: "Processes");
        }
    }
}
