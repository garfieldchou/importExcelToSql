using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace importSteamToSql.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CountryDLStatOverviews",
                columns: table => new
                {
                    CountryDLStatOverviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    TotalTb = table.Column<decimal>(nullable: false),
                    AvgDlSpeedMbps = table.Column<decimal>(nullable: false),
                    SteamPercent = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryDLStatOverviews", x => x.CountryDLStatOverviewId);
                });

            migrationBuilder.CreateTable(
                name: "CountryLists",
                columns: table => new
                {
                    CountryListId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Territory = table.Column<string>(nullable: true),
                    Region = table.Column<string>(nullable: true),
                    Country = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryLists", x => x.CountryListId);
                });

            migrationBuilder.CreateTable(
                name: "CountryNetworkDLStats",
                columns: table => new
                {
                    CountryNetworkDLStatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Time = table.Column<DateTime>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    Network = table.Column<string>(nullable: true),
                    AvgDlSpeedMbps = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CountryNetworkDLStats", x => x.CountryNetworkDLStatId);
                });

            migrationBuilder.CreateTable(
                name: "DetailsGames",
                columns: table => new
                {
                    DetailsGameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Game = table.Column<string>(nullable: true),
                    GameDescription = table.Column<string>(nullable: true),
                    RecentReviews = table.Column<string>(nullable: true),
                    AllReviews = table.Column<string>(nullable: true),
                    ReleaseDate = table.Column<DateTime>(nullable: false),
                    HotTags = table.Column<string>(nullable: true),
                    SystemRequirements = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DetailsGames", x => x.DetailsGameId);
                });

            migrationBuilder.CreateTable(
                name: "DirectXOSs",
                columns: table => new
                {
                    DirectXOSId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DirectXOSs", x => x.DirectXOSId);
                });

            migrationBuilder.CreateTable(
                name: "GameRanks",
                columns: table => new
                {
                    GameRankId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    Ranks = table.Column<int>(nullable: false),
                    Players = table.Column<int>(nullable: false),
                    Peak = table.Column<int>(nullable: false),
                    Game = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameRanks", x => x.GameRankId);
                });

            migrationBuilder.CreateTable(
                name: "HWSurveys",
                columns: table => new
                {
                    HWSurveyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HWSurveys", x => x.HWSurveyId);
                });

            migrationBuilder.CreateTable(
                name: "OnlineStats",
                columns: table => new
                {
                    OnlineStatId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<TimeSpan>(nullable: false),
                    DateTime = table.Column<DateTime>(nullable: false),
                    Players = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OnlineStats", x => x.OnlineStatId);
                });

            migrationBuilder.CreateTable(
                name: "PcPhyCpuDetails",
                columns: table => new
                {
                    PcPhyCpuDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PcPhyCpuDetails", x => x.PcPhyCpuDetailId);
                });

            migrationBuilder.CreateTable(
                name: "PCVideoCardUsageDetails",
                columns: table => new
                {
                    PCVideoCardUsageDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PCVideoCardUsageDetails", x => x.PCVideoCardUsageDetailId);
                });

            migrationBuilder.CreateTable(
                name: "ProceUsageDetails",
                columns: table => new
                {
                    ProceUsageDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Category = table.Column<string>(nullable: true),
                    Item = table.Column<string>(nullable: true),
                    Percentage = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProceUsageDetails", x => x.ProceUsageDetailId);
                });

            migrationBuilder.CreateTable(
                name: "RegionDLStatDetails",
                columns: table => new
                {
                    RegionDLStatDetailId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Time = table.Column<DateTime>(nullable: false),
                    Country = table.Column<string>(nullable: true),
                    BandWidthGbps = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionDLStatDetails", x => x.RegionDLStatDetailId);
                });

            migrationBuilder.CreateTable(
                name: "RegionDLStatOverviews",
                columns: table => new
                {
                    RegionDLStatOverviewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(nullable: false),
                    Month = table.Column<int>(nullable: false),
                    WorkWeek = table.Column<int>(nullable: false),
                    Day = table.Column<int>(nullable: false),
                    Region = table.Column<string>(nullable: true),
                    Average = table.Column<string>(nullable: true),
                    Max = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegionDLStatOverviews", x => x.RegionDLStatOverviewId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CountryDLStatOverviews");

            migrationBuilder.DropTable(
                name: "CountryLists");

            migrationBuilder.DropTable(
                name: "CountryNetworkDLStats");

            migrationBuilder.DropTable(
                name: "DetailsGames");

            migrationBuilder.DropTable(
                name: "DirectXOSs");

            migrationBuilder.DropTable(
                name: "GameRanks");

            migrationBuilder.DropTable(
                name: "HWSurveys");

            migrationBuilder.DropTable(
                name: "OnlineStats");

            migrationBuilder.DropTable(
                name: "PcPhyCpuDetails");

            migrationBuilder.DropTable(
                name: "PCVideoCardUsageDetails");

            migrationBuilder.DropTable(
                name: "ProceUsageDetails");

            migrationBuilder.DropTable(
                name: "RegionDLStatDetails");

            migrationBuilder.DropTable(
                name: "RegionDLStatOverviews");
        }
    }
}
