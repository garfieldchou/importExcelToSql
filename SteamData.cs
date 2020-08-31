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
      optionsBuilder.UseSqlServer (
        @"Server=APZA001GOD;Database=Steam;Integrated Security=True;MultipleActiveResultSets=true");
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