using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class change : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Properties_UrlId",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "PropertyId",
                table: "Urls",
                newName: "UrlTypeId");

            migrationBuilder.AddColumn<string>(
                name: "PostalCodeExtended",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCodeFull",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostalCodePrefix",
                table: "Properties",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Properties",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "PropertyAdded",
                table: "Properties",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Type",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UrlType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    UrlPortion = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UrlType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urls_UrlTypeId",
                table: "Urls",
                column: "UrlTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UrlId",
                table: "Properties",
                column: "UrlId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_UrlType_UrlTypeId",
                table: "Urls",
                column: "UrlTypeId",
                principalTable: "UrlType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_UrlType_UrlTypeId",
                table: "Urls");

            migrationBuilder.DropTable(
                name: "UrlType");

            migrationBuilder.DropIndex(
                name: "IX_Urls_UrlTypeId",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Properties_UrlId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PostalCodeExtended",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PostalCodeFull",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PostalCodePrefix",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyAdded",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Properties");

            migrationBuilder.RenameColumn(
                name: "UrlTypeId",
                table: "Urls",
                newName: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UrlId",
                table: "Properties",
                column: "UrlId",
                unique: true,
                filter: "[UrlId] IS NOT NULL");
        }
    }
}
