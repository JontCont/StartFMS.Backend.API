using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartFMS.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMenuItemsToImportAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImportAt",
                table: "SystemCatalogItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsGroup",
                table: "SystemCatalogItems",
                type: "bit",
                nullable: true,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportAt",
                table: "SystemCatalogItems");

            migrationBuilder.DropColumn(
                name: "IsGroup",
                table: "SystemCatalogItems");
        }
    }
}
