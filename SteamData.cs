using System;
using Microsoft.EntityFrameworkCore;
using SteamData.DownloadedStatistics;
using SteamData.GameRanks;
using SteamData.HardwareSoftwareSurvey;

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

    // Hardware_Software_Survey
    public DbSet<HWSurvey> HWSurveys { get; set; }
    public DbSet<PCVideoCardUsageDetail> PCVideoCardUsageDetails { get; set; }
    public DbSet<DirectXOS> DirectXOSs { get; set; }
    public DbSet<ProceUsageDetail> ProceUsageDetails { get; set; }
    public DbSet<PcPhyCpuDetail> PcPhyCpuDetails { get; set; }

    protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder) {
      DotNetEnv.Env.Load ();

      string connectionString =
        DotNetEnv.Env.GetBool ("DB_INTEGRATED_SECURITY") &&
        DotNetEnv.Env.GetString ("DOTNET_ENV") == "prod" ?
        string.Format ("Server={0};Database={1};Integrated Security={2};MultipleActiveResultSets={3}",
          DotNetEnv.Env.GetString ("DB_SERVER"),
          DotNetEnv.Env.GetString ("DB_NAME"),
          DotNetEnv.Env.GetBool ("DB_INTEGRATED_SECURITY"),
          DotNetEnv.Env.GetBool ("DB_MULTI_ACTIVE_RES_SET")
        ) :
        string.Format ("Server={0};Database={1};Integrated Security={2};User ID={3};Password={4};MultipleActiveResultSets={5}",
          DotNetEnv.Env.GetString ("DB_SERVER"),
          DotNetEnv.Env.GetString ("DB_NAME"),
          DotNetEnv.Env.GetBool ("DB_INTEGRATED_SECURITY"),
          DotNetEnv.Env.GetString ("DB_SERVER_USER"),
          DotNetEnv.Env.GetString ("DB_SERVER_PASSWORD"),
          DotNetEnv.Env.GetBool ("DB_MULTI_ACTIVE_RES_SET")
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