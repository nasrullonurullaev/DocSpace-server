using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.NotifyDbContextMSSql
{
    public partial class MSSqlNotifyDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "notify_info",
                columns: table => new
                {
                    notify_id = table.Column<int>(type: "int", nullable: false),
                    state = table.Column<int>(type: "int", nullable: false),
                    attempts = table.Column<int>(type: "int", nullable: false),
                    modify_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    priority = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("notify_info_pkey", x => x.notify_id);
                });

            migrationBuilder.CreateTable(
                name: "notify_queue",
                columns: table => new
                {
                    notify_id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    sender = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    reciever = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    subject = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    content_type = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    sender_type = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    reply_to = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    creation_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    attachments = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    auto_submitted = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("notify_queue_pkey", x => x.notify_id);
                });

            migrationBuilder.CreateIndex(
                name: "state",
                table: "notify_info",
                column: "state");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "notify_info");

            migrationBuilder.DropTable(
                name: "notify_queue");
        }
    }
}
