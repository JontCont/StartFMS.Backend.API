using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartFMS.Backend.API.Migrations
{
    public partial class Update230701 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TypeCode",
                table: "B10_LineMessageType",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TypeCode",
                table: "B10_LineMessageType");
        }
    }
}
