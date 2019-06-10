using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class propertydetailstables2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PropertyDescriptions_Properties_PropertyId",
                table: "PropertyDescriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_Urls_Properties_PropertyId",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Urls_PropertyId",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_PropertyDescriptions_PropertyId",
                table: "PropertyDescriptions");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_UrlId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "PropertyDescriptions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId",
                unique: true,
                filter: "[PropertyDescriptionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UrlId",
                table: "Properties",
                column: "UrlId",
                unique: true,
                filter: "[UrlId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_UrlId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "PropertyId",
                table: "PropertyDescriptions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Urls_PropertyId",
                table: "Urls",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDescriptions_PropertyId",
                table: "PropertyDescriptions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UrlId",
                table: "Properties",
                column: "UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_PropertyDescriptions_Properties_PropertyId",
                table: "PropertyDescriptions",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_Properties_PropertyId",
                table: "Urls",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
