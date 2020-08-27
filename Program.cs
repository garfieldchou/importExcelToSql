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
      var fileNameChecker = new Regex(@"(DownloadedStatistics|GameRanks|HWSSurvey)_(\d{8}).xlsx$");
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
          ExcelContent.GetExcelContent(fileName, reportDate);

          ImportSteamData(category);
        }
      }
    }

    static void ImportSteamData(string category)
    {
      using (var steamDb = new SteamDataContext())
      {
        switch (category)
        {
          case "DownloadedStatistics":
            ImportDownloadedStatistics(steamDb);
            break;
          case "GameRanks":
            ImportGameRank(steamDb);
            break;
          case "HWSSurvey":
            ImportHardwareSoftwareSurvey(steamDb);
            break;
          default:
            WriteLine("Category not found.");
            break;
        }
      }
    }

    static void ImportDownloadedStatistics(SteamDataContext db)
    {
      ImportCountryList(db);
      ImportRegionDLStatDetail(db);
      ImportCountryDLStatOverview(db);
      ImportCountryNetworkDLStat(db);
    }

    static void ImportGameRank(SteamDataContext db)
    {
      ImportOnlineStat(db);
      ImportDetailsGame(db);
      ImportGameRanks(db);
    }

    static void ImportHardwareSoftwareSurvey(SteamDataContext db)
    {
      ImportHWSurvey(db);
    }
  }
}