using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class AddForeignKeyToGameRanks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DetailsGameId",
                table: "GameRanks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_GameRanks_DetailsGameId",
                table: "GameRanks",
                column: "DetailsGameId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameRanks_DetailsGames_DetailsGameId",
                table: "GameRanks",
                column: "DetailsGameId",
                principalTable: "DetailsGames",
                principalColumn: "DetailsGameId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameRanks_DetailsGames_DetailsGameId",
                table: "GameRanks");

            migrationBuilder.DropIndex(
                name: "IX_GameRanks_DetailsGameId",
                table: "GameRanks");

            migrationBuilder.DropColumn(
                name: "DetailsGameId",
                table: "GameRanks");
        }
    }
}
