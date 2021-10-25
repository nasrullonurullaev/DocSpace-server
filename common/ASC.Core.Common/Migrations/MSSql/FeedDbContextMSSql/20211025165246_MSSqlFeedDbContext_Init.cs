using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.FeedDbContextMSSql
{
    public partial class MSSqlFeedDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "feed_aggregate",
                columns: table => new
                {
                    id = table.Column<string>(type: "nvarchar(88)", maxLength: 88, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    tenant = table.Column<int>(type: "int", nullable: false),
                    product = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    author = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    group_id = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    aggregated_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    json = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    keywords = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feed_aggregate", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "feed_last",
                columns: table => new
                {
                    last_key = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    last_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("feed_last_pkey", x => x.last_key);
                });

            migrationBuilder.CreateTable(
                name: "feed_readed",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    module = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("feed_readed_pkey", x => new { x.tenant_id, x.user_id, x.module });
                });

            migrationBuilder.CreateTable(
                name: "feed_users",
                columns: table => new
                {
                    feed_id = table.Column<string>(type: "nvarchar(88)", maxLength: 88, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("feed_users_pkey", x => new { x.feed_id, x.user_id });
                });

            migrationBuilder.CreateIndex(
                name: "aggregated_date",
                table: "feed_aggregate",
                columns: new[] { "tenant", "aggregated_date" });

            migrationBuilder.CreateIndex(
                name: "modified_date",
                table: "feed_aggregate",
                columns: new[] { "tenant", "modified_date" });

            migrationBuilder.CreateIndex(
                name: "product",
                table: "feed_aggregate",
                columns: new[] { "tenant", "product" });

            migrationBuilder.CreateIndex(
                name: "user_id_feed_users",
                table: "feed_users",
                column: "user_id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "feed_aggregate");

            migrationBuilder.DropTable(
                name: "feed_last");

            migrationBuilder.DropTable(
                name: "feed_readed");

            migrationBuilder.DropTable(
                name: "feed_users");
        }
    }
}
