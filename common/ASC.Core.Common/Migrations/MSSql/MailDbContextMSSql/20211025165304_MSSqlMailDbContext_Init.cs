using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASC.Core.Common.Migrations.MSSql.MailDbContextMSSql
{
    public partial class MSSqlMailDbContext_Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "api_keys",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    access_token = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_api_keys", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "greylisting_whitelist",
                columns: table => new
                {
                    comment = table.Column<string>(type: "nvarchar(450)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    source = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("comment_pk", x => x.comment);
                });

            migrationBuilder.CreateTable(
                name: "mail_mailbox",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    tenant = table.Column<int>(type: "int", nullable: false),
                    id_user = table.Column<string>(type: "nvarchar(38)", maxLength: 38, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    enabled = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    is_removed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_processed = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_server_mailbox = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsTeamlabMailbox = table.Column<bool>(type: "bit", nullable: false),
                    imap = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    user_online = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    is_default = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    msg_count_last = table.Column<int>(type: "int", nullable: false),
                    size_last = table.Column<int>(type: "int", nullable: false),
                    login_delay = table.Column<int>(type: "int", nullable: false, defaultValue: 30),
                    quota_error = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    imap_intervals = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    begin_date = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "'1975-01-01 00:00:00'"),
                    email_in_folder = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    pop3_password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    smtp_password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    token_type = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    token = table.Column<string>(type: "nvarchar(max)", nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    id_smtp_server = table.Column<int>(type: "int", nullable: false),
                    id_in_server = table.Column<int>(type: "int", nullable: false),
                    date_checked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_user_checked = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_login_delay_expires = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "'1975-01-01 00:00:00'"),
                    date_auth_error = table.Column<DateTime>(type: "datetime2", nullable: true),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_modified = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mail_mailbox", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mail_mailbox_provider",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    display_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    display_short_name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    documentation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mail_mailbox_provider", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "mail_mailbox_server",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    id_provider = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    hostname = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    port = table.Column<int>(type: "int", nullable: false),
                    socket_type = table.Column<string>(type: "nvarchar(max)", nullable: false, defaultValue: "plain", collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    username = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    authentication = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    is_user_data = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mail_mailbox_server", x => x.id);
                    table.CheckConstraint("constraint_type", "[type] = 'pop3' or [type] = 'imap' or [type] = 'smtp'");
                });

            migrationBuilder.CreateTable(
                name: "mail_server_server",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    mx_record = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false, defaultValue: " ", collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    connection_string = table.Column<string>(type: "nvarchar(max)", nullable: false, collation: "LATIN1_GENERAL_100_CI_AS_SC_UTF8"),
                    server_type = table.Column<int>(type: "int", nullable: false),
                    smtp_settings_id = table.Column<int>(type: "int", nullable: false),
                    imap_settings_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mail_server_server", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_provider",
                columns: new[] { "id", "display_name", "display_short_name", "documentation", "name" },
                values: new object[,]
                {
                    { 1, "1&1", "1&1", "http://hilfe-center.1und1.de/access/search/go.php?t=e698123", "1und1.de" },
                    { 141, "??????????", "INET-SHIBATA", null, "pop.shibata.ne.jp" },
                    { 142, "Posteo", "Posteo", null, "posteo.de" },
                    { 143, "???", "???", null, "purple.plala.or.jp" },
                    { 144, "qip.ru", "qip.ru", null, "qip.ru" },
                    { 145, "???", "???", null, "rainbow.plala.or.jp" },
                    { 146, "Rambler Mail", "Rambler", null, "rambler.ru" },
                    { 147, "???", "???", null, "red.plala.or.jp" },
                    { 148, "???", "???", null, "rmail.plala.or.jp" },
                    { 149, "???", "???", null, "rondo.plala.or.jp" },
                    { 150, "???", "???", null, "rose.plala.or.jp" },
                    { 151, "???", "???", null, "rouge.plala.or.jp" },
                    { 152, "RoadRunner", "RR", "http://help.rr.com/HMSFaqs/e_emailserveraddys.aspx", "rr.com" },
                    { 153, "???", "???", null, "ruby.plala.or.jp" },
                    { 154, "?????????", "Saku-Net", null, "sakunet.ne.jp" },
                    { 155, "???", "???", null, "sea.plala.or.jp" },
                    { 156, "???", "???", null, "sepia.plala.or.jp" },
                    { 157, "???", "???", null, "serenade.plala.or.jp" },
                    { 158, "Seznam", "Seznam", "http://napoveda.seznam.cz/cz/jake-jsou-adresy-pop3-a-smtp-serveru.html", "seznam.cz" },
                    { 159, "SFR / Neuf", "SFR", "http://assistance.sfr.fr/internet_neufbox-de-SFR/utiliser-email/parametrer-id-sfr/fc-505-50680", "sfr.fr" },
                    { 160, "???", "???", null, "silk.plala.or.jp" },
                    { 161, "???", "???", null, "silver.plala.or.jp" },
                    { 162, "???", "???", null, "sky.plala.or.jp" },
                    { 163, "skynet", "skynet", "http://support.en.belgacom.be/app/answers/detail/a_id/14337/kw/thunderbird", "skynet.be" },
                    { 140, "???", "???", null, "polka.plala.or.jp" },
                    { 139, "'?????????", "wind", null, "po.wind.ne.jp" },
                    { 138, "DCN???????????", "DCN", null, "po.dcn.ne.jp" },
                    { 137, "???", "???", null, "plum.plala.or.jp" },
                    { 113, "Apple iCloud", "Apple", null, "me.com" },
                    { 114, "???", "???", null, "minuet.plala.or.jp" },
                    { 115, "???????????", "IWAFUNE", null, "ml.murakami.ne.jp" },
                    { 116, "Mnet ??? ????", "Mnet???", null, "mnet.ne.jp" },
                    { 117, "mopera U", "mopera U", null, "'mopera.net" },
                    { 118, "Mozilla Corporation and Mozilla Foundation internal email addresses", "mozilla.com", null, "mozilla.com" },
                    { 119, "TikiTiki???????", "TikiTiki", null, "mx1.tiki.ne.jp" },
                    { 120, "???", "???", null, "navy.plala.or.jp" },
                    { 121, "nctsoft", "nct", null, "nctsoft.com" },
                    { 122, "@nifty", "@nifty", null, "nifty.com" },
                    { 123, "BB????", "NSAT", null, "nsat.jp" },
                    { 164, "???", "???", null, "smail.plala.or.jp" },
                    { 124, "o2 Poczta", "o2", null, "o2.pl" },
                    { 126, "Poczta Onet", "Onet", null, "onet.pl" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_provider",
                columns: new[] { "id", "display_name", "display_short_name", "documentation", "name" },
                values: new object[,]
                {
                    { 127, "???", "???", null, "opal.plala.or.jp" },
                    { 128, "???", "???", null, "orange.plala.or.jp" },
                    { 129, "???", "???", null, "orchid.plala.or.jp" },
                    { 130, "OVH", "OVH", "http://guides.ovh.com/ConfigurationEmail", "ovh.net" },
                    { 131, "????FTTH", "????FTTH", null, "pal.kijimadaira.jp" },
                    { 132, "???", "???", null, "palette.plala.or.jp" },
                    { 133, "??????", "PARABOX", null, "parabox.or.jp" },
                    { 134, "Portland State University Mail", "PSU Mail", null, "pdx.edu" },
                    { 135, "???", "???", null, "peach.plala.or.jp" },
                    { 136, "PeoplePC", "PeoplePC", null, "peoplepc.com" },
                    { 125, "???", "???", null, "olive.plala.or.jp" },
                    { 165, "???", "???", null, "snow.plala.or.jp" },
                    { 166, "?????????", "wind", null, "so.wind.ne.jp" },
                    { 167, "???", "???", null, "sonata.plala.or.jp" },
                    { 197, "???", "???", null, "wine.plala.or.jp" },
                    { 198, "???", "???", null, "wmail.plala.or.jp" },
                    { 199, "Poczta Wirtualna Polska", "Poczta WP", null, "wp.pl" },
                    { 200, "???", "???", null, "xmail.plala.or.jp" },
                    { 201, "?????????", "wind", null, "xp.wind.jp" },
                    { 202, "???", "???", null, "xpost.plala.or.jp" },
                    { 203, "XS4All", "XS4All", null, "xs4all.nl" },
                    { 204, "Yahoo! Mail", "Yahoo", null, "xtra.co.nz" },
                    { 205, "Yahoo! ???", "Yahoo! ??? ", null, "yahoo.co.jp" },
                    { 206, "Yahoo! Mail", "Yahoo", null, "yahoo.com" },
                    { 207, "Yandex Mail", "Yandex", null, "yandex.ru" },
                    { 196, "Your WildWest domain", "WildWest", null, "wildwestdomains.com" },
                    { 208, "Yahoo! BB", "Yahoo! BB", null, "ybb.ne.jp" },
                    { 210, "???", "???", null, "ymail.plala.or.jp" },
                    { 211, "???", "???", null, "ypost.plala.or.jp" },
                    { 212, "Ziggo", "Ziggo", null, "ziggo.nl" },
                    { 213, "???", "???", null, "zmail.plala.or.jp" },
                    { 214, "???", "???", null, "zpost.plala.or.jp" },
                    { 215, "avsmedia.com", "avsmedia", null, "avsmedia.com" },
                    { 216, "avsmedia.net", "avsmedia", null, "avsmedia.net" },
                    { 218, "ilearney.com", "ilearney.com", null, "ilearney.com" },
                    { 219, "fpl-technology.com", "fpl-technology.com", "http://fpl-technology.com", "fpl -technology.com" },
                    { 220, "Apple iCloud", "Apple", null, "icloud.com" },
                    { 221, "Microsoft Office 365", "Office365", "https://products.office.com", "office365.com" },
                    { 209, "???", "???", null, "yellow.plala.or.jp" },
                    { 112, "???", "???", null, "maroon.plala.or.jp" },
                    { 195, "???", "???", null, "white.plala.or.jp" },
                    { 193, "???", "???", null, "wave.plala.or.jp" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_provider",
                columns: new[] { "id", "display_name", "display_short_name", "documentation", "name" },
                values: new object[,]
                {
                    { 168, "Strato", "Strato", null, "strato.de" },
                    { 169, "Universita degli Studi di Verona", "UniVR", null, "studenti.univr.it" },
                    { 170, "???", "???", null, "suite.plala.or.jp" },
                    { 171, "Sympatico Email", "Sympatico", "http://internet.bell.ca/index.cfm?method=content.view&category_id=585&content_id=12767", "sympatico.ca" },
                    { 173, "T-Online", "T-Online", null, "t-online.de" },
                    { 174, "???", "???", null, "taupe.plala.or.jp" },
                    { 175, "Correo Terra", "Terra", null, "terra.es" },
                    { 176, "TikiTiki???????", "TikiTiki", null, "tiki.ne.jp" },
                    { 177, "Tiscali", "Tiscali", null, "tiscali.cz" },
                    { 178, "Tiscali Italy", "Tiscali", "http://assistenza.tiscali.it/tecnica/posta/configurazioni/", "tiscali.it" },
                    { 179, "???", "???", null, "tmail.plala.or.jp" },
                    { 194, "WEB.DE Freemail", "Web.de", "http://hilfe.freemail.web.de/freemail/e-mail/pop3/thunderbird/", "web.de" },
                    { 180, "???", "???", null, "toccata.plala.or.jp" },
                    { 182, "???", "???", null, "trio.plala.or.jp" },
                    { 183, "???", "???", null, "umail.plala.or.jp" },
                    { 184, "UM ITCS Email", "UM ITCS", null, "umich.edu" },
                    { 185, "UPC Nederland", "UPC", null, "upcmail.nl" },
                    { 186, "Verizon Online", "Verizon", null, "verizon.net" },
                    { 187, "Versatel", "Versatel", "http://www.versatel.de/hilfe/index_popup.php?einrichtung_email_programm", "versatel.de" },
                    { 188, "???", "???", null, "violet.plala.or.jp" },
                    { 189, "aikis", "aikis", null, "vm.aikis.or.jp" },
                    { 190, "???", "???", null, "vmail.plala.or.jp" },
                    { 191, "TikiTiki???????", "TikiTiki", null, "vp.tiki.ne.jp" },
                    { 192, "???", "???", null, "waltz.plala.or.jp" },
                    { 181, "???", "???", null, "topaz.plala.or.jp" },
                    { 111, "?????????", "???", null, "mail.wind.ne.jp" },
                    { 172, "???", "???", null, "symphony.plala.or.jp" },
                    { 109, "mail.ru", "mail.ru", null, "mail.ru" },
                    { 30, "???????????", "CEK-Net", null, "cek.ne.jp" },
                    { 31, "UCSF CGL email", "CGL emai", null, "cgl.ucsf.edu" },
                    { 32, "Charter Commuications", "Charter", null, "charter.com" },
                    { 33, "CLIO-Net??????", "CLIO-Net", null, "clio.ne.jp" },
                    { 34, "???", "???", null, "cmail.plala.or.jp" },
                    { 35, "?????????", "wind", null, "co1.wind.ne.jp" },
                    { 36, "?????????", "wind", null, "co2.wind.ne.jp" },
                    { 37, "?????????", "wind", null, "co3.wind.ne.jp" },
                    { 38, "???", "???", null, "cocoa.plala.or.jp" },
                    { 39, "???", "Arcor", null, "coda.plala.or.jp" },
                    { 40, "???", "Comcast", null, "comcast.net" },
                    { 41, "???", "???", null, "concerto.plala.or.jp" },
                    { 42, "???", "???", null, "coral.plala.or.jp" },
                    { 43, "???", "???", null, "courante.plala.or.jp" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_provider",
                columns: new[] { "id", "display_name", "display_short_name", "documentation", "name" },
                values: new object[,]
                {
                    { 44, "???", "???", null, "cpost.plala.or.jp" },
                    { 45, "???", "???", null, "cream.plala.or.jp" },
                    { 46, "???", "wind", null, "dan.wind.ne.jp" },
                    { 47, "???", "???", null, "dance.plala.or.jp" },
                    { 48, "IIJ4U", "???", null, "dd.iij4u.or.jp" },
                    { 49, "domainFACTORY", "domainFACTORY", "http://www.df.eu/de/service/df-faq/e-mail/mail-programme/", "df.eu" },
                    { 50, "???", "???", null, "dmail.plala.or.jp" },
                    { 51, "EarthLink", "EarthLink", "http://support.earthlink.net/email/email-server-settings.php", "earthlink.net" },
                    { 52, "???", "???", null, "ebony.plala.or.jp" },
                    { 29, "CC9???????????", "CC9", null, "cc9.ne.jp" },
                    { 28, "???", "???", null, "cameo.plala.or.jp" },
                    { 27, "???", "???", null, "camel.plala.or.jp" },
                    { 25, "???", "???", null, "broba.cc" },
                    { 110, "Telenor Danmark", "Telenor", null, "mail.telenor.dk" },
                    { 2, "???", "???", null, "abc.plala.or.jp" },
                    { 3, "???", "???", null, "agate.plala.or.jp" },
                    { 4, "Alice Italy", "Alice", "http://aiuto.alice.it/informazioni/clientemail/thunderbird.html", "alice.it" },
                    { 5, "???", "???", null, "amail.plala.or.jp" },
                    { 6, "???", "???", null, "amber.plala.or.jp" },
                    { 7, "AOL Mail", "AOL", null, "aol.com" },
                    { 8, "???", "???", null, "apost.plala.or.jp" },
                    { 9, "???", "???", null, "aqua.plala.or.jp" },
                    { 10, "Arcor", "Arcor", null, "arcor.de" },
                    { 11, "Aruba PEC", "Aruba", "http://pec.aruba.it/guide_filmate.asp", "arubapec.it" },
                    { 53, "email.it", "email.it", "http://www.email.it/ita/config/thunder.php", "email.it" },
                    { 12, "AT&T", "AT&T", "http://www.att.com/esupport/article.jsp?sid=KB401570&ct=9000152", "att.net" },
                    { 14, "?????????", "wind", null, "bay.wind.ne.jp" },
                    { 15, "BB????", "BB-NIIGATA", null, "bb-niigata.jp" },
                    { 16, "???", "???", null, "beige.plala.or.jp" },
                    { 17, "Biglobe", "Biglobe", null, "biglobe.ne.jp" },
                    { 18, "Telstra Bigpond", "Bigpond", null, "bigpond.com" },
                    { 19, "???", "???", null, "blue.plala.or.jp" },
                    { 20, "bluewin.ch", "bluewin.ch", "http://smtphelp.bluewin.ch/swisscomdtg/setup/?", "bluemail.ch" },
                    { 21, "bluewin.ch", "bluewin.ch", "http://smtphelp.bluewin.ch/swisscomdtg/setup/", "bluewin.ch" },
                    { 22, "???", "???", null, "bmail.plala.or.jp" },
                    { 23, "???", "???", null, "bolero.plala.or.jp" },
                    { 24, "???", "???", null, "bpost.plala.or.jp" },
                    { 13, "???", "???", null, "ballade.plala.or.jp" },
                    { 54, "???", "???", null, "email.plala.or.jp" },
                    { 26, "???", "???", null, "brown.plala.or.jp" },
                    { 56, "???", "???", null, "fantasy.plala.or.jp" },
                    { 85, "IPAX Internet Services", "IPAX", null, "ipax.at" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_provider",
                columns: new[] { "id", "display_name", "display_short_name", "documentation", "name" },
                values: new object[,]
                {
                    { 86, "???", "???", null, "ivory.plala.or.jp" },
                    { 87, "???????????", "IWAFUNE", null, "iwafune.ne.jp" },
                    { 88, "???", "???", null, "jade.plala.or.jp" },
                    { 89, "Janis", "Janis", null, "janis.or.jp" },
                    { 90, "JETINTERNET", "JET", null, "jet.ne.jp" },
                    { 91, "JETINTERNET", "JET", null, "ji.jet.ne.jp" },
                    { 92, "???", "???", null, "jmail.plala.or.jp" },
                    { 93, "Kabel Deutschland", "Kabel D", null, "kabelmail.de" },
                    { 94, "KELCOM Internet", "KELCOM", null, "kelcom.net" },
                    { 96, "?????????", "wind", null, "kl.wind.ne.jp" },
                    { 84, "Internode", "Internode", "http://www.internode.on.net/support/guides/email/secure_email/", "internode.on.net" },
                    { 97, "???", "???", null, "kmail.plala.or.jp" },
                    { 99, "???", "???", null, "lapis.plala.or.jp" },
                    { 100, "LaPoste", "LaPoste", "http://www.geckozone.org/forum/viewtopic.php?f=4&t=93118", "laposte.net" },
                    { 101, "???", "???", null, "lemon.plala.or.jp" },
                    { 102, "Libero Mail", "Libero", "http://aiuto.libero.it/mail/istruzioni/configura-mozilla-thunderbird-per-windows-a11.phtml", "libero.it" },
                    { 103, "???", "???", null, "lilac.plala.or.jp" },
                    { 104, "???", "???", null, "lime.plala.or.jp" },
                    { 105, "???????????", "????", null, "mahoroba.ne.jp" },
                    { 106, "mail.com", "mail.com", null, "mail.com" },
                    { 107, "TDC (DK)", "TDC", null, "mail.dk" },
                    { 55, "EWE Tel", "EWE Tel", null, "ewetel.de" },
                    { 108, "???????????", "IWAFUNE", null, "mail.iwafune.ne.jp" },
                    { 98, "????????????", "?????", null, "kokuyou.ne.jp" },
                    { 83, "??????????", "INET-SHIBATA", null, "inet-shibata.or.jp" },
                    { 95, "???", "???", null, "khaki.plala.or.jp" },
                    { 81, "Inbox.lv", "Inbox.lv", null, "inbox.lv" },
                    { 57, "???", "???", null, "flamenco.plala.or.jp" },
                    { 58, "???", "???", null, "fmail.plala.or.jp" },
                    { 82, "???", "???", null, "indigo.plala.or.jp" },
                    { 59, "France Telecom / Orange", "Orange", null, "francetelecom.fr" },
                    { 60, "Free Telecom", "free.fr", "http://www.free.fr/assistance/599-thunderbird.html", "free.fr" },
                    { 61, "Freenet Mail", "Freenet", null, "freenet.de" },
                    { 62, "???", "???", null, "fuga.plala.or.jp" },
                    { 63, "Gandi Mail", "Gandi", null, "gandi.net" },
                    { 65, "GMX Freemail", "GMX", null, "gmx.com" },
                    { 66, "GMX Freemail", "GMX", null, "gmx.net" },
                    { 67, "????????????????????", "TVM-Net", null, "go.tvm.ne.jp" },
                    { 68, "goo ????????", "goo", null, "goo.jp" },
                    { 69, "Google Mail", "GMail", null, "googlemail.com" },
                    { 64, "???", "???", null, "gmail.plala.or.jp" },
                    { 70, "???", "???", null, "grape.plala.or.jp" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_provider",
                columns: new[] { "id", "display_name", "display_short_name", "documentation", "name" },
                values: new object[,]
                {
                    { 71, "???", "???", null, "gray.plala.or.jp" },
                    { 72, "?????????", "HAL", null, "hal.ne.jp" },
                    { 73, "????????", "????", null, "hana.or.jp" },
                    { 74, "Microsoft Live Hotmail", "Hotmail", null, "hotmail.com" },
                    { 75, "SoftBank", "SoftBank", null, "i.softbank.jp" },
                    { 76, "IC-NET", "IC-NET", null, "ic-net.or.jp" },
                    { 77, "IIJmio ????????", "IIJmio", null, "iijmio-mail.jp" },
                    { 78, "???????i?????", "i?????", null, "iiyama-catv.ne.jp" },
                    { 79, "???", "???", null, "imail.plala.or.jp" },
                    { 80, "Inbox.lt", "Inbox.lt", null, "inbox.lt" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 825, "", "posteo.de", 142, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 826, "", "posteo.de", 142, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 827, "", "purple.mail.plala.or.jp", 143, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 835, "", "pop.rambler.ru", 146, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 828, "", "purple.mail.plala.or.jp", 143, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 832, "", "rainbow.mail.plala.or.jp", 145, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 830, "", "imap.qip.ru", 144, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 831, "", "smtp.qip.ru", 144, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 834, "", "imap.rambler.ru", 146, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 833, "", "rainbow.mail.plala.or.jp", 145, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 824, "", "%EMAILDOMAIN%", 141, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 829, "", "pop.qip.ru", 144, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 823, "", "%EMAILDOMAIN%", 141, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 812, "", "imap.peoplepc.com", 136, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 821, "", "polka.mail.plala.or.jp", 140, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 820, "", "po.wind.ne.jp", 139, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 819, "", "po.wind.ne.jp", 139, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 818, "none", "po.dcn.ne.jp", 138, 25, "plain", "smtp", "" },
                    { 817, "", "po.dcn.ne.jp", 138, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 816, "", "plum.mail.plala.or.jp", 137, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 815, "", "plum.mail.plala.or.jp", 137, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 814, "", "smtpauth.peoplepc.com", 136, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 813, "", "pop.peoplepc.com", 136, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 811, "", "peach.mail.plala.or.jp", 135, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 809, "password-encrypted", "mailhost.pdx.edu", 134, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 836, "", "smtp.rambler.ru", 146, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 810, "", "peach.mail.plala.or.jp", 135, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 822, "", "polka.mail.plala.or.jp", 140, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 837, "password-cleartext", "red.mail.plala.or.jp", 147, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 863, "", "smtp.sfr.fr", 159, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 839, "password-encrypted", "rmail.mail.plala.or.jp", 148, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 867, "", "silver.mail.plala.or.jp", 161, 587, "plain", "smtp", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 808, "password-encrypted", "psumail.pdx.edu", 134, 587, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 866, "", "silver.mail.plala.or.jp", 161, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 865, "", "silk.mail.plala.or.jp", 160, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 864, "", "silk.mail.plala.or.jp", 160, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 862, "", "pop.sfr.fr", 159, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 861, "", "imap.sfr.fr", 159, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 860, "", "smtp.seznam.cz", 158, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 859, "", "pop3.seznam.cz", 158, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 858, "", "serenade.mail.plala.or.jp", 157, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 857, "", "serenade.mail.plala.or.jp", 157, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 856, "", "sepia.mail.plala.or.jp", 156, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 855, "", "sepia.mail.plala.or.jp", 156, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 838, "password-cleartext", "red.mail.plala.or.jp", 147, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 854, "", "sea.mail.plala.or.jp", 155, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 852, "none", "smtp.sakunet.ne.jp", 154, 25, "plain", "smtp", "" },
                    { 851, "", "mail.sakunet.ne.jp", 154, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 850, "", "ruby.mail.plala.or.jp", 153, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 849, "", "ruby.mail.plala.or.jp", 153, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 848, "", "smtp-server.%EMAILDOMAIN%", 152, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 847, "", "pop-server.%EMAILDOMAIN%", 152, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 846, "password-cleartext", "rouge.mail.plala.or.jp", 151, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 845, "password-cleartext", "rouge.mail.plala.or.jp", 151, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 844, "password-cleartext", "rose.mail.plala.or.jp", 150, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 843, "password-cleartext", "rose.mail.plala.or.jp", 150, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 842, "password-cleartext", "rondo.mail.plala.or.jp", 149, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 841, "", "rondo.mail.plala.or.jp", 149, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 840, "", "rmail.mail.plala.or.jp", 148, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 853, "", "sea.mail.plala.or.jp", 155, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 807, "password-encrypted", "psumail.pdx.edu", 134, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 753, "", "smtp.mail.ru", 109, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 805, "", "pop3.parabox.or.jp", 133, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 772, "", "mail.mopera.net", 117, 110, "STARTTLS", "pop3", "%EMAILLOCALPART%" },
                    { 771, "", "mail.mopera.net", 117, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 770, "", "mail.mopera.net", 117, 143, "STARTTLS", "imap", "%EMAILLOCALPART%" },
                    { 769, "", "mail.mopera.net", 117, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 768, "", "mail.mnet.ne.jp", 116, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 767, "", "mail.mnet.ne.jp", 116, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 766, "", "ml.murakami.ne.jp", 115, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 765, "", "ml.murakami.ne.jp", 115, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 764, "", "minuet.mail.plala.or.jp", 114, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 763, "", "minuet.mail.plala.or.jp", 114, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 762, "", "smtp.mail.me.com", 113, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 761, "", "imap.mail.me.com", 113, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 760, "", "maroon.mail.plala.or.jp", 112, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 759, "", "maroon.mail.plala.or.jp", 112, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 758, "", "mail.wind.ne.jp", 111, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 757, "", "mail.wind.ne.jp", 111, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 756, "", "mail.telenor.dk", 110, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 755, "", "mail.telenor.dk", 110, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 754, "", "mail.telenor.dk", 110, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 752, "", "imap.mail.ru", 109, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 751, "", "pop.mail.ru", 109, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 750, "", "mail.iwafune.ne.jp", 108, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 749, "", "mail.iwafune.ne.jp", 108, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 748, "", "asmtp.mail.dk", 107, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 747, "", "pop3.mail.dk", 107, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 746, "", "smtp.mail.com", 106, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 868, "", "sky.mail.plala.or.jp", 162, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 773, "", "mail.mopera.net", 117, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 774, "", "mail.mozilla.com", 118, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 775, "", "smtp.mozilla.org", 118, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 776, "", "%EMAILDOMAIN%", 119, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 804, "", "palette.mail.plala.or.jp", 132, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 803, "", "palette.mail.plala.or.jp", 132, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 802, "none", "smtp.pal.kijimadaira.jp", 131, 25, "plain", "smtp", "" },
                    { 801, "", "mail.pal.kijimadaira.jp", 131, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 800, "password-cleartext", "ssl0.ovh.net", 130, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 799, "password-cleartext", "ssl0.ovh.net", 130, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 798, "password-cleartext", "ssl0.ovh.net", 130, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 797, "", "orchid.mail.plala.or.jp", 129, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 796, "", "orchid.mail.plala.or.jp", 129, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 795, "", "orange.mail.plala.or.jp", 128, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 794, "", "orange.mail.plala.or.jp", 128, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 793, "", "opal.mail.plala.or.jp", 127, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 792, "", "opal.mail.plala.or.jp", 127, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 806, "", "smtp.parabox.or.jp", 134, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 791, "", "pop3.poczta.onet.pl", 126, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 789, "", "olive.mail.plala.or.jp", 125, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 788, "", "olive.mail.plala.or.jp", 125, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 787, "password-cleartext", "poczta.o2.pl", 124, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 786, "password-cleartext", "poczta.o2.pl", 124, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 785, "", "mail.nsat.jp", 123, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 784, "", "mail.nsat.jp", 123, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 783, "", "smtp.nifty.com", 122, 587, "plain", "smtp", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 782, "", "pop.nifty.com", 122, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 781, "none", "mail.nctsoft.com", 121, 25, "plain", "smtp", "" },
                    { 780, "", "mail.nctsoft.com", 121, 110, "plain", "pop3", "%EMAILLOCALPART%@%EMAILHOSTNAME%" },
                    { 779, "", "navy.mail.plala.or.jp", 120, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 778, "", "navy.mail.plala.or.jp", 120, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 777, "", "smtp-auth.tiki.ne.jp", 119, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 790, "", "pop3.poczta.onet.pl", 126, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 869, "", "sky.mail.plala.or.jp", 162, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 979, "", "ypost.mail.plala.or.jp", 211, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 871, "password-encrypted", "pop.skynet.be", 163, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 962, "", "pop3.xtra.co.nz", 204, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 961, "", "smtps.xs4all.nl", 203, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 960, "", "pops.xs4all.nl", 203, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 959, "", "xpost.mail.plala.or.jp", 202, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 958, "", "xpost.mail.plala.or.jp", 202, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 957, "", "xp.wind.jp", 201, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 956, "", "xp.wind.jp", 201, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 955, "", "xmail.mail.plala.or.jp", 200, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 954, "", "xmail.mail.plala.or.jp", 200, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 953, "", "smtp.wp.pl", 199, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 952, "", "pop3.wp.pl", 199, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 951, "", "wmail.mail.plala.or.jp", 198, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 950, "", "wmail.mail.plala.or.jp", 198, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 949, "", "wine.mail.plala.or.jp", 197, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 948, "", "wine.mail.plala.or.jp", 197, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 947, "password-cleartext", "smtpout.secureserver.net", 196, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 946, "password-cleartext", "imap.secureserver.net", 196, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 945, "password-cleartext", "pop.secureserver.net", 196, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 944, "", "white.mail.plala.or.jp", 195, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 943, "", "white.mail.plala.or.jp", 195, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 942, "", "smtp.web.de", 194, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 941, "", "pop3.web.de", 194, 110, "STARTTLS", "pop3", "%EMAILLOCALPART%" },
                    { 940, "", "pop3.web.de", 194, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 939, "", "imap.web.de", 194, 143, "STARTTLS", "imap", "%EMAILLOCALPART%" },
                    { 938, "", "imap.web.de", 194, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 937, "", "wave.mail.plala.or.jp", 193, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 936, "", "wave.mail.plala.or.jp", 193, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 963, "", "send.xtra.co.nz", 204, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 964, "", "pop.mail.yahoo.co.jp", 204, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 965, "", "smtp.mail.yahoo.co.jp", 205, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 966, "", "pop.mail.yahoo.com", 206, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 996, "", "outlook.office365.com", 221, 995, "SSL", "pop3", "%EMAILADDRESS%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 995, "", "smtp.mail.me.com", 220, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 994, "", "imap.mail.me.com", 220, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 993, "", "imap-mail.outlook.com", 74, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 992, "password-cleartext", "smtps.aruba.it", 219, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 991, "password-cleartext", "imaps.aruba.it", 219, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 990, "password-cleartext", "pop3s.aruba.it", 219, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 989, "none", "mail.ilearney.com", 218, 25, "plain", "smtp", "" },
                    { 988, "", "mail.ilearney.com", 218, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 985, "", "zpost.mail.plala.or.jp", 214, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 984, "", "zpost.mail.plala.or.jp", 214, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 983, "", "zmail.mail.plala.or.jp", 213, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 982, "", "zmail.mail.plala.or.jp", 213, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 935, "", "waltz.mail.plala.or.jp", 192, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 981, "", "smtp.ziggo.nl", 212, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 745, "", "pop.mail.com", 106, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 978, "", "ypost.mail.plala.or.jp", 211, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 977, "", "ymail.mail.plala.or.jp", 210, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 976, "", "ymail.mail.plala.or.jp", 210, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 975, "", "yellow.mail.plala.or.jp", 209, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 974, "", "yellow.mail.plala.or.jp", 209, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 973, "", "ybbsmtp.mail.yahoo.co.jp", 208, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 972, "", "ybbpop.mail.yahoo.co.jp", 208, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 971, "", "smtp.yandex.ru", 207, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 970, "", "pop.yandex.ru", 207, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 969, "", "imap.yandex.ru", 207, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 968, "", "smtp.mail.yahoo.com", 206, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 967, "", "imap.mail.yahoo.com", 206, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 980, "", "pop.ziggo.nl", 212, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 934, "", "waltz.mail.plala.or.jp", 192, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 933, "", "vs.tiki.ne.jp", 191, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 932, "", "vp.tiki.ne.jp", 191, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 899, "", "mailhost.terra.es", 175, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 898, "", "pop3.terra.es", 175, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 897, "", "imap4.terra.es", 175, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 896, "", "taupe.mail.plala.or.jp", 174, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 895, "", "taupe.mail.plala.or.jp", 174, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 894, "", "securesmtp.t-online.de", 173, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 893, "", "securepop.t-online.de", 173, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 892, "", "secureimap.t-online.de", 173, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 891, "", "symphony.mail.plala.or.jp", 172, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 890, "", "symphony.mail.plala.or.jp", 172, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 889, "", "smtphm.sympatico.ca", 171, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 888, "", "pophm.sympatico.ca", 171, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 887, "", "suite.mail.plala.or.jp", 170, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 900, "", "mx.tiki.ne.jp", 176, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 886, "", "suite.mail.plala.or.jp", 170, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 884, "", "mail.studenti.univr.it", 169, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 883, "password-encrypted", "smtp.strato.de", 168, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 882, "password-encrypted", "pop3.strato.de", 168, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 881, "password-encrypted", "imap.strato.de", 168, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 880, "", "sonata.mail.plala.or.jp", 167, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 879, "", "sonata.mail.plala.or.jp", 167, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 878, "", "so.wind.ne.jp", 166, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 877, "", "so.wind.ne.jp", 166, 143, "plain", "imap", "%EMAILLOCALPART%" },
                    { 876, "", "snow.mail.plala.or.jp", 165, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 875, "", "snow.mail.plala.or.jp", 165, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 874, "", "smail.mail.plala.or.jp", 164, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 873, "", "smail.mail.plala.or.jp", 164, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 872, "password-cleartext", "relay.skynet.be", 163, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 885, "", "mail.studenti.univr.it", 169, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 870, "password-cleartext", "imap.skynet.be", 163, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 901, "", "smtp-auth.tiki.ne.jp", 176, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 903, "", "smtp.mail.tiscali.cz", 177, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 931, "", "vmail.mail.plala.or.jp", 190, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 930, "", "vmail.mail.plala.or.jp", 190, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 929, "", "mail.aikis.or.jp", 189, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 928, "", "mail.aikis.or.jp", 189, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 927, "", "violet.mail.plala.or.jp", 188, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 926, "", "violet.mail.plala.or.jp", 188, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 925, "password-cleartext", "smtp.versatel.de", 187, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 924, "password-cleartext", "mx.versatel.de", 187, 143, "plain", "pop3", "%EMAILADDRESS%" },
                    { 923, "password-cleartext", "mx.versatel.de", 187, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 922, "", "outgoing.verizon.net", 186, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 921, "", "incoming.verizon.net", 186, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 920, "", "smtp.upcmail.nl", 185, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 919, "", "pop3.upcmail.nl", 185, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 902, "", "pop3.mail.tiscali.cz", 177, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 918, "", "smtp.mail.umich.edu", 184, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 916, "", "umail.mail.plala.or.jp", 183, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 915, "", "umail.mail.plala.or.jp", 183, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 914, "", "trio.mail.plala.or.jp", 182, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 913, "", "trio.mail.plala.or.jp", 182, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 912, "", "topaz.mail.plala.or.jp", 181, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 911, "", "topaz.mail.plala.or.jp", 181, 110, "plain", "pop3", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 910, "", "toccata.mail.plala.or.jp", 180, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 909, "", "toccata.mail.plala.or.jp", 180, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 908, "", "tmail.mail.plala.or.jp", 179, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 907, "", "tmail.mail.plala.or.jp", 179, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 906, "none", "smtp.tiscali.it", 178, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 905, "", "pop.tiscali.it", 178, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 904, "", "imap.tiscali.it", 178, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 917, "", "mail.umich.edu", 184, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 744, "", "pop.mail.com", 106, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 634, "", "smtp.free.fr", 60, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 742, "", "imap.mail.com", 106, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 582, "", "co3.wind.ne.jp", 37, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 581, "", "co2.wind.ne.jp", 36, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 580, "", "co2.wind.ne.jp", 36, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 579, "", "co1.wind.ne.jp", 35, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 578, "", "co1.wind.ne.jp", 35, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 577, "", "cmail.mail.plala.or.jp", 34, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 576, "", "cmail.mail.plala.or.jp", 34, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 575, "", "mail.clio.ne.jp", 33, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 574, "", "mail.clio.ne.jp", 33, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 573, "", "mobile.charter.net", 32, 587, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 572, "", "imap.charter.net", 32, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 571, "", "mobile.charter.net", 32, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 570, "", "plato.cgl.ucsf.edu", 31, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 569, "", "plato.cgl.ucsf.edu", 31, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 568, "none", "smtp.cek.ne.jp", 30, 25, "plain", "smtp", "" },
                    { 567, "", "mail.cek.ne.jp", 30, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 566, "none", "smtp.cc9.ne.jp", 29, 25, "plain", "smtp", "" },
                    { 565, "", "pop.cc9.ne.jp", 29, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 564, "", "cameo.mail.plala.or.jp", 28, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 563, "", "cameo.mail.plala.or.jp", 28, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 562, "", "camel.mail.plala.or.jp", 27, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 561, "", "camel.mail.plala.or.jp", 27, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 560, "", "brown.mail.plala.or.jp", 26, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 559, "", "brown.mail.plala.or.jp", 26, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 558, "", "mail.broba.cc", 25, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 557, "", "mail.broba.cc", 25, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 556, "", "bpost.mail.plala.or.jp", 24, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 583, "", "co3.wind.ne.jp", 37, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 584, "", "cocoa.mail.plala.or.jp", 38, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 585, "", "cocoa.mail.plala.or.jp", 38, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 586, "", "coda.mail.plala.or.jp", 39, 110, "plain", "pop3", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 614, "", "ebony.mail.plala.or.jp", 52, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 613, "", "smtpauth.earthlink.net", 51, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 612, "", "pop.earthlink.net", 51, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 611, "", "imap.earthlink.net", 51, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 610, "", "dmail.mail.plala.or.jp", 50, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 609, "", "dmail.mail.plala.or.jp", 50, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 608, "", "smtprelaypool.ispgateway.de", 49, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 607, "", "sslmailpool.ispgateway.de", 49, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 606, "", "sslmailpool.ispgateway.de", 49, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 605, "", "mbox.iij4u.or.jp", 48, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 604, "", "mbox.iij4u.or.jp", 48, 110, "STARTTLS", "pop3", "%EMAILLOCALPART%" },
                    { 603, "", "dance.mail.plala.or.jp", 47, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 602, "", "dance.mail.plala.or.jp", 47, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 555, "", "bpost.mail.plala.or.jp", 24, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 601, "", "dan.wind.ne.jp", 46, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 599, "", "cream.mail.plala.or.jp", 45, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 598, "", "cream.mail.plala.or.jp", 45, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 597, "", "cpost.mail.plala.or.jp", 44, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 596, "", "cpost.mail.plala.or.jp", 44, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 595, "", "courante.mail.plala.or.jp", 43, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 594, "", "courante.mail.plala.or.jp", 43, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 593, "", "coral.mail.plala.or.jp", 42, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 592, "", "coral.mail.plala.or.jp", 42, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 591, "", "concerto.mail.plala.or.jp", 41, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 590, "", "concerto.mail.plala.or.jp", 41, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 589, "", "smtp.comcast.net", 40, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 588, "", "mail.comcast.net", 40, 110, "STARTTLS", "pop3", "%EMAILLOCALPART%" },
                    { 587, "", "coda.mail.plala.or.jp", 39, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 600, "", "dan.wind.ne.jp", 46, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 615, "", "ebony.mail.plala.or.jp", 52, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 554, "", "bolero.mail.plala.or.jp", 23, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 552, "", "bmail.mail.plala.or.jp", 22, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 519, "password-cleartext", "imap.arcor.de", 10, 143, "STARTTLS", "imap", "%EMAILLOCALPART%" },
                    { 518, "", "imap.arcor.de", 10, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 517, "", "aqua.mail.plala.or.jp", 9, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 516, "", "aqua.mail.plala.or.jp", 9, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 515, "", "apost.mail.plala.or.jp", 8, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 514, "", "apost.mail.plala.or.jp", 8, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 513, "", "smtp.aol.com", 7, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 512, "", "pop.aol.com", 7, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 511, "", "pop.aol.com", 7, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 510, "", "imap.aol.com", 7, 143, "STARTTLS", "imap", "%EMAILADDRESS%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 509, "", "imap.aol.com", 7, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 508, "", "amber.mail.plala.or.jp", 6, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 507, "", "amber.mail.plala.or.jp", 6, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 506, "", "amail.mail.plala.or.jp", 5, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 505, "", "amail.mail.plala.or.jp", 5, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 504, "", "out.alice.it", 4, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 503, "", "in.alice.it", 4, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 502, "", "in.alice.it", 4, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 501, "", "agate.mail.plala.or.jp", 3, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 500, "", "agate.mail.plala.or.jp", 3, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 499, "", "abc.mail.plala.or.jp", 2, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 498, "", "abc.mail.plala.or.jp", 2, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 497, "password-cleartext", "smtp.1und1.de", 1, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 496, "password-cleartext", "pop.1und1.de", 1, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 495, "password-cleartext", "pop.1und1.de", 1, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 494, "password-cleartext", "imap.1und1.de", 1, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 493, "password-cleartext", "imap.1und1.de", 1, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 520, "password-cleartext", "pop3.arcor.de", 10, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 521, "password-cleartext", "mail.arcor.de", 10, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 522, "", "imaps.pec.aruba.it", 11, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 523, "", "pop3s.pec.aruba.it", 11, 993, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 551, "", "bmail.mail.plala.or.jp", 22, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 550, "", "smtpauths.bluewin.ch", 21, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 549, "", "pop3s.bluewin.ch", 21, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 548, "", "imaps.bluewin.ch", 21, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 547, "", "smtpauths.bluewin.ch", 20, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 546, "", "pop3s.bluewin.ch", 20, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 545, "", "imaps.bluewin.ch", 20, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 544, "", "blue.mail.plala.or.jp", 19, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 543, "", "blue.mail.plala.or.jp", 19, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 542, "", "mail.bigpond.com", 18, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 541, "", "mail.bigpond.com", 18, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 540, "", "mail.biglobe.ne.jp", 17, 587, "plain", "smtp", "%EMAILADDRESS%" },
                    { 539, "", "mail.biglobe.ne.jp", 17, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 553, "", "bolero.mail.plala.or.jp", 23, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 538, "", "beige.mail.plala.or.jp", 16, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 536, "", "pop.bb-niigata.jp", 15, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 535, "", "pop.bb-niigata.jp", 15, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 534, "", "bay.wind.ne.jp", 14, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 533, "", "bay.wind.ne.jp", 14, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 532, "", "ballade.mail.plala.or.jp", 13, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 531, "", "ballade.mail.plala.or.jp", 13, 110, "plain", "pop3", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 530, "none", "78.40.187.62", 216, 25, "plain", "smtp", "" },
                    { 529, "", "78.40.187.62", 216, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 528, "none", "mail.avsmedia.com", 215, 25, "plain", "smtp", "" },
                    { 527, "", "mail.avsmedia.com", 215, 110, "plain", "pop3", "%EMAILLOCALPART%@%EMAILHOSTNAME%" },
                    { 526, "", "smtp.att.yahoo.com", 12, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 525, "", "pop.att.yahoo.com", 12, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 524, "", "smtps.pec.aruba.it", 11, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 537, "", "beige.mail.plala.or.jp", 16, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 616, "", "imapmail.email.it", 53, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 617, "", "popmail.email.it", 53, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 618, "", "smtp.email.it", 53, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 709, "", "smtp.jet.ne.jp", 90, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 708, "", "imap.jet.ne.jp", 90, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 707, "", "pop.jet.ne.jp", 90, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 706, "none", "smtp.%EMAILDOMAIN%", 89, 25, "plain", "smtp", "" },
                    { 705, "", "mail.%EMAILDOMAIN%", 89, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 704, "", "jade.mail.plala.or.jp", 88, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 703, "", "jade.mail.plala.or.jp", 88, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 702, "", "po.iwafune.ne.jp", 87, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 701, "", "po.iwafune.ne.jp", 87, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 700, "", "ivory.mail.plala.or.jp", 86, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 699, "", "ivory.mail.plala.or.jp", 86, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 698, "", "mail.ipax.at", 85, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 697, "", "mail.ipax.at", 85, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 696, "password-cleartext", "mail.internode.on.net", 84, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 695, "password-cleartext", "mail.internode.on.net", 84, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 694, "password-cleartext", "mail.internode.on.net", 84, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 693, "", "po.inet-shibata.or.jp", 83, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 692, "", "po.inet-shibata.or.jp", 83, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 691, "", "indigo.mail.plala.or.jp", 82, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 690, "", "indigo.mail.plala.or.jp", 82, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 689, "", "mail.inbox.lv", 81, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 688, "", "mail.inbox.lv", 81, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 687, "", "mail.inbox.lt", 80, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 686, "", "mail.inbox.lt", 80, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 685, "", "imail.mail.plala.or.jp", 79, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 684, "", "imail.mail.plala.or.jp", 79, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 683, "none", "smtp.iiyama-catv.ne.jp", 78, 25, "plain", "smtp", "" },
                    { 710, "", "pop02.jet.ne.jp", 91, 110, "plain", "pop3", "%%EMAILLOCALPART%%" },
                    { 711, "", "smtp02.jet.ne.jp", 91, 587, "plain", "smtp", "%%EMAILLOCALPART%%" },
                    { 712, "", "jmail.mail.plala.or.jp", 92, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 713, "", "jmail.mail.plala.or.jp", 92, 587, "plain", "smtp", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 741, "", "mail.mahoroba.ne.jp", 105, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 740, "", "mail.mahoroba.ne.jp", 105, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 739, "", "lime.mail.plala.or.jp", 104, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 738, "", "lime.mail.plala.or.jp", 104, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 737, "", "lilac.mail.plala.or.jp", 103, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 736, "", "lilac.mail.plala.or.jp", 103, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 735, "", "smtp.libero.it", 102, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 734, "", "popmail.libero.it", 102, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 733, "", "imapmail.libero.it", 102, 143, "plain", "imap", "%EMAILADDRESS%" },
                    { 732, "", "lemon.mail.plala.or.jp", 101, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 731, "", "lemon.mail.plala.or.jp", 101, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 730, "", "smtp.laposte.net", 100, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 729, "", "pop.laposte.net", 100, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 682, "", "mail.iiyama-catv.ne.jp", 78, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 728, "", "imap.laposte.net", 100, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 726, "", "lapis.mail.plala.or.jp", 99, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 725, "none", "smtp.kokuyou.ne.jp", 98, 25, "plain", "smtp", "" },
                    { 724, "", "mail.kokuyou.ne.jp", 98, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 723, "", "kmail.mail.plala.or.jp", 97, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 722, "", "kmail.mail.plala.or.jp", 97, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 721, "", "kl.wind.ne.jp", 96, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 720, "", "kl.wind.ne.jp", 96, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 719, "", "khaki.mail.plala.or.jp", 95, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 718, "", "khaki.mail.plala.or.jp", 95, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 717, "", "smtp.kelcom.net", 94, 25, "plain", "smtp", "%EMAILADDRESS%" },
                    { 716, "", "pop1.kelcom.net", 94, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 715, "", "smtp.kabelmail.de", 93, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 714, "", "pop3.kabelmail.de", 93, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 727, "", "lapis.mail.plala.or.jp", 99, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 681, "", "mbox.iijmio-mail.jp", 77, 587, "STARTTLS", "smtp", "%EMAILLOCALPART%.%EMAILDOMAIN%" },
                    { 680, "", "mbox.iijmio-mail.jp", 77, 110, "STARTTLS", "pop3", "%EMAILLOCALPART%.%EMAILDOMAIN%" },
                    { 679, "", "smtp.ic-net.or.jp", 76, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 646, "password-cleartext", "mail.gandi.net", 63, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 645, "password-cleartext", "mail.gandi.net", 63, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 644, "password-cleartext", "mail.gandi.net", 63, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 643, "password-cleartext", "mail.gandi.net", 63, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 642, "password-cleartext", "mail.gandi.net", 63, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 641, "", "fuga.mail.plala.or.jp", 62, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 640, "", "fuga.mail.plala.or.jp", 62, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 639, "password-encrypted", "mx.freenet.de", 61, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 638, "password-cleartext", "mx.freenet.de", 61, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 637, "password-cleartext", "mx.freenet.de", 61, 995, "SSL", "pop3", "%EMAILADDRESS%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 636, "password-encrypted", "mx.freenet.de", 61, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 635, "password-encrypted", "mx.freenet.de", 61, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 997, "", "outlook.office365.com", 221, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 647, "", "gmail.mail.plala.or.jp", 64, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 633, "", "pop.free.fr", 60, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 631, "", "smtp.orange.fr", 59, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 630, "", "imap.orange.fr", 59, 143, "plain", "imap", "%EMAILLOCALPART%" },
                    { 629, "", "pop.orange.fr", 59, 995, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 628, "", "fmail.mail.plala.or.jp", 58, 143, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 627, "", "fmail.mail.plala.or.jp", 58, 993, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 626, "", "flamenco.mail.plala.or.jp", 57, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 625, "", "flamenco.mail.plala.or.jp", 57, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 624, "", "fantasy.mail.plala.or.jp", 56, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 623, "", "fantasy.mail.plala.or.jp", 56, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 622, "password-cleartext", "smtps-1.ewetel.net", 55, 25, "STARTTLS", "smtp", "%EMAILLOCALPART%" },
                    { 621, "password-cleartext", "pop3s-1.ewetel.net", 55, 995, "SSL", "pop3", "%EMAILLOCALPART%" },
                    { 620, "", "email.mail.plala.or.jp", 54, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 619, "", "email.mail.plala.or.jp", 54, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 632, "", "imap.free.fr", 60, 993, "SSL", "imap", "%EMAILLOCALPART%" },
                    { 743, "", "imap.mail.com", 106, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 648, "", "gmail.mail.plala.or.jp", 64, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 650, "", "imap.gmx.com", 65, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 678, "", "mail.ic-net.or.jp", 76, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 677, "password-cleartext", "smtp.softbank.jp", 75, 465, "SSL", "smtp", "%EMAILLOCALPART%" },
                    { 676, "password-cleartext", "imap.softbank.jp", 75, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 675, "", "smtp-mail.outlook.com", 74, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" },
                    { 674, "", "pop-mail.outlook.com", 74, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 673, "", "mail.hana.or.jp", 73, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 672, "", "mail.hana.or.jp", 73, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 671, "", "mail.hal.ne.jp", 72, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 670, "", "mail.hal.ne.jp", 72, 110, "plain", "pop3", "%EMAILADDRESS%" },
                    { 669, "", "gray.mail.plala.or.jp", 71, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 668, "", "gray.mail.plala.or.jp", 71, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 667, "", "grape.mail.plala.or.jp", 70, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 666, "", "grape.mail.plala.or.jp", 70, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 649, "", "imap.gmx.com", 65, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 665, "", "smtp.googlemail.com", 69, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 663, "", "imap.googlemail.com", 69, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 662, "", "smtp.mail.goo.ne.jp", 68, 587, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 661, "", "pop.mail.goo.ne.jp", 68, 110, "plain", "pop3", "%EMAILLOCALPART%" },
                    { 660, "", "go.tvm.ne.jp", 67, 25, "plain", "smtp", "%EMAILLOCALPART%" },
                    { 659, "", "go.tvm.ne.jp", 67, 110, "plain", "pop3", "%EMAILLOCALPART%" }
                });

            migrationBuilder.InsertData(
                table: "mail_mailbox_server",
                columns: new[] { "id", "authentication", "hostname", "id_provider", "port", "socket_type", "type", "username" },
                values: new object[,]
                {
                    { 658, "", "mail.gmx.net", 66, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 657, "", "pop.gmx.net", 66, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 656, "", "pop.gmx.net", 66, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 655, "", "imap.gmx.net", 66, 143, "STARTTLS", "imap", "%EMAILADDRESS%" },
                    { 654, "", "imap.gmx.net", 66, 993, "SSL", "imap", "%EMAILADDRESS%" },
                    { 653, "", "mail.gmx.com", 65, 465, "SSL", "smtp", "%EMAILADDRESS%" },
                    { 652, "", "pop.gmx.com", 65, 110, "STARTTLS", "pop3", "%EMAILADDRESS%" },
                    { 651, "", "pop.gmx.com", 65, 995, "SSL", "pop3", "%EMAILADDRESS%" },
                    { 664, "", "pop.googlemail.com", 69, 995, "SSL", "pop3", "recent:%EMAILADDRESS%" },
                    { 998, "", "smtp.office365.com", 221, 587, "STARTTLS", "smtp", "%EMAILADDRESS%" }
                });

            migrationBuilder.CreateIndex(
                name: "address_index",
                table: "mail_mailbox",
                column: "address");

            migrationBuilder.CreateIndex(
                name: "date_login_delay_expires",
                table: "mail_mailbox",
                columns: new[] { "date_checked", "date_login_delay_expires" });

            migrationBuilder.CreateIndex(
                name: "main_mailbox_id_in_server_mail_mailbox_server_id",
                table: "mail_mailbox",
                column: "id_in_server");

            migrationBuilder.CreateIndex(
                name: "main_mailbox_id_smtp_server_mail_mailbox_server_id",
                table: "mail_mailbox",
                column: "id_smtp_server");

            migrationBuilder.CreateIndex(
                name: "user_id_index",
                table: "mail_mailbox",
                columns: new[] { "tenant", "id_user" });

            migrationBuilder.CreateIndex(
                name: "id_provider_mail_mailbox_server",
                table: "mail_mailbox_server",
                column: "id_provider");

            migrationBuilder.CreateIndex(
                name: "mail_server_server_type_server_type_fk_id",
                table: "mail_server_server",
                column: "server_type");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "api_keys");

            migrationBuilder.DropTable(
                name: "greylisting_whitelist");

            migrationBuilder.DropTable(
                name: "mail_mailbox");

            migrationBuilder.DropTable(
                name: "mail_mailbox_provider");

            migrationBuilder.DropTable(
                name: "mail_mailbox_server");

            migrationBuilder.DropTable(
                name: "mail_server_server");
        }
    }
}
