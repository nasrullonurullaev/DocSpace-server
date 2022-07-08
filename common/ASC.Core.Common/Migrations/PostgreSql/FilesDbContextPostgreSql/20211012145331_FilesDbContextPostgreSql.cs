﻿// (c) Copyright Ascensio System SIA 2010-2022
//
// This program is a free software product.
// You can redistribute it and/or modify it under the terms
// of the GNU Affero General Public License (AGPL) version 3 as published by the Free Software
// Foundation. In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended
// to the effect that Ascensio System SIA expressly excludes the warranty of non-infringement of
// any third-party rights.
//
// This program is distributed WITHOUT ANY WARRANTY, without even the implied warranty
// of MERCHANTABILITY or FITNESS FOR A PARTICULAR  PURPOSE. For details, see
// the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
//
// You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
//
// The  interactive user interfaces in modified source and object code versions of the Program must
// display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
//
// Pursuant to Section 7(b) of the License you must retain the original Product logo when
// distributing the program. Pursuant to Section 7(e) we decline to grant you any rights under
// trademark law for use of our trademarks.
//
// All the Product's GUI elements, including illustrations and icon sets, as well as technical writing
// content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0
// International. See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode

namespace ASC.Core.Common.Migrations.PostgreSql.FilesDbContextPostgreSql;

