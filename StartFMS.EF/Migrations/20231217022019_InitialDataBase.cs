using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartFMS.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SystemCatalogItems",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false, comment: "目錄識別碼")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MenuName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "目錄名稱"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValue: "", comment: "備註"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, comment: "顯示順序 (透過Id抓取，判斷在第幾層位置)"),
                    Url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, defaultValue: "", comment: "網址.."),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValue: "", comment: "Icon"),
                    ParentId = table.Column<int>(type: "int", nullable: true, comment: "父層ID (目前設為 Id)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemCatalogItems", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserAccounts",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())", comment: "識別碼"),
                    Account = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "帳號"),
                    Password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "密碼"),
                    UserRoleID = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "使用者角色ID (UserRole)"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true, comment: "是否啟用")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccounts", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())", comment: "識別碼"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "名稱", collation: "SQL_Latin1_General_CP1_CI_AS"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "備註"),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemCatalogItems");

            migrationBuilder.DropTable(
                name: "UserAccounts");

            migrationBuilder.DropTable(
                name: "UserRole");
        }
    }
}
