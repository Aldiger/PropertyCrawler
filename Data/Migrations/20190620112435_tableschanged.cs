using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class tableschanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessPostalCodeUrlFaileds",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    ProcessPostalCodeId = table.Column<int>(nullable: false),
                    UrlId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessPostalCodeUrlFaileds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessPostalCodeUrlFaileds_ProcessPostalCodes_ProcessPostalCodeId",
                        column: x => x.ProcessPostalCodeId,
                        principalTable: "ProcessPostalCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProcessPostalCodeUrlFaileds_Urls_UrlId",
                        column: x => x.UrlId,
                        principalTable: "Urls",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessPostalCodeUrlFaileds_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFaileds",
                column: "ProcessPostalCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProcessPostalCodeUrlFaileds_UrlId",
                table: "ProcessPostalCodeUrlFaileds",
                column: "UrlId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessPostalCodeUrlFaileds");
        }
    }
}
