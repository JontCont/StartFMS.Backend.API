using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartFMS.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "UserAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UserAccounts",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                collation: "SQL_Latin1_General_CP1_CI_AS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "UserAccounts");
        }
    }
}
