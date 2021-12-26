using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBC.Basit.Cari.Migrations
{
    public partial class userfkcarikart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Users_CariKartId",
                table: "Users",
                column: "CariKartId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CariKart_CariKartId",
                table: "Users",
                column: "CariKartId",
                principalTable: "CariKart",
                principalColumn: "CariKartId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_CariKart_CariKartId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_CariKartId",
                table: "Users");
        }
    }
}
