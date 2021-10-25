using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.ResourceDbContextMSSql
{
    public partial class MSSqlResourceDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "res_authors",
                columns: table => new
                {
                    login = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    password = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false),
                    online = table.Column<bool>(type: "bit", nullable: false),
                    lastVisit = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("res_authors_pkey", x => x.login);
                });

            migrationBuilder.CreateTable(
                name: "res_authorsfile",
                columns: table => new
                {
                    authorLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    fileid = table.Column<int>(type: "int", nullable: false),
                    writeAccess = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("res_authorsfile_pkey", x => new { x.authorLogin, x.fileid });
                });

            migrationBuilder.CreateTable(
                name: "res_authorslang",
                columns: table => new
                {
                    authorLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    cultureTitle = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("res_authorslang_pkey", x => new { x.authorLogin, x.cultureTitle });
                });

            migrationBuilder.CreateTable(
                name: "res_cultures",
                columns: table => new
                {
                    title = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    available = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    creationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("res_cultures_pkey", x => x.title);
                });

            migrationBuilder.CreateTable(
                name: "res_data",
                columns: table => new
                {
                    fileid = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    cultureTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textValue = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    timeChanges = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    resourceType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    flag = table.Column<int>(type: "int", nullable: false),
                    link = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    authorLogin = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, defaultValue: "Console", collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("res_data_pkey", x => new { x.fileid, x.cultureTitle, x.title });
                });

            migrationBuilder.CreateTable(
                name: "res_files",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    projectName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    moduleName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    resName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    isLock = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    lastUpdate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    creationDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "'1975-03-03 00:00:00'")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_res_files", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "res_reserve",
                columns: table => new
                {
                    fileid = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(120)", maxLength: 120, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    cultureTitle = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    textValue = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    flag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("res_reserve_pkey", x => new { x.fileid, x.title, x.cultureTitle });
                });

            migrationBuilder.CreateIndex(
                name: "res_authorsfile_FK2",
                table: "res_authorsfile",
                column: "fileid");

            migrationBuilder.CreateIndex(
                name: "res_authorslang_FK2",
                table: "res_authorslang",
                column: "cultureTitle");

            migrationBuilder.CreateIndex(
                name: "dateIndex",
                table: "res_data",
                column: "timeChanges");

            migrationBuilder.CreateIndex(
                name: "id_res_data",
                table: "res_data",
                column: "id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "resources_FK2",
                table: "res_data",
                column: "cultureTitle");

            migrationBuilder.CreateIndex(
                name: "resname",
                table: "res_files",
                column: "resName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "res_authors");

            migrationBuilder.DropTable(
                name: "res_authorsfile");

            migrationBuilder.DropTable(
                name: "res_authorslang");

            migrationBuilder.DropTable(
                name: "res_cultures");

            migrationBuilder.DropTable(
                name: "res_data");

            migrationBuilder.DropTable(
                name: "res_files");

            migrationBuilder.DropTable(
                name: "res_reserve");
        }
    }
}
