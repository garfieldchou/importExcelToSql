using System;
using System.Text;
using System.Text.RegularExpressions;
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

      string fileName = args[0];
      var fileNameChecker = new Regex(@"(DownloadedStatistics|Game Ranks|HWSSurvey)_(\d{8}).xlsx$");
      Match match = fileNameChecker.Match(fileName);

      if (match == Match.Empty)
      {
        WriteLine("File is not supported.");
        return;
      }
      else
      {
        if (!Exists(fileName))
        {
          WriteLine("File does not exist.");
          return;
        }
        else
        {
          string category = match.Groups[1].Captures[0].ToString();
          DateTime reportDate = DateTime.ParseExact(match.Groups[2].Captures[0].ToString(), "yyyyMMdd", null);

          Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
          var content = CommonUtils.GetExcelContent(fileName);

          ImportSteamData(category, reportDate, content);
        }
      }
    }

    static void ImportSteamData(string category, DateTime reportDate, DataSet content)
    {
      using (var steamDb = new SteamDataContext())
      {
        switch (category)
        {
          case "DownloadedStatistics":
            ImportDownloadedStatistics(steamDb, content, reportDate);
            break;
          case "Game Ranks":
            ImportGameRank(steamDb, content, reportDate);
            break;
          case "HWSSurvey":
            ImportHardwareSoftwareSurvey(steamDb, content, reportDate);
            break;
          default:
            WriteLine("Category not found.");
            break;
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
      ImportDetailsGame(db, dataSet);
      ImportGameRanks(db, dataSet, reportDate);
    }

    static void ImportHardwareSoftwareSurvey(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      ImportHWSurvey(db, dataSet, reportDate);
    }
  }
}