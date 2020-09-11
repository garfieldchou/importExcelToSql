using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class CreateDetailsGamesReviewerHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DetailsGamesReviewerHistory",
                columns: table => new
                {
                    DetailsGamesReviewerHistoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DetailsGameId = table.Column<int>(nullable: false),
                    RecordYear = table.Column<int>(nullable: false),
                    RecordMonth = table.Column<int>(nullable: false),
                    RecordWorkWeek = table.Column<int>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    RecentReviews = table.Column<string>(nullable: true),
                    AllReviews = table.Column<string>(nullable: true),
                    RecentReviewsPosCount = table.Column<int>(nullable: true),
                    RecentReviewsPosPercent = table.Column<decimal>(nullable: true),
                    AllReviewsPosCount = table.Column<int>(nullable: true),
                    AllReviewsPosPercent = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsGamesReviewerHistory", x => x.DetailsGamesReviewerHistoryId);
                    table.ForeignKey(
                        name: "FK_DetailsGamesReviewerHistory_DetailsGames_DetailsGameId",
                        column: x => x.DetailsGameId,
                        principalTable: "DetailsGames",
                        principalColumn: "DetailsGameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DetailsGamesReviewerHistory_DetailsGameId",
                table: "DetailsGamesReviewerHistory",
                column: "DetailsGameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DetailsGamesReviewerHistory");
        }
    }
}
