using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RightMove.Migrations
{
    public partial class updateschemaportal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Portal",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Url = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Portal", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Url_PortalId",
                table: "Url",
                column: "PortalId");

            migrationBuilder.AddForeignKey(
                name: "FK_Url_Portal_PortalId",
                table: "Url",
                column: "PortalId",
                principalTable: "Portal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Url_Portal_PortalId",
                table: "Url");

            migrationBuilder.DropTable(
                name: "Portal");

            migrationBuilder.DropIndex(
                name: "IX_Url_PortalId",
                table: "Url");
        }
    }
}
