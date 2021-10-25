using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Files.Core.Migrations.MSSql.FilesDbContextMSSql
{
    public partial class MSSqlFilesDbContext_Init : Microsoft.EntityFrameworkCore.Migrations.Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "files_bunch_objects",
                columns: table => new
                {
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    right_node = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    left_node = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_bunch_objects_pkey", x => new { x.tenant_id, x.right_node });
                });

            migrationBuilder.CreateTable(
                name: "files_file",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false),
                    version = table.Column<int>(type: "int", nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    version_group = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    current_version = table.Column<bool>(type: "bit", nullable: false),
                    folder_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    content_length = table.Column<long>(type: "bigint", nullable: false, defaultValue: 0L),
                    file_status = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<int>(type: "int", nullable: false),
                    create_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    create_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    modified_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    converted_type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    comment = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    changes = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    encrypted = table.Column<bool>(type: "bit", nullable: false),
                    forcesave = table.Column<int>(type: "int", nullable: false),
                    thumb = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_file_pkey", x => new { x.tenant_id, x.id, x.version });
                });

            migrationBuilder.CreateTable(
                name: "files_folder",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    folder_type = table.Column<int>(type: "int", nullable: false),
                    create_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    create_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    modified_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    modified_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    foldersCount = table.Column<int>(type: "int", nullable: false),
                    filesCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files_folder", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files_folder_tree",
                columns: table => new
                {
                    folder_id = table.Column<int>(type: "int", nullable: false),
                    parent_id = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_folder_tree_pkey", x => new { x.parent_id, x.folder_id });
                });

            migrationBuilder.CreateTable(
                name: "files_security",
                columns: table => new
                {
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    entry_id = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    entry_type = table.Column<int>(type: "int", nullable: false),
                    subject = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    owner = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: false),
                    security = table.Column<int>(type: "int", nullable: false),
                    timestamp = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_security_pkey", x => new { x.tenant_id, x.entry_id, x.entry_type, x.subject });
                });

            migrationBuilder.CreateTable(
                name: "files_tag",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    owner = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    flag = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files_tag", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files_tag_link",
                columns: table => new
                {
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    tag_id = table.Column<int>(type: "int", nullable: false),
                    entry_type = table.Column<int>(type: "int", nullable: false),
                    entry_id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    create_by = table.Column<Guid>(type: "uniqueidentifier", fixedLength: true, maxLength: 38, nullable: true),
                    create_on = table.Column<DateTime>(type: "datetime2", nullable: true),
                    tag_count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_tag_link_pkey", x => new { x.tenant_id, x.tag_id, x.entry_id, x.entry_type });
                });

            migrationBuilder.CreateTable(
                name: "files_thirdparty_account",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    provider = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    customer_title = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    user_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    folder_type = table.Column<int>(type: "int", nullable: false),
                    create_on = table.Column<DateTime>(type: "datetime2", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    tenant_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_files_thirdparty_account", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "files_thirdparty_app",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", maxLength: 38, nullable: false),
                    app = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    modified_on = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_thirdparty_app_pkey", x => new { x.user_id, x.app });
                });

            migrationBuilder.CreateTable(
                name: "files_thirdparty_id_mapping",
                columns: table => new
                {
                    hash_id = table.Column<string>(type: "nchar(32)", fixedLength: true, maxLength: 32, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    tenant_id = table.Column<int>(type: "int", nullable: false),
                    id = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("files_thirdparty_id_mapping_pkey", x => x.hash_id);
                });

            migrationBuilder.CreateIndex(
                name: "left_node",
                table: "files_bunch_objects",
                column: "left_node");

            migrationBuilder.CreateIndex(
                name: "folder_id",
                table: "files_file",
                column: "folder_id");

            migrationBuilder.CreateIndex(
                name: "id",
                table: "files_file",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "modified_on_files_file",
                table: "files_file",
                column: "modified_on");

            migrationBuilder.CreateIndex(
                name: "modified_on_files_folder",
                table: "files_folder",
                column: "modified_on");

            migrationBuilder.CreateIndex(
                name: "parent_id",
                table: "files_folder",
                columns: new[] { "tenant_id", "parent_id" });

            migrationBuilder.CreateIndex(
                name: "folder_id_files_folder_tree",
                table: "files_folder_tree",
                column: "folder_id");

            migrationBuilder.CreateIndex(
                name: "owner",
                table: "files_security",
                column: "owner");

            migrationBuilder.CreateIndex(
                name: "tenant_id_files_security",
                table: "files_security",
                columns: new[] { "tenant_id", "entry_type", "entry_id", "owner" });

            migrationBuilder.CreateIndex(
                name: "name_files_tag",
                table: "files_tag",
                columns: new[] { "tenant_id", "owner", "name", "flag" });

            migrationBuilder.CreateIndex(
                name: "create_on_files_tag_link",
                table: "files_tag_link",
                column: "create_on");

            migrationBuilder.CreateIndex(
                name: "entry_id",
                table: "files_tag_link",
                columns: new[] { "tenant_id", "entry_id", "entry_type" });

            migrationBuilder.CreateIndex(
                name: "index_1",
                table: "files_thirdparty_id_mapping",
                columns: new[] { "tenant_id", "hash_id" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "files_bunch_objects");

            migrationBuilder.DropTable(
                name: "files_file");

            migrationBuilder.DropTable(
                name: "files_folder");

            migrationBuilder.DropTable(
                name: "files_folder_tree");

            migrationBuilder.DropTable(
                name: "files_security");

            migrationBuilder.DropTable(
                name: "files_tag");

            migrationBuilder.DropTable(
                name: "files_tag_link");

            migrationBuilder.DropTable(
                name: "files_thirdparty_account");

            migrationBuilder.DropTable(
                name: "files_thirdparty_app");

            migrationBuilder.DropTable(
                name: "files_thirdparty_id_mapping");
        }
    }
}
