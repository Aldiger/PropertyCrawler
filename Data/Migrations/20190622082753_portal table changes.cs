using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class portaltablechanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCodeUrlFaileds_ProcessPostalCodes_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFaileds");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCodeUrlFaileds_Urls_UrlId",
                table: "ProcessPostalCodeUrlFaileds");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessPostalCodeUrlFaileds",
                table: "ProcessPostalCodeUrlFaileds");

            migrationBuilder.RenameTable(
                name: "ProcessPostalCodeUrlFaileds",
                newName: "ProcessPostalCodeUrlFails");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCodeUrlFaileds_UrlId",
                table: "ProcessPostalCodeUrlFails",
                newName: "IX_ProcessPostalCodeUrlFails_UrlId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCodeUrlFaileds_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFails",
                newName: "IX_ProcessPostalCodeUrlFails_ProcessPostalCodeId");

            migrationBuilder.AddColumn<string>(
                name: "OutCodeKey",
                table: "Portals",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FailReason",
                table: "ProcessPostalCodeUrlFails",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPostalCodeUrlFails",
                table: "ProcessPostalCodeUrlFails",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCodeUrlFails_ProcessPostalCodes_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFails",
                column: "ProcessPostalCodeId",
                principalTable: "ProcessPostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCodeUrlFails_Urls_UrlId",
                table: "ProcessPostalCodeUrlFails",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCodeUrlFails_ProcessPostalCodes_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFails");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCodeUrlFails_Urls_UrlId",
                table: "ProcessPostalCodeUrlFails");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessPostalCodeUrlFails",
                table: "ProcessPostalCodeUrlFails");

            migrationBuilder.DropColumn(
                name: "OutCodeKey",
                table: "Portals");

            migrationBuilder.DropColumn(
                name: "FailReason",
                table: "ProcessPostalCodeUrlFails");

            migrationBuilder.RenameTable(
                name: "ProcessPostalCodeUrlFails",
                newName: "ProcessPostalCodeUrlFaileds");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCodeUrlFails_UrlId",
                table: "ProcessPostalCodeUrlFaileds",
                newName: "IX_ProcessPostalCodeUrlFaileds_UrlId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCodeUrlFails_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFaileds",
                newName: "IX_ProcessPostalCodeUrlFaileds_ProcessPostalCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPostalCodeUrlFaileds",
                table: "ProcessPostalCodeUrlFaileds",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCodeUrlFaileds_ProcessPostalCodes_ProcessPostalCodeId",
                table: "ProcessPostalCodeUrlFaileds",
                column: "ProcessPostalCodeId",
                principalTable: "ProcessPostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCodeUrlFaileds_Urls_UrlId",
                table: "ProcessPostalCodeUrlFaileds",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }
    }
}
