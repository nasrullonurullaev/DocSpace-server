using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.WebstudioDbContextMSSql
{
    public partial class MSSqlWebstudioDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "webstudio_index",
                columns: table => new
                {
                    index_name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    last_modified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("webstudio_index_pkey", x => x.index_name);
                });

            migrationBuilder.CreateTable(
                name: "webstudio_settings",
                columns: table => new
                {
                    TenantID = table.Column<int>(type: "int", nullable: false),
                    ID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 64, nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", maxLength: 64, nullable: false),
                    Data = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("webstudio_settings_pkey", x => new { x.TenantID, x.ID, x.UserID });
                });

            migrationBuilder.CreateTable(
                name: "webstudio_uservisit",
                columns: table => new
                {
                    tenantid = table.Column<int>(type: "int", nullable: false),
                    visitdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    productid = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    userid = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    visitcount = table.Column<int>(type: "int", nullable: false),
                    firstvisittime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    lastvisittime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("webstudio_uservisit_pkey", x => new { x.tenantid, x.visitdate, x.productid, x.userid });
                });

            migrationBuilder.InsertData(
                table: "webstudio_settings",
                columns: new[] { "ID", "TenantID", "UserID", "Data" },
                values: new object[] { new Guid("9a925891-1f92-4ed7-b277-d6f649739f06"), 1, new Guid("00000000-0000-0000-0000-000000000000"), "{\"Completed\":false}" });

            migrationBuilder.CreateIndex(
                name: "ID",
                table: "webstudio_settings",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "visitdate",
                table: "webstudio_uservisit",
                column: "visitdate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "webstudio_index");

            migrationBuilder.DropTable(
                name: "webstudio_settings");

            migrationBuilder.DropTable(
                name: "webstudio_uservisit");
        }
    }
}
