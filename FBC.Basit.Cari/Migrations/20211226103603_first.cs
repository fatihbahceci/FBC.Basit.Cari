using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBC.Basit.Cari.Migrations
{
    public partial class first : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CariKart",
                columns: table => new
                {
                    CariKartId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Isim = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariKart", x => x.CariKartId);
                });

            migrationBuilder.CreateTable(
                name: "CariHareket",
                columns: table => new
                {
                    CariHareketId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Aciklama = table.Column<string>(type: "TEXT", nullable: true),
                    EvrakNo = table.Column<string>(type: "TEXT", nullable: true),
                    Borc = table.Column<decimal>(type: "TEXT", nullable: false),
                    Alacak = table.Column<decimal>(type: "TEXT", nullable: false),
                    VadeTarihi = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CariKartId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CariHareket", x => x.CariHareketId);
                    table.ForeignKey(
                        name: "FK_CariHareket_CariKart_CariKartId",
                        column: x => x.CariKartId,
                        principalTable: "CariKart",
                        principalColumn: "CariKartId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    SysUserId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SysUserName = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Surname = table.Column<string>(type: "TEXT", nullable: false),
                    SysUserPassword = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsCanEditData = table.Column<bool>(type: "INTEGER", nullable: false),
                    CariKartId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.SysUserId);
                    table.ForeignKey(
                        name: "FK_Users_CariKart_CariKartId",
                        column: x => x.CariKartId,
                        principalTable: "CariKart",
                        principalColumn: "CariKartId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CariHareket_CariKartId",
                table: "CariHareket",
                column: "CariKartId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CariKartId",
                table: "Users",
                column: "CariKartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CariHareket");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CariKart");
        }
    }
}
