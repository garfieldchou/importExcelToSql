using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
using System.Linq;
using System.Threading;
using SteamData;
using SteamData.Utils;
using static SteamData.DownloadedStatistics.DownloadedStatisticsUtils;
using static SteamData.GameRanks.GameRanksUtils;
using static SteamData.HardwareSoftwareSurvey.HardwareSoftwareSurveyUtils;
using McMaster.Extensions.CommandLineUtils;

namespace importSteamToSql {
  [Command (Name = "importSteamToSql", Description = "Import Steam data from xlsx to MS SQL server")]
  [HelpOption ("-?")]
  class Program {
    static void Main (string[] args) => CommandLineApplication.Execute<Program> (args);

    [Argument (0, Description = "xlsx file to be imported or the path containing those files")]
    private string fileOrDirtPath { get; } = Directory.GetCurrentDirectory ();

    private void OnExecute (CommandLineApplication app, CancellationToken cancellationToken = default) {
      try {
        ImportSteamFromDirectory (fileOrDirtPath);
      } catch (Exception) {
        ImportXlsxFile (fileOrDirtPath);
      }
    }

    static void ImportSteamFromDirectory (string dirt) {
      var files = Directory.EnumerateFiles (dirt, "*.xlsx", SearchOption.AllDirectories).OrderBy (file => file);
      WriteLine ($"{files.Count()} files to be handled...");

      foreach (var item in files) {
        ImportXlsxFile (item);
      }
    }

    static void ImportXlsxFile (string fileName) {
      WriteLine ($"Handling {fileName}");
      var fileNameChecker = new Regex (@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_(\d{8}).xlsx$");
      Match match = fileNameChecker.Match (fileName);

      if (match == Match.Empty) {
        WriteLine ("File is not supported.");
        return;
      } else {
        if (!File.Exists (fileName)) {
          WriteLine ("File does not exist.");
          return;
        } else {
          string category = match.Groups[1].Captures[0].ToString ();
          DateTime reportDate = DateTime.ParseExact (match.Groups[2].Captures[0].ToString (), "yyyyMMdd", null);

          Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);
          ExcelContent.GetExcelContent (fileName, reportDate);

          ImportSteamData (category);
        }
      }
    }

    static void ImportSteamData (string category) {
      using (var steamDb = new SteamDataContext ()) {
        switch (category) {
          case "DownloadedStatistics":
            ImportDownloadedStatistics (steamDb);
            break;
          case "GameRanks":
            ImportGameRank (steamDb);
            break;
          case "HWSSurvey":
          case "Hardware_Software":
            ImportHardwareSoftwareSurvey (steamDb);
            break;
          default:
            WriteLine ("Category not found.");
            break;
        }
      }
    }

    static void ImportDownloadedStatistics (SteamDataContext db) {
      ImportCountryList (db);
      ImportRegionDLStatDetail (db);
      ImportCountryDLStatOverview (db);
      ImportCountryNetworkDLStat (db);
    }

    static void ImportGameRank (SteamDataContext db) {
      ImportOnlineStat (db);
      ImportDetailsGame (db);
      ImportGameRanks (db);
    }

    static void ImportHardwareSoftwareSurvey (SteamDataContext db) {
      ImportHWSurvey (db);
      ImportPCVideoCardUsageDetail (db);
      ImportDirectXOS (db);
      ImportProceUsageDetail (db);
      ImportPcPhyCpuDetail (db);
    }
  }
}