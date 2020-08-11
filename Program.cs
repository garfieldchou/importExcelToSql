using System;
using System.Text;
using System.Data;
using static System.IO.File;
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
      if (args.Length != 1)
      {
        WriteLine("Usage: importSteamToSql.exe xxx.xlsx");
        return;
      }

      if (!Exists(args[0]))
      {
        WriteLine("File does not exist.");
        return;
      }
      else
      {
        ImportSteamData(args[0]);
      }
    }

    static void ImportSteamData(string fileName)
    {
      Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
      DateTime reportDate = DateTime.ParseExact(fileName.Split(new[] { '_', '.' })[1], "yyyyMMdd", null);
      var result = CommonUtils.GetExcelContent(fileName);

      using (var steamDb = new SteamDataContext())
      {
        if (fileName.StartsWith("DownloadedStatistics"))
        {
          ImportDownloadedStatistics(steamDb, result, reportDate);
        }
        else if (fileName.StartsWith("Game Ranks"))
        {
          ImportGameRank(steamDb, result, reportDate);
        }
        else
        {
          ImportHardwareSoftwareSurvey(steamDb, result, reportDate);
        }
      }
    }

    static void ImportDownloadedStatistics(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      ImportCountryList(db, dataSet);
      ImportRegionDLStatDetail(db, dataSet);
      ImportCountryDLStatOverview(db, dataSet, reportDate);
      ImportCountryNetworkDLStat(db, dataSet, reportDate);
    }

    static void ImportGameRank(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      ImportOnlineStat(db, dataSet);
      ImportGameRanks(db, dataSet, reportDate);
      ImportDetailsGame(db, dataSet);
    }

    static void ImportHardwareSoftwareSurvey(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      ImportHWSurvey(db, dataSet, reportDate);
    }
  }
}