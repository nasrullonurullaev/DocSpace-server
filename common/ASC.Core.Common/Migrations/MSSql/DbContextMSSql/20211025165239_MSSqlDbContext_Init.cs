using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.DbContextMSSql
{
    public partial class MSSqlDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "dbip_location",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    addr_type = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    ip_start = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    ip_end = table.Column<string>(type: "nvarchar(39)", maxLength: 39, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    country = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    stateprov = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    district = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    city = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    zipcode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    latitude = table.Column<long>(type: "bigint", nullable: false),
                    longitude = table.Column<long>(type: "bigint", nullable: false),
                    geoname_id = table.Column<int>(type: "int", nullable: false),
                    timezone_offset = table.Column<double>(type: "float", nullable: false),
                    timezone_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    processed = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dbip_location", x => x.id);
                    table.CheckConstraint("constraint_addr_type", "[addr_type] = 'ipv4' or [addr_type] = 'ipv6'");
                });

            migrationBuilder.CreateTable(
                name: "mobile_app_install",
                columns: table => new
                {
                    user_email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    app_type = table.Column<int>(type: "int", nullable: false),
                    registered_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    last_sign = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("mobile_app_install_pkey", x => new { x.user_email, x.app_type });
                });

            migrationBuilder.CreateTable(
                name: "Regions",
                columns: table => new
                {
                    Region = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConnectionString = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateIndex(
                name: "ip_start",
                table: "dbip_location",
                column: "ip_start");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "dbip_location");

            migrationBuilder.DropTable(
                name: "mobile_app_install");

            migrationBuilder.DropTable(
                name: "Regions");
        }
    }
}
