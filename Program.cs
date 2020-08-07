using System.Text;
using static System.Console;
using SteamData;
using SteamData.Utils;
using static SteamData.DownloadedStatistics.DownloadedStatisticsUtils;

namespace importSteamToSql
{
  class Program
  {
    static void Main(string[] args)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

      var result = CommonUtils.GetExcelContent("DownloadedStatistics_20200601.xlsx");

      WriteLine(result.Tables);
      // The result of each spreadsheet is in result.Tables
      using (var steamDb = new SteamDataContext())
      {
        ImportCountryList(steamDb, result);
      }

    }
  }
}