namespace SteamData.DownloadedStatistics {
  public class RegionDLStatOverview {
    public int RegionDLStatOverviewId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public string Region { get; set; }
    public decimal Average { get; set; }
    public decimal Max { get; set; }
  }
}