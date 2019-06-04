using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RightMove.Migrations
{
    public partial class updateschema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "Url",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "Url",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "Url",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PortalId",
                table: "Url",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PostalCodeId",
                table: "Url",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "Active",
                table: "PostalCodes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAdded",
                table: "PostalCodes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateModified",
                table: "PostalCodes",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OpCode",
                table: "PostalCodes",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Url_PostalCodeId",
                table: "Url",
                column: "PostalCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Url_PostalCodes_PostalCodeId",
                table: "Url",
                column: "PostalCodeId",
                principalTable: "PostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Url_PostalCodes_PostalCodeId",
                table: "Url");

            migrationBuilder.DropIndex(
                name: "IX_Url_PostalCodeId",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "PortalId",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "PostalCodeId",
                table: "Url");

            migrationBuilder.DropColumn(
                name: "Active",
                table: "PostalCodes");

            migrationBuilder.DropColumn(
                name: "DateAdded",
                table: "PostalCodes");

            migrationBuilder.DropColumn(
                name: "DateModified",
                table: "PostalCodes");

            migrationBuilder.DropColumn(
                name: "OpCode",
                table: "PostalCodes");
        }
    }
}
