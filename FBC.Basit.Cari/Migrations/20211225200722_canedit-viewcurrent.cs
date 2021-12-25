using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBC.Basit.Cari.Migrations
{
    public partial class caneditviewcurrent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CariKartId",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanEditData",
                table: "Users",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CariKartId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsCanEditData",
                table: "Users");
        }
    }
}
