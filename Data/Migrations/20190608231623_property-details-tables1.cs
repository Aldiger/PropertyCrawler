using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class propertydetailstables1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyDescriptions_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Urls_UrlId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "UrlId",
                table: "Properties",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "PropertyDescriptionId",
                table: "Properties",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyDescriptions_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId",
                principalTable: "PropertyDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Urls_UrlId",
                table: "Properties",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyDescriptions_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Urls_UrlId",
                table: "Properties");

            migrationBuilder.AlterColumn<int>(
                name: "UrlId",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PropertyDescriptionId",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyDescriptions_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId",
                principalTable: "PropertyDescriptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Urls_UrlId",
                table: "Properties",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
