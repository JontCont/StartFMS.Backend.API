using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StartFMS.Backend.API.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "A00_Division",
                columns: table => new
                {
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__A00_Divi__20EFC6A8CCB5E78C", x => x.DivisionId);
                });

            migrationBuilder.CreateTable(
                name: "A00_JobTitle",
                columns: table => new
                {
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__A00_JobT__35382FE99AC0583C", x => x.JobTitleId);
                });

            migrationBuilder.CreateTable(
                name: "A00_Role",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__A00_Role__8AFACE1A3E7C0A0F", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "B10_LineMessageOption",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, defaultValueSql: "('')"),
                    type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValueSql: "('')"),
                    IsUse = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "(N'false')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B10_LineMessageOption", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "B10_LineMessageType",
                columns: table => new
                {
                    TypeId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TypeMemo = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValueSql: "('')")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B10_LineMessageType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "S01_MenuBasicSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    MenuName = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, defaultValueSql: "('')"),
                    DisplayOrder = table.Column<int>(type: "int", nullable: false, comment: "顯示順序 (透過Id抓取，判斷在第幾層位置)"),
                    Url = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, defaultValueSql: "('')"),
                    Icon = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, defaultValueSql: "('')", comment: "畫面Icon"),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true, comment: "父層ID (目前設為 Id)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_S01_MenuBasicSetting", x => x.Id);
                    table.ForeignKey(
                        name: "FK_S01_MenuBasicSetting_S01_MenuBasicSetting_ParentId",
                        column: x => x.ParentId,
                        principalTable: "S01_MenuBasicSetting",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "S10_SystemConfig",
                columns: table => new
                {
                    par_name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    par_value = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    par_memo = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_S10_SystemConfig", x => x.par_name);
                });

            migrationBuilder.CreateTable(
                name: "A00_Account",
                columns: table => new
                {
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "(newid())"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Account = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JobTitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DivisionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__A00_Acco__7AD04F11B573F9A5", x => x.EmployeeId);
                    table.ForeignKey(
                        name: "FK_Employee_ToTable",
                        column: x => x.JobTitleId,
                        principalTable: "A00_JobTitle",
                        principalColumn: "JobTitleId");
                    table.ForeignKey(
                        name: "FK_Employee_ToTable_1",
                        column: x => x.DivisionId,
                        principalTable: "A00_Division",
                        principalColumn: "DivisionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_A00_Account_DivisionId",
                table: "A00_Account",
                column: "DivisionId");

            migrationBuilder.CreateIndex(
                name: "IX_A00_Account_JobTitleId",
                table: "A00_Account",
                column: "JobTitleId");

            migrationBuilder.CreateIndex(
                name: "IX_S01_MenuBasicSetting_ParentId",
                table: "S01_MenuBasicSetting",
                column: "ParentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "A00_Account");

            migrationBuilder.DropTable(
                name: "A00_Role");

            migrationBuilder.DropTable(
                name: "B10_LineMessageOption");

            migrationBuilder.DropTable(
                name: "B10_LineMessageType");

            migrationBuilder.DropTable(
                name: "S01_MenuBasicSetting");

            migrationBuilder.DropTable(
                name: "S10_SystemConfig");

            migrationBuilder.DropTable(
                name: "A00_JobTitle");

            migrationBuilder.DropTable(
                name: "A00_Division");
        }
    }
}
