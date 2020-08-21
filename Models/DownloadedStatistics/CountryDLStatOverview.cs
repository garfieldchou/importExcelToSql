using System;

namespace SteamData.DownloadedStatistics
{
  public class CountryDLStatOverview
  {
    public int CountryDLStatOverviewId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public DateTime Time { get; set; }
    public string Country { get; set; }
    public decimal TotalTb { get; set; }
    public decimal AvgDlSpeedMbps { get; set; }
    public decimal SteamPercent { get; set; }
    public int CountryListId { get; set; }
    public virtual CountryList CountryList { get; set; }
  }
}