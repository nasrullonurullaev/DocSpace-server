using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.TelegramDbContextMSSql
{
    public partial class MSSqlTelegramDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "telegram_users",
                columns: table => new
                {
                    portal_user_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    telegram_user_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("telegram_users_pkey", x => new { x.tenant_id, x.portal_user_id });
                });

            migrationBuilder.CreateIndex(
                name: "tgId",
                table: "telegram_users",
                column: "telegram_user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "telegram_users");
        }
    }
}
