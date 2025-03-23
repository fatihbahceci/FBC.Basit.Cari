using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBC.Basit.Cari.Migrations
{
    /// <inheritdoc />
    public partial class CariAktif : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Aktif",
                table: "CariKart",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);
            //Set Aktif to true for all existing records
            migrationBuilder.Sql("UPDATE CariKart SET Aktif = 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Aktif",
                table: "CariKart");
        }
    }
}
