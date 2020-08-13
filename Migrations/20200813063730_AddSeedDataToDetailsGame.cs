using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class AddSeedDataToDetailsGame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "DetailsGames",
                columns: new[] { "DetailsGameId", "AllReviews", "Game", "GameDescription", "HotTags", "RecentReviews", "ReleaseDate", "SystemRequirements" },
                values: new object[] { 1, "", "", "", "", "", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DetailsGames",
                keyColumn: "DetailsGameId",
                keyValue: 1);
        }
    }
}