public partial class FilesDbContextPostgreSql : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AlterDatabase()
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.CreateTable(
            name: "files_converts",
            columns: table => new
            {
                input = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_general_ci")
                    .Annotation("MySql:CharSet", "utf8"),
                output = table.Column<string>(type: "varchar(50)", nullable: false, collation: "utf8_general_ci")
                    .Annotation("MySql:CharSet", "utf8")
            },
            constraints: table =>
            {
                table.PrimaryKey("PRIMARY", x => new { x.input, x.output });
            })
            .Annotation("MySql:CharSet", "utf8mb4");

        migrationBuilder.InsertData(
            table: "files_converts",
            columns: new[] { "input", "output" },
            values: new object[,]
            {
                                    {".csv", ".ods"},
                {".csv", ".ots"},
                {".csv", ".pdf"},
                {".csv", ".xlsm"},
                {".csv", ".xlsx"},
                {".csv", ".xltm"},
                {".csv", ".xltx"},
                {".doc", ".docm"},
                {".doc", ".docx"},
                {".doc", ".dotm"},
                {".doc", ".dotx"},
                {".doc", ".epub"},
                {".doc", ".fb2"},
                {".doc", ".html"},
                {".doc", ".odt"},
                {".doc", ".ott"},
                {".doc", ".pdf"},
                {".doc", ".rtf"},
                {".doc", ".txt"},
                {".docm", ".docx"},
                {".docm", ".dotm"},
                {".docm", ".dotx"},
                {".docm", ".epub"},
                {".docm", ".fb2"},
                {".docm", ".html"},
                {".docm", ".odt"},
                {".docm", ".ott"},
                {".docm", ".pdf"},
                {".docm", ".rtf"},
                {".docm", ".txt"},
                {".doct", ".docx"},
                {".docx", ".docm"},
                {".docx", ".docxf"},
                {".docx", ".dotm"},
                {".docx", ".dotx"},
                {".docx", ".epub"},
                {".docx", ".fb2"},
                {".docx", ".html"},
                {".docx", ".odt"},
                {".docx", ".ott"},
                {".docx", ".pdf"},
                {".docx", ".rtf"},
                {".docx", ".txt"},
                {".docxf", ".docx"},
                {".docxf", ".dotx"},
                {".docxf", ".epub"},
                {".docxf", ".fb2"},
                {".docxf", ".html"},
                {".docxf", ".odt"},
                {".docxf", ".oform"},
                {".docxf", ".ott"},
                {".docxf", ".pdf"},
                {".docxf", ".rtf"},
                {".docxf", ".txt"},
                {".dot", ".docm"},
                {".dot", ".docx"},
                {".dot", ".dotm"},
                {".dot", ".dotx"},
                {".dot", ".epub"},
                {".dot", ".fb2"},
                {".dot", ".html"},
                {".dot", ".odt"},
                {".dot", ".ott"},
                {".dot", ".pdf"},
                {".dot", ".rtf"},
                {".dot", ".txt"},
                {".dotm", ".docm"},
                {".dotm", ".docx"},
                {".dotm", ".dotx"},
                {".dotm", ".epub"},
                {".dotm", ".fb2"},
                {".dotm", ".html"},
                {".dotm", ".odt"},
                {".dotm", ".ott"},
                {".dotm", ".pdf"},
                {".dotm", ".rtf"},
                {".dotm", ".txt"},
                {".dotx", ".docm"},
                {".dotx", ".docx"},
                {".dotx", ".dotm"},
                {".dotx", ".epub"},
                {".dotx", ".fb2"},
                {".dotx", ".html"},
                {".dotx", ".odt"},
                {".dotx", ".ott"},
                {".dotx", ".pdf"},
                {".dotx", ".rtf"},
                {".dotx", ".txt"},
                {".epub", ".docm"},
                {".epub", ".docx"},
                {".epub", ".dotm"},
                {".epub", ".dotx"},
                {".epub", ".fb2"},
                {".epub", ".html"},
                {".epub", ".odt"},
                {".epub", ".ott"},
                {".epub", ".pdf"},
                {".epub", ".rtf"},
                {".epub", ".txt"},
                {".fb2", ".docm"},
                {".fb2", ".docx"},
                {".fb2", ".dotm"},
                {".fb2", ".dotx"},
                {".fb2", ".epub"},
                {".fb2", ".html"},
                {".fb2", ".odt"},
                {".fb2", ".ott"},
                {".fb2", ".pdf"},
                {".fb2", ".rtf"},
                {".fb2", ".txt"},
                {".fodp", ".odp"},
                {".fodp", ".otp"},
                {".fodp", ".pdf"},
                {".fodp", ".potm"},
                {".fodp", ".potx"},
                {".fodp", ".pptm"},
                {".fodp", ".pptx"},
                {".fods", ".csv"},
                {".fods", ".ods"},
                {".fods", ".ots"},
                {".fods", ".pdf"},
                {".fods", ".xlsm"},
                {".fods", ".xlsx"},
                {".fods", ".xltm"},
                {".fods", ".xltx"},
                {".fodt", ".docm"},
                {".fodt", ".docx"},
                {".fodt", ".dotm"},
                {".fodt", ".dotx"},
                {".fodt", ".epub"},
                {".fodt", ".fb2"},
                {".fodt", ".html"},
                {".fodt", ".odt"},
                {".fodt", ".ott"},
                {".fodt", ".pdf"},
                {".fodt", ".rtf"},
                {".fodt", ".txt"},
                {".html", ".docm"},
                {".html", ".docx"},
                {".html", ".dotm"},
                {".html", ".dotx"},
                {".html", ".epub"},
                {".html", ".fb2"},
                {".html", ".odt"},
                {".html", ".ott"},
                {".html", ".pdf"},
                {".html", ".rtf"},
                {".html", ".txt"},
                {".mht", ".docm"},
                {".mht", ".docx"},
                {".mht", ".dotm"},
                {".mht", ".dotx"},
                {".mht", ".epub"},
                {".mht", ".fb2"},
                {".mht", ".odt"},
                {".mht", ".ott"},
                {".mht", ".pdf"},
                {".mht", ".rtf"},
                {".mht", ".txt"},
                {".odp", ".otp"},
                {".odp", ".pdf"},
                {".odp", ".potm"},
                {".odp", ".potx"},
                {".odp", ".pptm"},
                {".odp", ".pptx"},
                {".otp", ".odp"},
                {".otp", ".pdf"},
                {".otp", ".potm"},
                {".otp", ".potx"},
                {".otp", ".pptm"},
                {".otp", ".pptx"},
                {".ods", ".csv"},
                {".ods", ".ots"},
                {".ods", ".pdf"},
                {".ods", ".xlsm"},
                {".ods", ".xlsx"},
                {".ods", ".xltm"},
                {".ods", ".xltx"},
                {".ots", ".csv"},
                {".ots", ".ods"},
                {".ots", ".pdf"},
                {".ots", ".xlsm"},
                {".ots", ".xlsx"},
                {".ots", ".xltm"},
                {".ots", ".xltx"},
                {".odt", ".docm"},
                {".odt", ".docx"},
                {".odt", ".dotm"},
                {".odt", ".dotx"},
                {".odt", ".epub"},
                {".odt", ".fb2"},
                {".odt", ".html"},
                {".odt", ".ott"},
                {".odt", ".pdf"},
                {".odt", ".rtf"},
                {".odt", ".txt"},
                {".ott", ".docm"},
                {".ott", ".docx"},
                {".ott", ".dotm"},
                {".ott", ".dotx"},
                {".ott", ".epub"},
                {".ott", ".fb2"},
                {".ott", ".html"},
                {".ott", ".odt"},
                {".ott", ".pdf"},
                {".ott", ".rtf"},
                {".ott", ".txt"},
                {".oxps", ".pdf"},
                {".pot", ".odp"},
                {".pot", ".otp"},
                {".pot", ".pdf"},
                {".pot", ".pptm"},
                {".pot", ".pptx"},
                {".pot", ".potm"},
                {".pot", ".potx"},
                {".potm", ".odp"},
                {".potm", ".otp"},
                {".potm", ".pdf"},
                {".potm", ".potx"},
                {".potm", ".pptm"},
                {".potm", ".pptx"},
                {".potx", ".odp"},
                {".potx", ".otp"},
                {".potx", ".pdf"},
                {".potx", ".potm"},
                {".potx", ".pptm"},
                {".potx", ".pptx"},
                {".pps", ".odp"},
                {".pps", ".otp"},
                {".pps", ".pdf"},
                {".pps", ".potm"},
                {".pps", ".potx"},
                {".pps", ".pptm"},
                {".pps", ".pptx"},
                {".ppsm", ".odp"},
                {".ppsm", ".otp"},
                {".ppsm", ".pdf"},
                {".ppsm", ".potm"},
                {".ppsm", ".potx"},
                {".ppsm", ".pptm"},
                {".ppsm", ".pptx"},
                {".ppsx", ".odp"},
                {".ppsx", ".otp"},
                {".ppsx", ".pdf"},
                {".ppsx", ".potm"},
                {".ppsx", ".potx"},
                {".ppsx", ".pptm"},
                {".ppsx", ".pptx"},
                {".ppt", ".odp"},
                {".ppt", ".otp"},
                {".ppt", ".pdf"},
                {".ppt", ".potm"},
                {".ppt", ".potx"},
                {".ppt", ".pptm"},
                {".ppt", ".pptx"},
                {".pptm", ".odp"},
                {".pptm", ".otp"},
                {".pptm", ".pdf"},
                {".pptm", ".potm"},
                {".pptm", ".potx"},
                {".pptm", ".pptx"},
                {".pptt", ".pptx"},
                {".pptx", ".odp"},
                {".pptx", ".otp"},
                {".pptx", ".pdf"},
                {".pptx", ".potm"},
                {".pptx", ".potx"},
                {".pptx", ".pptm"},
                {".rtf", ".docm"},
                {".rtf", ".docx"},
                {".rtf", ".dotm"},
                {".rtf", ".dotx"},
                {".rtf", ".epub"},
                {".rtf", ".fb2"},
                {".rtf", ".html"},
                {".rtf", ".odt"},
                {".rtf", ".ott"},
                {".rtf", ".pdf"},
                {".rtf", ".txt"},
                {".txt", ".docm"},
                {".txt", ".docx"},
                {".txt", ".dotm"},
                {".txt", ".dotx"},
                {".txt", ".epub"},
                {".txt", ".fb2"},
                {".txt", ".html"},
                {".txt", ".odt"},
                {".txt", ".ott"},
                {".txt", ".pdf"},
                {".txt", ".rtf"},
                {".xls", ".csv"},
                {".xls", ".ods"},
                {".xls", ".ots"},
                {".xls", ".pdf"},
                {".xls", ".xlsm"},
                {".xls", ".xlsx"},
                {".xls", ".xltm"},
                {".xls", ".xltx"},
                {".xlsm", ".csv"},
                {".xlsm", ".ods"},
                {".xlsm", ".ots"},
                {".xlsm", ".pdf"},
                {".xlsm", ".xlsx"},
                {".xlsm", ".xltm"},
                {".xlsm", ".xltx"},
                {".xlst", ".xlsx"},
                {".xlsx", ".csv"},
                {".xlsx", ".ods"},
                {".xlsx", ".ots"},
                {".xlsx", ".pdf"},
                {".xlsx", ".xlsm"},
                {".xlsx", ".xltm"},
                {".xlsx", ".xltx"},
                {".xlt", ".csv"},
                {".xlt", ".ods"},
                {".xlt", ".ots"},
                {".xlt", ".pdf"},
                {".xlt", ".xlsm"},
                {".xlt", ".xlsx"},
                {".xlt", ".xltm"},
                {".xlt", ".xltx"},
                {".xltm", ".csv"},
                {".xltm", ".ods"},
                {".xltm", ".ots"},
                {".xltm", ".pdf"},
                {".xltm", ".xlsm"},
                {".xltm", ".xlsx"},
                {".xltm", ".xltx"},
                {".xltx", ".csv"},
                {".xltx", ".ods"},
                {".xltx", ".ots"},
                {".xltx", ".pdf"},
                {".xltx", ".xlsm"},
                {".xltx", ".xlsx"},
                {".xltx", ".xltm"},
                {".xml", ".docm"},
                {".xml", ".docx"},
                {".xml", ".dotm"},
                {".xml", ".dotx"},
                {".xml", ".epub"},
                {".xml", ".fb2"},
                {".xml", ".html"},
                {".xml", ".odt"},
                {".xml", ".ott"},
                {".xml", ".pdf"},
                {".xml", ".rtf"},
                {".xml", ".txt"},
                {".xps", ".pdf"}
            });
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "files_converts");
    }
}
