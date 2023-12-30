using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartFMS.EF.Migrations
{
    /// <inheritdoc />
    public partial class 加入系統參數 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImportAt",
                table: "SystemCatalogItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldDefaultValue: "");

            migrationBuilder.CreateTable(
                name: "SystemParameter",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "識別碼")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false, comment: "名稱"),
                    Value1 = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, comment: "參數1"),
                    Value2 = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, comment: "參數2"),
                    Value3 = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: true, comment: "參數3"),
                    Description = table.Column<string>(type: "varchar(500)", unicode: false, maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SystemParameter", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SystemParameter");

            migrationBuilder.AlterColumn<string>(
                name: "ImportAt",
                table: "SystemCatalogItems",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldNullable: true);
        }
    }
}
