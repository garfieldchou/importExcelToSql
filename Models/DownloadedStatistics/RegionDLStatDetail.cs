using System;

namespace SteamData.DownloadedStatistics {
  public class RegionDLStatDetail {
    public int RegionDLStatDetailId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public string Date { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime Full_DateTime { get; set; }
    public string Country { get; set; }
    public decimal BandWidthGbps { get; set; }
  }
}