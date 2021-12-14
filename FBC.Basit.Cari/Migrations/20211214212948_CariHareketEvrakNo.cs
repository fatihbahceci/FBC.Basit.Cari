using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBC.Basit.Cari.Migrations
{
    public partial class CariHareketEvrakNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EvrakNo",
                table: "CariHareket",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EvrakNo",
                table: "CariHareket");
        }
    }
}
