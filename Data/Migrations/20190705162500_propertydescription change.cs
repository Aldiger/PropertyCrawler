using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class propertydescriptionchange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDescriptions_PropertyId",
                table: "PropertyDescriptions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDescriptions_Properties_PropertyId",
                table: "PropertyDescriptions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDescriptions_Properties_PropertyId",
                table: "PropertyDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDescriptions_PropertyId",
                table: "PropertyDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId",
                unique: true,
                filter: "[PropertyDescriptionId] IS NOT NULL");
        }
    }
}
