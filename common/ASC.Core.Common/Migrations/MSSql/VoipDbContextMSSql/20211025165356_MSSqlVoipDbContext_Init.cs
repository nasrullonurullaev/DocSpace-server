using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.VoipDbContextMSSql
{
    public partial class MSSqlVoipDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "crm_contact",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    is_company = table.Column<bool>(type: "bit", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    first_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    last_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    company_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    industry = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    status_id = table.Column<int>(type: "int", nullable: false),
                    company_id = table.Column<int>(type: "int", nullable: false),
                    contact_type_id = table.Column<int>(type: "int", nullable: false),
                    create_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    create_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_modifed_by = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    last_modifed_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    display_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    is_shared = table.Column<bool>(type: "bit", nullable: false),
                    currency = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crm_contact", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "crm_voip_number",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    number = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    alias = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    settings = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    tenant_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crm_voip_number", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "crm_voip_calls",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    parent_call_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    number_from = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    number_to = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    status = table.Column<int>(type: "int", nullable: false),
                    answered_by = table.Column<Guid>(type: "uniqueidentifier", maxLength: 50, nullable: false, defaultValue: new Guid("00000000-0000-0000-0000-000000000000")),
                    dial_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dial_duration = table.Column<int>(type: "int", nullable: false),
                    record_sid = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    record_url = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    record_duration = table.Column<int>(type: "int", nullable: false),
                    record_price = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    contact_id = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(10,4)", nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    CrmContactId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_crm_voip_calls", x => x.id);
                    table.ForeignKey(
                        name: "FK_crm_voip_calls_crm_contact_CrmContactId",
                        column: x => x.CrmContactId,
                        principalTable: "crm_contact",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "company_id",
                table: "crm_contact",
                columns: new[] { "tenant_id", "company_id" });

            migrationBuilder.CreateIndex(
                name: "create_on_crm_contact",
                table: "crm_contact",
                column: "create_on");

            migrationBuilder.CreateIndex(
                name: "display_name",
                table: "crm_contact",
                columns: new[] { "tenant_id", "display_name" });

            migrationBuilder.CreateIndex(
                name: "last_modifed_on_crm_contact",
                table: "crm_contact",
                columns: new[] { "last_modifed_on", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "IX_crm_voip_calls_CrmContactId",
                table: "crm_voip_calls",
                column: "CrmContactId");

            migrationBuilder.CreateIndex(
                name: "parent_call_id",
                table: "crm_voip_calls",
                columns: new[] { "parent_call_id", "tenant_id" });

            migrationBuilder.CreateIndex(
                name: "tenant_id_crm_voip_calls",
                table: "crm_voip_calls",
                column: "tenant_id");

            migrationBuilder.CreateIndex(
                name: "tenant_id_crm_voip_number",
                table: "crm_voip_number",
                column: "tenant_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "crm_voip_calls");

            migrationBuilder.DropTable(
                name: "crm_voip_number");

            migrationBuilder.DropTable(
                name: "crm_contact");
        }
    }
}
