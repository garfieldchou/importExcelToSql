using System;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;
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
    static void ImportXlsxFile(string fileName)
    {
      var fileNameChecker = new Regex(@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_(\d{8}).xlsx$");
      Match match = fileNameChecker.Match(fileName);

      if (match == Match.Empty)
      {
        WriteLine("File is not supported.");
        return;
      }
      else
      {
        if (!File.Exists(fileName))
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

    static void Main(string[] args)
    {
      var files = Directory.EnumerateFiles("../spec/Steam_20200826", "*.xlsx", SearchOption.AllDirectories).OrderBy(file => file);

      foreach (var item in files)
      {
        WriteLine($"Handling {item}");
        ImportXlsxFile(item);
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
          case "Hardware_Software":
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