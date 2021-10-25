using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.CoreDbContextMSSql
{
    public partial class MSSqlCoreDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tenants_buttons",
                columns: table => new
                {
                    tariff_id = table.Column<int>(type: "int", nullable: false),
                    partner_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    button_url = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tenants_buttons_pkey", x => new { x.tariff_id, x.partner_id });
                });

            migrationBuilder.CreateTable(
                name: "tenants_quota",
                columns: table => new
                {
                    tenant = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "varchar(128)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    description = table.Column<string>(type: "varchar(128)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    max_file_size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    max_total_size = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    active_users = table.Column<int>(type: "int", nullable: false),
                    features = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    price = table.Column<decimal>(type: "decimal(10,2)", nullable: false, defaultValue: 0m),
                    avangate_id = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    visible = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("tenants_quota_pkey", x => x.tenant);
                });

            migrationBuilder.CreateTable(
                name: "tenants_quotarow",
                columns: table => new
                {
                    tenant = table.Column<int>(type: "int", nullable: false),
                    path = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    counter = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    tag = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true, defaultValue: "0", collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    last_modified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("tenants_quotarow_pkey", x => new { x.tenant, x.path });
                });

            migrationBuilder.CreateTable(
                name: "tenants_tariff",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant = table.Column<int>(type: "int", nullable: false),
                    tariff = table.Column<int>(type: "int", nullable: false),
                    stamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    create_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tenants_tariff", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "tenants_quota",
                columns: new[] { "tenant", "active_users", "avangate_id", "description", "features", "max_file_size", "max_total_size", "name", "visible" },
                values: new object[] { -1, 10000, "0", null, "domain,audit,controlpanel,healthcheck,ldap,sso,whitelabel,branding,ssbranding,update,support,portals:10000,discencryption,privacyroom,restore", 102400L, 10995116277760L, "default", false });

            migrationBuilder.CreateIndex(
                name: "last_modified_tenants_quotarow",
                table: "tenants_quotarow",
                column: "last_modified");

            migrationBuilder.CreateIndex(
                name: "tenant_tenants_tariff",
                table: "tenants_tariff",
                column: "tenant");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tenants_buttons");

            migrationBuilder.DropTable(
                name: "tenants_quota");

            migrationBuilder.DropTable(
                name: "tenants_quotarow");

            migrationBuilder.DropTable(
                name: "tenants_tariff");
        }
    }
}
