using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.AccountLinkContextMSSql
{
    public partial class MSSqlAccountLinkContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "account_links",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    uid = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    provider = table.Column<string>(type: "nchar(60)", fixedLength: true, maxLength: 60, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    profile = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    linked = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("account_links_pkey", x => new { x.id, x.uid });
                });

            migrationBuilder.CreateIndex(
                name: "uid",
                table: "account_links",
                column: "uid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "account_links");
        }
    }
}
