using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class propertydescriptionchange2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PropertyUrl",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "OpCode",
                table: "PostalCodes");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PropertyUrl",
                table: "Urls",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OpCode",
                table: "PostalCodes",
                nullable: true);
        }
    }
}
