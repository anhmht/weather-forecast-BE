using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

namespace GloboWeather.WeatherManagement.Weather.Migrations
{
    public partial class RefDateIntoNhietdo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "capgio",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: true),
                    Name = table.Column<string>(maxLength: 255, nullable: true),
                    Description = table.Column<string>(maxLength: 255, nullable: true),
                    Windms = table.Column<float>(name: "Wind(m/s)", nullable: true),
                    Wavem = table.Column<float>(name: "Wave(m)", nullable: true),
                    Color = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "diemdubao",
                columns: table => new
                {
                    ID = table.Column<string>(maxLength: 255, nullable: false),
                    Ten = table.Column<string>(maxLength: 255, nullable: true),
                    X = table.Column<float>(nullable: true),
                    Y = table.Column<float>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_diemdubao", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "htthoitiet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true),
                    _24h = table.Column<byte>(name: "24h", type: "tinyint(3) unsigned", nullable: true),
                    Ngay = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true),
                    Dem = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_htthoitiet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "icon24",
                columns: table => new
                {
                    Icon = table.Column<string>(maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true),
                    Index = table.Column<int>(type: "int(11)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Icon);
                });

            migrationBuilder.CreateTable(
                name: "icondem",
                columns: table => new
                {
                    Icon = table.Column<string>(maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Icon);
                });

            migrationBuilder.CreateTable(
                name: "iconngay",
                columns: table => new
                {
                    Icon = table.Column<string>(maxLength: 255, nullable: false),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Icon);
                });

            migrationBuilder.CreateTable(
                name: "iconthoitiet",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true),
                    Icon = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_iconthoitiet", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "may",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true),
                    _24h = table.Column<byte>(name: "24h", type: "tinyint(3) unsigned", nullable: true),
                    Ngay = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true),
                    Dem = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_may", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "mua",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true),
                    _24h = table.Column<byte>(name: "24h", type: "tinyint(3) unsigned", nullable: true),
                    Ngay = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true),
                    Dem = table.Column<byte>(type: "tinyint(3) unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mua", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "nhietdo",
                columns: table => new
                {
                    DiemId = table.Column<string>(maxLength: 255, nullable: false),
                    RefDate = table.Column<DateTime>(type: "date", nullable: false),
                    _1 = table.Column<int>(name: "1", type: "int(2)", nullable: true),
                    _2 = table.Column<int>(name: "2", type: "int(2)", nullable: true),
                    _3 = table.Column<int>(name: "3", type: "int(2)", nullable: true),
                    _4 = table.Column<int>(name: "4", type: "int(2)", nullable: true),
                    _5 = table.Column<int>(name: "5", type: "int(2)", nullable: true),
                    _6 = table.Column<int>(name: "6", type: "int(2)", nullable: true),
                    _7 = table.Column<int>(name: "7", type: "int(2)", nullable: true),
                    _8 = table.Column<int>(name: "8", type: "int(2)", nullable: true),
                    _9 = table.Column<int>(name: "9", type: "int(2)", nullable: true),
                    _10 = table.Column<int>(name: "10", type: "int(2)", nullable: true),
                    _11 = table.Column<int>(name: "11", type: "int(2)", nullable: true),
                    _12 = table.Column<int>(name: "12", type: "int(2)", nullable: true),
                    _13 = table.Column<int>(name: "13", type: "int(2)", nullable: true),
                    _14 = table.Column<int>(name: "14", type: "int(2)", nullable: true),
                    _15 = table.Column<int>(name: "15", type: "int(2)", nullable: true),
                    _16 = table.Column<int>(name: "16", type: "int(2)", nullable: true),
                    _17 = table.Column<int>(name: "17", type: "int(2)", nullable: true),
                    _18 = table.Column<int>(name: "18", type: "int(2)", nullable: true),
                    _19 = table.Column<int>(name: "19", type: "int(2)", nullable: true),
                    _20 = table.Column<int>(name: "20", type: "int(2)", nullable: true),
                    _21 = table.Column<int>(name: "21", type: "int(2)", nullable: true),
                    _22 = table.Column<int>(name: "22", type: "int(2)", nullable: true),
                    _23 = table.Column<int>(name: "23", type: "int(2)", nullable: true),
                    _24 = table.Column<int>(name: "24", type: "int(2)", nullable: true),
                    _25 = table.Column<int>(name: "25", type: "int(2)", nullable: true),
                    _26 = table.Column<int>(name: "26", type: "int(2)", nullable: true),
                    _27 = table.Column<int>(name: "27", type: "int(2)", nullable: true),
                    _28 = table.Column<int>(name: "28", type: "int(2)", nullable: true),
                    _29 = table.Column<int>(name: "29", type: "int(2)", nullable: true),
                    _30 = table.Column<int>(name: "30", type: "int(2)", nullable: true),
                    _31 = table.Column<int>(name: "31", type: "int(2)", nullable: true),
                    _32 = table.Column<int>(name: "32", type: "int(2)", nullable: true),
                    _33 = table.Column<int>(name: "33", type: "int(2)", nullable: true),
                    _34 = table.Column<int>(name: "34", type: "int(2)", nullable: true),
                    _35 = table.Column<int>(name: "35", type: "int(2)", nullable: true),
                    _36 = table.Column<int>(name: "36", type: "int(2)", nullable: true),
                    _37 = table.Column<int>(name: "37", type: "int(2)", nullable: true),
                    _38 = table.Column<int>(name: "38", type: "int(2)", nullable: true),
                    _39 = table.Column<int>(name: "39", type: "int(2)", nullable: true),
                    _40 = table.Column<int>(name: "40", type: "int(2)", nullable: true),
                    _41 = table.Column<int>(name: "41", type: "int(2)", nullable: true),
                    _42 = table.Column<int>(name: "42", type: "int(2)", nullable: true),
                    _43 = table.Column<int>(name: "43", type: "int(2)", nullable: true),
                    _44 = table.Column<int>(name: "44", type: "int(2)", nullable: true),
                    _45 = table.Column<int>(name: "45", type: "int(2)", nullable: true),
                    _46 = table.Column<int>(name: "46", type: "int(2)", nullable: true),
                    _47 = table.Column<int>(name: "47", type: "int(2)", nullable: true),
                    _48 = table.Column<int>(name: "48", type: "int(2)", nullable: true),
                    _49 = table.Column<int>(name: "49", type: "int(2)", nullable: true),
                    _50 = table.Column<int>(name: "50", type: "int(2)", nullable: true),
                    _51 = table.Column<int>(name: "51", type: "int(2)", nullable: true),
                    _52 = table.Column<int>(name: "52", type: "int(2)", nullable: true),
                    _53 = table.Column<int>(name: "53", type: "int(2)", nullable: true),
                    _54 = table.Column<int>(name: "54", type: "int(2)", nullable: true),
                    _55 = table.Column<int>(name: "55", type: "int(2)", nullable: true),
                    _56 = table.Column<int>(name: "56", type: "int(2)", nullable: true),
                    _57 = table.Column<int>(name: "57", type: "int(2)", nullable: true),
                    _58 = table.Column<int>(name: "58", type: "int(2)", nullable: true),
                    _59 = table.Column<int>(name: "59", type: "int(2)", nullable: true),
                    _60 = table.Column<int>(name: "60", type: "int(2)", nullable: true),
                    _61 = table.Column<int>(name: "61", type: "int(2)", nullable: true),
                    _62 = table.Column<int>(name: "62", type: "int(2)", nullable: true),
                    _63 = table.Column<int>(name: "63", type: "int(2)", nullable: true),
                    _64 = table.Column<int>(name: "64", type: "int(2)", nullable: true),
                    _65 = table.Column<int>(name: "65", type: "int(2)", nullable: true),
                    _66 = table.Column<int>(name: "66", type: "int(2)", nullable: true),
                    _67 = table.Column<int>(name: "67", type: "int(2)", nullable: true),
                    _68 = table.Column<int>(name: "68", type: "int(2)", nullable: true),
                    _69 = table.Column<int>(name: "69", type: "int(2)", nullable: true),
                    _70 = table.Column<int>(name: "70", type: "int(2)", nullable: true),
                    _71 = table.Column<int>(name: "71", type: "int(2)", nullable: true),
                    _72 = table.Column<int>(name: "72", type: "int(2)", nullable: true),
                    _73 = table.Column<int>(name: "73", type: "int(2)", nullable: true),
                    _74 = table.Column<int>(name: "74", type: "int(2)", nullable: true),
                    _75 = table.Column<int>(name: "75", type: "int(2)", nullable: true),
                    _76 = table.Column<int>(name: "76", type: "int(2)", nullable: true),
                    _77 = table.Column<int>(name: "77", type: "int(2)", nullable: true),
                    _78 = table.Column<int>(name: "78", type: "int(2)", nullable: true),
                    _79 = table.Column<int>(name: "79", type: "int(2)", nullable: true),
                    _80 = table.Column<int>(name: "80", type: "int(2)", nullable: true),
                    _81 = table.Column<int>(name: "81", type: "int(2)", nullable: true),
                    _82 = table.Column<int>(name: "82", type: "int(2)", nullable: true),
                    _83 = table.Column<int>(name: "83", type: "int(2)", nullable: true),
                    _84 = table.Column<int>(name: "84", type: "int(2)", nullable: true),
                    _85 = table.Column<int>(name: "85", type: "int(2)", nullable: true),
                    _86 = table.Column<int>(name: "86", type: "int(2)", nullable: true),
                    _87 = table.Column<int>(name: "87", type: "int(2)", nullable: true),
                    _88 = table.Column<int>(name: "88", type: "int(2)", nullable: true),
                    _89 = table.Column<int>(name: "89", type: "int(2)", nullable: true),
                    _90 = table.Column<int>(name: "90", type: "int(2)", nullable: true),
                    _91 = table.Column<int>(name: "91", type: "int(2)", nullable: true),
                    _92 = table.Column<int>(name: "92", type: "int(2)", nullable: true),
                    _93 = table.Column<int>(name: "93", type: "int(2)", nullable: true),
                    _94 = table.Column<int>(name: "94", type: "int(2)", nullable: true),
                    _95 = table.Column<int>(name: "95", type: "int(2)", nullable: true),
                    _96 = table.Column<int>(name: "96", type: "int(2)", nullable: true),
                    _97 = table.Column<int>(name: "97", type: "int(2)", nullable: true),
                    _98 = table.Column<int>(name: "98", type: "int(2)", nullable: true),
                    _99 = table.Column<int>(name: "99", type: "int(2)", nullable: true),
                    _100 = table.Column<int>(name: "100", type: "int(2)", nullable: true),
                    _101 = table.Column<int>(name: "101", type: "int(2)", nullable: true),
                    _102 = table.Column<int>(name: "102", type: "int(2)", nullable: true),
                    _103 = table.Column<int>(name: "103", type: "int(2)", nullable: true),
                    _104 = table.Column<int>(name: "104", type: "int(2)", nullable: true),
                    _105 = table.Column<int>(name: "105", type: "int(2)", nullable: true),
                    _106 = table.Column<int>(name: "106", type: "int(2)", nullable: true),
                    _107 = table.Column<int>(name: "107", type: "int(2)", nullable: true),
                    _108 = table.Column<int>(name: "108", type: "int(2)", nullable: true),
                    _109 = table.Column<int>(name: "109", type: "int(2)", nullable: true),
                    _110 = table.Column<int>(name: "110", type: "int(2)", nullable: true),
                    _111 = table.Column<int>(name: "111", type: "int(2)", nullable: true),
                    _112 = table.Column<int>(name: "112", type: "int(2)", nullable: true),
                    _113 = table.Column<int>(name: "113", type: "int(2)", nullable: true),
                    _114 = table.Column<int>(name: "114", type: "int(2)", nullable: true),
                    _115 = table.Column<int>(name: "115", type: "int(2)", nullable: true),
                    _116 = table.Column<int>(name: "116", type: "int(2)", nullable: true),
                    _117 = table.Column<int>(name: "117", type: "int(2)", nullable: true),
                    _118 = table.Column<int>(name: "118", type: "int(2)", nullable: true),
                    _119 = table.Column<int>(name: "119", type: "int(2)", nullable: true),
                    _120 = table.Column<int>(name: "120", type: "int(2)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.DiemId);
                });

            migrationBuilder.CreateTable(
                name: "thoigian",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(11)", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    MoTa = table.Column<string>(maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_thoigian", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "DiemId",
                table: "nhietdo",
                column: "DiemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "capgio");

            migrationBuilder.DropTable(
                name: "diemdubao");

            migrationBuilder.DropTable(
                name: "htthoitiet");

            migrationBuilder.DropTable(
                name: "icon24");

            migrationBuilder.DropTable(
                name: "icondem");

            migrationBuilder.DropTable(
                name: "iconngay");

            migrationBuilder.DropTable(
                name: "iconthoitiet");

            migrationBuilder.DropTable(
                name: "may");

            migrationBuilder.DropTable(
                name: "mua");

            migrationBuilder.DropTable(
                name: "nhietdo");

            migrationBuilder.DropTable(
                name: "thoigian");
        }
    }
}
