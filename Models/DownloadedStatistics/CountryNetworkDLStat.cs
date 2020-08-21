using System;

namespace SteamData.DownloadedStatistics
{
  public class CountryNetworkDLStat
  {
    public int CountryNetworkDLStatId { get; set; }

    public DateTime Time { get; set; }
    public string Country { get; set; }
    public string Network { get; set; }
    public decimal AvgDlSpeedMbps { get; set; }
    public int CountryListId { get; set; }
    public virtual CountryList CountryList { get; set; }

  }
}