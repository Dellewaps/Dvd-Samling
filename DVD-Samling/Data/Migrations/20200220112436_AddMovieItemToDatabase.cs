using Microsoft.EntityFrameworkCore.Migrations;

namespace DVD_Samling.Data.Migrations
{
    public partial class AddMovieItemToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "movieItems",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Subtitels = table.Column<string>(nullable: true),
                    Agelimit = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    GenreId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_movieItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_movieItems_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_movieItems_GenreId",
                table: "movieItems",
                column: "GenreId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movieItems");
        }
    }
}
