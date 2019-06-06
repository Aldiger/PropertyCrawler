using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RightMove.Data.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Portal",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        DateAdded = table.Column<DateTime>(nullable: false),
            //        DateModified = table.Column<DateTime>(nullable: false),
            //        Active = table.Column<bool>(nullable: false),
            //        Name = table.Column<string>(nullable: true),
            //        Url = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Portal", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "PostalCodes",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        DateAdded = table.Column<DateTime>(nullable: false),
            //        DateModified = table.Column<DateTime>(nullable: false),
            //        Active = table.Column<bool>(nullable: false),
            //        Code = table.Column<string>(nullable: true),
            //        OpCode = table.Column<string>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_PostalCodes", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Url",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
            //        DateAdded = table.Column<DateTime>(nullable: false),
            //        DateModified = table.Column<DateTime>(nullable: false),
            //        Active = table.Column<bool>(nullable: false),
            //        PropertyUrl = table.Column<string>(nullable: true),
            //        Type = table.Column<int>(nullable: false),
            //        PostalCodeId = table.Column<int>(nullable: false),
            //        PortalId = table.Column<int>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Url", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Url_Portal_PortalId",
            //            column: x => x.PortalId,
            //            principalTable: "Portal",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_Url_PostalCodes_PostalCodeId",
            //            column: x => x.PostalCodeId,
            //            principalTable: "PostalCodes",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Url_PortalId",
            //    table: "Url",
            //    column: "PortalId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Url_PostalCodeId",
            //    table: "Url",
            //    column: "PostalCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Url");

            migrationBuilder.DropTable(
                name: "Portal");

            migrationBuilder.DropTable(
                name: "PostalCodes");
        }
    }
}
