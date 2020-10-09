using System;
using Microsoft.EntityFrameworkCore;
using SteamData.DownloadedStatistics;
using SteamData.GameRanks;
using SteamData.HardwareSoftwareSurvey;
using static DotNetEnv.Env;

namespace SteamData {
  public class SteamDataContext : DbContext {
    // Downloaded_Statistics
    public DbSet<CountryList> CountryLists { get; set; }
    public DbSet<RegionDLStatOverview> RegionDLStatOverviews { get; set; }
    public DbSet<RegionDLStatDetail> RegionDLStatDetails { get; set; }
    public DbSet<CountryDLStatOverview> CountryDLStatOverviews { get; set; }
    public DbSet<CountryNetworkDLStat> CountryNetworkDLStats { get; set; }

    // Game_Ranks
    public DbSet<OnlineStat> OnlineStats { get; set; }
    public DbSet<GameRank> GameRanks { get; set; }
    public DbSet<DetailsGame> DetailsGames { get; set; }
    public DbSet<DetailsGamesReviewerHistory> DetailsGamesReviewerHistory { get; set; }

    // Hardware_Software_Survey
    public DbSet<HWSurvey> HWSurveys { get; set; }
    public DbSet<PCVideoCardUsageDetail> PCVideoCardUsageDetails { get; set; }
    public DbSet<DirectXOS> DirectXOSs { get; set; }
    public DbSet<ProceUsageDetail> ProceUsageDetails { get; set; }
    public DbSet<PcPhyCpuDetail> PcPhyCpuDetails { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
      Load ();

      string connectionString =
        GetBool ("DB_INTEGRATED_SECURITY") &&
        GetString ("DOTNET_ENV") is "prod" ?
        string.Format ("Server={0};Database={1};Integrated Security={2};MultipleActiveResultSets={3}",
          GetString ("DB_SERVER"),
          GetString ("DB_NAME"),
          GetBool ("DB_INTEGRATED_SECURITY"),
          GetBool ("DB_MULTI_ACTIVE_RES_SET")
        ) :
        string.Format ("Server={0};Database={1};Integrated Security={2};User ID={3};Password={4};MultipleActiveResultSets={5}",
          GetString ("DB_SERVER"),
          GetString ("DB_NAME"),
          GetBool ("DB_INTEGRATED_SECURITY"),
          GetString ("DB_SERVER_USER"),
          GetString ("DB_SERVER_PASSWORD"),
          GetBool ("DB_MULTI_ACTIVE_RES_SET")
        );

      optionsBuilder.UseSqlServer (connectionString);
    }
    protected override void OnModelCreating (
      ModelBuilder modelBuilder) {
      modelBuilder.Entity<CountryList> ()
        .HasIndex (countryList => countryList.Country)
        .IsUnique ();

      modelBuilder.Entity<CountryDLStatOverview> ()
        .Property (st => st.TotalTb)
        .HasColumnType ("decimal(18,9)");

      modelBuilder.Entity<CountryDLStatOverview> ()
        .Property (st => st.AvgDlSpeedMbps)
        .HasColumnType ("decimal(18,3)");

      modelBuilder.Entity<DetailsGame> ()
        .HasData (new DetailsGame {
          DetailsGameId = 1,
            Game = string.Empty,
            GameDescription = string.Empty,
            RecentReviews = string.Empty,
            AllReviews = string.Empty,
            ReleaseDate = DateTime.MinValue,
            HotTags = string.Empty,
            SystemRequirements = string.Empty,
        });
    }

  }
}