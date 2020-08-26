using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class AddLinksColumnToGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Links",
                table: "GameRanks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Links",
                table: "DetailsGames",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Links",
                table: "GameRanks");

            migrationBuilder.DropColumn(
                name: "Links",
                table: "DetailsGames");
        }
    }
}
