using Microsoft.EntityFrameworkCore.Migrations;

namespace DVD_Samling.Data.Migrations
{
    public partial class ChangeGenreNameToName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenreName",
                table: "Genre");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Genre",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Genre");

            migrationBuilder.AddColumn<string>(
                name: "GenreName",
                table: "Genre",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
