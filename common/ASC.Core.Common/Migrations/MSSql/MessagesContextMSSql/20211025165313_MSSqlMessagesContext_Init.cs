using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.MessagesContextMSSql
{
    public partial class MSSqlMessagesContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "login_events",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    login = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    ip = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    browser = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    platform = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    page = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    action = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_login_events", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "date_login_events",
                table: "login_events",
                column: "date");

            migrationBuilder.CreateIndex(
                name: "tenant_id_login_events",
                table: "login_events",
                columns: new[] { "user_id", "tenant_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "login_events");
        }
    }
}
