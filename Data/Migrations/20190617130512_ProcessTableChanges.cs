using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class ProcessTableChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PropertyType",
                table: "Processes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ProcessPostalCode_PostalCodeId",
                table: "ProcessPostalCode",
                column: "PostalCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCode_PostalCodes_PostalCodeId",
                table: "ProcessPostalCode",
                column: "PostalCodeId",
                principalTable: "PostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCode_PostalCodes_PostalCodeId",
                table: "ProcessPostalCode");

            migrationBuilder.DropIndex(
                name: "IX_ProcessPostalCode_PostalCodeId",
                table: "ProcessPostalCode");

            migrationBuilder.DropColumn(
                name: "PropertyType",
                table: "Processes");
        }
    }
}
