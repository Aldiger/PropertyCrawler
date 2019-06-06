using Microsoft.EntityFrameworkCore.Migrations;

namespace RightMove.Data.Migrations
{
    public partial class added_new_tables1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Property_PropertyId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Property_Agents_AgentId",
                table: "Property");

            migrationBuilder.DropForeignKey(
                name: "FK_Url_Portal_PortalId",
                table: "Url");

            migrationBuilder.DropForeignKey(
                name: "FK_Url_PostalCodes_PostalCodeId",
                table: "Url");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Url",
                table: "Url");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Property",
                table: "Property");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portal",
                table: "Portal");

            migrationBuilder.RenameTable(
                name: "Url",
                newName: "Urls");

            migrationBuilder.RenameTable(
                name: "Property",
                newName: "Properties");

            migrationBuilder.RenameTable(
                name: "Portal",
                newName: "Portals");

            migrationBuilder.RenameIndex(
                name: "IX_Url_PostalCodeId",
                table: "Urls",
                newName: "IX_Urls_PostalCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_Url_PortalId",
                table: "Urls",
                newName: "IX_Urls_PortalId");

            migrationBuilder.RenameIndex(
                name: "IX_Property_AgentId",
                table: "Properties",
                newName: "IX_Properties_AgentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Properties",
                table: "Properties",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portals",
                table: "Portals",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Properties_PropertyId",
                table: "Images",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Agents_AgentId",
                table: "Properties",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_Portals_PortalId",
                table: "Urls",
                column: "PortalId",
                principalTable: "Portals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_PostalCodes_PostalCodeId",
                table: "Urls",
                column: "PostalCodeId",
                principalTable: "PostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_Properties_PropertyId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Agents_AgentId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Urls_Portals_PortalId",
                table: "Urls");

            migrationBuilder.DropForeignKey(
                name: "FK_Urls_PostalCodes_PostalCodeId",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Properties",
                table: "Properties");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Portals",
                table: "Portals");

            migrationBuilder.RenameTable(
                name: "Urls",
                newName: "Url");

            migrationBuilder.RenameTable(
                name: "Properties",
                newName: "Property");

            migrationBuilder.RenameTable(
                name: "Portals",
                newName: "Portal");

            migrationBuilder.RenameIndex(
                name: "IX_Urls_PostalCodeId",
                table: "Url",
                newName: "IX_Url_PostalCodeId");

            migrationBuilder.RenameIndex(
                name: "IX_Urls_PortalId",
                table: "Url",
                newName: "IX_Url_PortalId");

            migrationBuilder.RenameIndex(
                name: "IX_Properties_AgentId",
                table: "Property",
                newName: "IX_Property_AgentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Url",
                table: "Url",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Property",
                table: "Property",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Portal",
                table: "Portal",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_Property_PropertyId",
                table: "Images",
                column: "PropertyId",
                principalTable: "Property",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Property_Agents_AgentId",
                table: "Property",
                column: "AgentId",
                principalTable: "Agents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Url_Portal_PortalId",
                table: "Url",
                column: "PortalId",
                principalTable: "Portal",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Url_PostalCodes_PostalCodeId",
                table: "Url",
                column: "PostalCodeId",
                principalTable: "PostalCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
