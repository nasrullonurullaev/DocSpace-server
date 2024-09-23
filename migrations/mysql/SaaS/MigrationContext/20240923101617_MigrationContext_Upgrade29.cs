using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASC.Migrations.MySql.SaaS.Migrations
{
    /// <inheritdoc />
    public partial class MigrationContext_Upgrade29 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "core_acl",
                columns: new[] { "action", "object", "subject", "tenant", "acetype" },
                values: new object[] { "3e74aff2-7c0c-4089-b209-6495b8643471", "", "88f11e7c-7407-4bea-b4cb-070010cdbb6b", -1, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "core_acl",
                keyColumns: new[] { "action", "object", "subject", "tenant" },
                keyValues: new object[] { "3e74aff2-7c0c-4089-b209-6495b8643471", "", "88f11e7c-7407-4bea-b4cb-070010cdbb6b", -1 });
        }
    }
}
