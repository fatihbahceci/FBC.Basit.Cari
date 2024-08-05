using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBC.Basit.Cari.Migrations
{
    public partial class Doviz : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DovizKuruId",
                table: "CariHareket",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DovizKuru",
                columns: table => new
                {
                    DovizKuruId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DovizCinsi = table.Column<string>(type: "TEXT", nullable: false),
                    DovizAdi = table.Column<string>(type: "TEXT", nullable: false),
                    GuncelKurAlis = table.Column<decimal>(type: "TEXT", nullable: false),
                    GuncelKurSatis = table.Column<decimal>(type: "TEXT", nullable: false),
                    GuncellemeTarihi = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DovizKuru", x => x.DovizKuruId);
                });

            migrationBuilder.CreateTable(
                name: "DovizKuruHareket",
                columns: table => new
                {
                    DovizKuruHareketId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Tarih = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Alis = table.Column<decimal>(type: "TEXT", nullable: false),
                    Satis = table.Column<decimal>(type: "TEXT", nullable: false),
                    Kaynak = table.Column<string>(type: "TEXT", nullable: false),
                    DovizKuruId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DovizKuruHareket", x => x.DovizKuruHareketId);
                    table.ForeignKey(
                        name: "FK_DovizKuruHareket_DovizKuru_DovizKuruId",
                        column: x => x.DovizKuruId,
                        principalTable: "DovizKuru",
                        principalColumn: "DovizKuruId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CariHareket_DovizKuruId",
                table: "CariHareket",
                column: "DovizKuruId");

            migrationBuilder.CreateIndex(
                name: "IX_DovizKuru_DovizCinsi",
                table: "DovizKuru",
                column: "DovizCinsi",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DovizKuruHareket_DovizKuruId",
                table: "DovizKuruHareket",
                column: "DovizKuruId");

            migrationBuilder.CreateIndex(
                name: "IX_DovizKuruHareket_Tarih_DovizKuruId",
                table: "DovizKuruHareket",
                columns: new[] { "Tarih", "DovizKuruId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CariHareket_DovizKuru_DovizKuruId",
                table: "CariHareket",
                column: "DovizKuruId",
                principalTable: "DovizKuru",
                principalColumn: "DovizKuruId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CariHareket_DovizKuru_DovizKuruId",
                table: "CariHareket");

            migrationBuilder.DropTable(
                name: "DovizKuruHareket");

            migrationBuilder.DropTable(
                name: "DovizKuru");

            migrationBuilder.DropIndex(
                name: "IX_CariHareket_DovizKuruId",
                table: "CariHareket");

            migrationBuilder.DropColumn(
                name: "DovizKuruId",
                table: "CariHareket");
        }
    }
}
