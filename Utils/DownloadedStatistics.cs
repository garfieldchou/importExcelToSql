using System.Data;
using static System.Console;

namespace SteamData.DownloadedStatistics
{
  public static class DownloadedStatisticsUtils
  {
    public static void ImportCountryList(DataSet dataSet)
    {
      foreach (var item in dataSet.Tables)
      {
        WriteLine(item);
      }
    }
  }
}