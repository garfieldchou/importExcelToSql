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
      if (Directory.Exists (fileOrDirtPath)) {
        ImportSteamFromDirectory (fileOrDirtPath);
      } else {
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
      if (!File.Exists (fileName)) {
        WriteLine ($"{fileName} does not exist.");
        return;
      }

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

          Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);
          ExcelContent.GetExcelContent (fileName);

          ImportSteamData (category);
        }
      }
    }

    static void ImportSteamData (string category) {
      using (var steamDb = new SteamDataContext ()) {
        switch (category) {
          case "DownloadedStatistics":
            steamDb.ImportDownloadedStatistics ();
            break;
          case "GameRanks":
            steamDb.ImportGameRank ();
            break;
          case "HWSSurvey":
          case "Hardware_Software":
            steamDb.ImportHardwareSoftwareSurvey ();
            break;
          default:
            WriteLine ("Category not found.");
            break;
        }
      }
    }
  }
}