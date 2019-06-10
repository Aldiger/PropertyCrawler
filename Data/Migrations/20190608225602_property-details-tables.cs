using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PropertyCrawler.Data.Migrations
{
    public partial class propertydetailstables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PriceType",
                table: "Properties",
                newName: "PropertySubType");

            migrationBuilder.RenameColumn(
                name: "NumberOfBedrooms",
                table: "Properties",
                newName: "BedroomsCount");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Properties",
                newName: "LettingType");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "Properties",
                newName: "Added");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Agents",
                newName: "DisplayAddress");

            migrationBuilder.RenameColumn(
                name: "LogoUrl",
                table: "Agents",
                newName: "CompanyType");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Agents",
                newName: "AgentCode");

            migrationBuilder.RenameColumn(
                name: "Address",
                table: "Agents",
                newName: "CompanyName");

            migrationBuilder.AddColumn<int>(
                name: "PropertyCode",
                table: "Urls",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Urls",
                nullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Longtitude",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AlterColumn<double>(
                name: "Latitude",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<int>(
                name: "FloorPlanCount",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PropertyDescriptionId",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UrlId",
                table: "Properties",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OutCode",
                table: "PostalCodes",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Caption",
                table: "Images",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "AgentType",
                table: "Agents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchName",
                table: "Agents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BranchPostcode",
                table: "Agents",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrandName",
                table: "Agents",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PropertyDescriptions",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    PropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyDescriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyDescriptions_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PropertyPrices",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateAdded = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    PriceQualifier = table.Column<string>(nullable: true),
                    PropertyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyPrices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyPrices_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Urls_PropertyId",
                table: "Urls",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_UrlId",
                table: "Properties",
                column: "UrlId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyDescriptions_PropertyId",
                table: "PropertyDescriptions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPrices_PropertyId",
                table: "PropertyPrices",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_PropertyDescriptions_PropertyDescriptionId",
                table: "Properties",
                column: "PropertyDescriptionId",
                principalTable: "PropertyDescriptions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Urls_UrlId",
                table: "Properties",
                column: "UrlId",
                principalTable: "Urls",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_Properties_PropertyId",
                table: "Urls",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_PropertyDescriptions_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Urls_UrlId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Urls_Properties_PropertyId",
                table: "Urls");

            migrationBuilder.DropTable(
                name: "PropertyDescriptions");

            migrationBuilder.DropTable(
                name: "PropertyPrices");

            migrationBuilder.DropIndex(
                name: "IX_Urls_PropertyId",
                table: "Urls");

            migrationBuilder.DropIndex(
                name: "IX_Properties_PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_UrlId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyCode",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Urls");

            migrationBuilder.DropColumn(
                name: "FloorPlanCount",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "PropertyDescriptionId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UrlId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "OutCode",
                table: "PostalCodes");

            migrationBuilder.DropColumn(
                name: "AgentType",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "BranchName",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "BranchPostcode",
                table: "Agents");

            migrationBuilder.DropColumn(
                name: "BrandName",
                table: "Agents");

            migrationBuilder.RenameColumn(
                name: "PropertySubType",
                table: "Properties",
                newName: "PriceType");

            migrationBuilder.RenameColumn(
                name: "LettingType",
                table: "Properties",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "BedroomsCount",
                table: "Properties",
                newName: "NumberOfBedrooms");

            migrationBuilder.RenameColumn(
                name: "Added",
                table: "Properties",
                newName: "City");

            migrationBuilder.RenameColumn(
                name: "DisplayAddress",
                table: "Agents",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "CompanyType",
                table: "Agents",
                newName: "LogoUrl");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "Agents",
                newName: "Address");

            migrationBuilder.RenameColumn(
                name: "AgentCode",
                table: "Agents",
                newName: "Code");

            migrationBuilder.AlterColumn<float>(
                name: "Longtitude",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<float>(
                name: "Latitude",
                table: "Properties",
                nullable: false,
                oldClrType: typeof(double));

            migrationBuilder.AlterColumn<int>(
                name: "Caption",
                table: "Images",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
