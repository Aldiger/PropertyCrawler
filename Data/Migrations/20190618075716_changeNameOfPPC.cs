using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class changeNameOfPPC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCode_PostalCodes_PostalCodeId",
                table: "ProcessPostalCode");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCode_Processes_ProcessId",
                table: "ProcessPostalCode");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessPostalCode",
                table: "ProcessPostalCode");

            migrationBuilder.RenameTable(
                name: "ProcessPostalCode",
                newName: "ProcessPostalCodes");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCode_ProcessId",
                table: "ProcessPostalCodes",
                newName: "IX_ProcessPostalCodes_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCode_PostalCodeId",
                table: "ProcessPostalCodes",
                newName: "IX_ProcessPostalCodes_PostalCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPostalCodes",
                table: "ProcessPostalCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCodes_PostalCodes_PostalCodeId",
                table: "ProcessPostalCodes",
                column: "PostalCodeId",
                principalTable: "PostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCodes_Processes_ProcessId",
                table: "ProcessPostalCodes",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCodes_PostalCodes_PostalCodeId",
                table: "ProcessPostalCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_ProcessPostalCodes_Processes_ProcessId",
                table: "ProcessPostalCodes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProcessPostalCodes",
                table: "ProcessPostalCodes");

            migrationBuilder.RenameTable(
                name: "ProcessPostalCodes",
                newName: "ProcessPostalCode");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCodes_ProcessId",
                table: "ProcessPostalCode",
                newName: "IX_ProcessPostalCode_ProcessId");

            migrationBuilder.RenameIndex(
                name: "IX_ProcessPostalCodes_PostalCodeId",
                table: "ProcessPostalCode",
                newName: "IX_ProcessPostalCode_PostalCodeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProcessPostalCode",
                table: "ProcessPostalCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCode_PostalCodes_PostalCodeId",
                table: "ProcessPostalCode",
                column: "PostalCodeId",
                principalTable: "PostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProcessPostalCode_Processes_ProcessId",
                table: "ProcessPostalCode",
                column: "ProcessId",
                principalTable: "Processes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
