using System;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using static System.Console;
using System.Linq;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using SteamData;
using SteamData.Utils;

namespace importSteamToSql {
  [Command (Name = "importSteamToSql", Description = "Import Steam data from xlsx to MS SQL server")]
  [HelpOption ("-?")]
  class Program {
    static void Main (string[] args) => CommandLineApplication.Execute<Program> (args);

    [Argument (0, Description = "Specify xlsx file to be imported or the directory containing those files\n default: current directory")]
    private string fileOrDirtPath { get; } = Directory.GetCurrentDirectory ();

    [Option ("-w|--watch", Description = "Watch new files and import")]
    private bool watchMode { get; } = false;

    private void OnExecute (CommandLineApplication app, CancellationToken cancellationToken = default) {
      WriteLine ($"watch mode: {watchMode}");
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
          // ExcelContent.GetExcelContent (fileName, reportDate);

          // var downloadStatistics = new DownloadStatistics(DataSet ds);
          // downloadStatistics.ImportTo(db);

          ImportSteamData (category, fileName, reportDate);
        }
      }
    }

    static void ImportSteamData (string category, string filename, DateTime reportDate) {
      using (var steamDb = new SteamDataContext ()) {
        switch (category) {
          case "DownloadedStatistics":
            steamDb.ImportDownloadedStatistics (filename, reportDate);
            break;
          case "GameRanks":
            // steamDb.ImportGameRank ();
            break;
          case "HWSSurvey":
          case "Hardware_Software":
            // steamDb.ImportHardwareSoftwareSurvey ();
            break;
          default:
            WriteLine ("Category not found.");
            break;
        }
      }
    }
  }
}