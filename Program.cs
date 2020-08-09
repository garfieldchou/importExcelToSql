using System;
using System.Text;
using static System.Console;
using SteamData;
using SteamData.Utils;
using static SteamData.DownloadedStatistics.DownloadedStatisticsUtils;
using static SteamData.GameRanks.GameRanksUtils;
using static SteamData.HardwareSoftwareSurvey.HardwareSoftwareSurveyUtils;

namespace importSteamToSql
{
  class Program
  {
    static void Main(string[] args)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

      DateTime reportDate = DateTime.Parse("DownloadedStatistics_20200601.xlsx".Split(new[] { '_', '.' })[1].ToDateString());

      // var result = CommonUtils.GetExcelContent("DownloadedStatistics_20200601.xlsx");
      // var result = CommonUtils.GetExcelContent("Game Ranks_20200601.xlsx");
      var result = CommonUtils.GetExcelContent("HWSSurvey_20200601.xlsx");

      using (var steamDb = new SteamDataContext())
      {
        // DownloadedStatistics
        // ImportCountryList(steamDb, result);
        // ImportRegionDLStatDetail(steamDb, result);
        // ImportCountryDLStatOverview(steamDb, result, reportDate);
        // ImportCountryNetworkDLStat(steamDb, result, reportDate);

        // GameRanks
        // ImportOnlineStat(steamDb, result);
        // ImportGameRanks(steamDb, result, reportDate);
        // ImportDetailsGame(steamDb, result);

        // HardwareSoftwareSurvey
        ImportHWSurvey(steamDb, result, reportDate);
      }
    }
  }
}