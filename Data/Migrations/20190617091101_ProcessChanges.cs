using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class ProcessChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_UrlType_UrlTypeId",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlType",
                table: "UrlType");

            migrationBuilder.RenameTable(
                name: "UrlType",
                newName: "UrlTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlTypes",
                table: "UrlTypes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Processes",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Processes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProcessPostalCode",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    ProcessId = table.Column<int>(nullable: false),
                    PostalCodeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessPostalCode", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessPostalCode_Processes_ProcessId",
                        column: x => x.ProcessId,
                        principalTable: "Processes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessPostalCode_ProcessId",
                table: "ProcessPostalCode",
                column: "ProcessId");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_UrlTypes_UrlTypeId",
                table: "Urls",
                column: "UrlTypeId",
                principalTable: "UrlTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_UrlTypes_UrlTypeId",
                table: "Urls");

            migrationBuilder.DropTable(
                name: "ProcessPostalCode");

            migrationBuilder.DropTable(
                name: "Processes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UrlTypes",
                table: "UrlTypes");

            migrationBuilder.RenameTable(
                name: "UrlTypes",
                newName: "UrlType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UrlType",
                table: "UrlType",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_UrlType_UrlTypeId",
                table: "Urls",
                column: "UrlTypeId",
                principalTable: "UrlType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
