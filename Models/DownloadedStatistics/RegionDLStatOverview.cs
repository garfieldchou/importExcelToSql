namespace SteamData.DownloadedStatistics
{
  public class RegionDLStatOverview
  {
    public int RegionDLStatOverviewId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public string Region { get; set; }
    public string Average { get; set; }
    public string Max { get; set; }
  }
}