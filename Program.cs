using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using McMaster.Extensions.CommandLineUtils;
using SteamData;
using SteamData.Utils;

namespace importSteamToSql {
  [Command (Name = "importSteamToSql", Description = "Import Steam data from xlsx to MS SQL server")]
  [HelpOption ("-?")]
  [VersionOptionFromMember ("-v|--version", MemberName = nameof (GetVersion))]

  class Program {
    static void Main (string[] args) {
      try {
        Trace.Listeners.Add (new TextWriterTraceListener (
          File.CreateText ("log.txt")));
        Trace.AutoFlush = true;

        CommandLineApplication.Execute<Program> (args);
      } catch (System.Exception ex) {
        Trace.WriteLine ($"{ex.GetType()}: {ex.Message}");
        Debug.WriteLine ($"{ex.StackTrace}");
      }
    }

    [Argument (0, Description = "Specify xlsx file to be imported or the directory containing those files\n default: current directory")]
    private string fileOrDirtPath { get; } = Directory.GetCurrentDirectory ();

    [Option ("-w|--watch", Description = "Watch new files and import")]
    private bool watchMode { get; } = false;

    private void OnExecute (CommandLineApplication app, CancellationToken cancellationToken = default) {
      Trace.WriteLine ($"Version {GetVersion()}");
      using (var steamDb = new SteamDataContext ()) {
        Trace.WriteLine ($"Send a query for Db connection test...");
        int count = (
          from d in steamDb.DetailsGames
          let dt = d.DetailsGameId
          orderby dt descending select dt).Count ();
        Trace.WriteLine ($"DetailsGames current count: {count}\n");
      }

      Debug.WriteLine ($"watch mode: {watchMode}");
      if (Directory.Exists (fileOrDirtPath)) {
        ImportSteamFromDirectory (fileOrDirtPath);
      } else {
        ImportXlsxFile (fileOrDirtPath);
      }
    }

    static void ImportSteamFromDirectory (string dirt) {
      var fileInfo = EnumerateXlsxFiles (dirt);
      Trace.WriteLine ($"{fileInfo.Count} files to be handled...");

      foreach (var file in fileInfo.Files) {
        ImportXlsxFile (file);
      }

      // Print out the xlsx files left in the target directory to check if anything is missed.
      var processedFilesInfo = EnumerateXlsxFiles (dirt);
      Trace.WriteLine ($"{processedFilesInfo.Count} xlsx file(s) in target directory");

      if (processedFilesInfo.Count > 0) {
        foreach (var file in processedFilesInfo.Files) {
          Trace.WriteLine ($"{file}");
        }
      }
    }

    static void ImportXlsxFile (string fileName) {
      Trace.WriteLine ($"Handling {fileName}");

      if (!File.Exists (fileName)) {
        Trace.WriteLine ($"{fileName} does not exist.");
        return;
      }

      if (!new Regex (@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_(\d{8}).xlsx$")
        .IsMatch (fileName)) {
        Trace.WriteLine ($"{fileName} is not supported.");
        return;
      }
      Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

      using (var steamDb = new SteamDataContext ()) {
        steamDb.ImportSteamDataFrom (fileName);
      }
    }

    private string GetVersion () => typeof (Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ().InformationalVersion;

    static (int Count, IEnumerable<string> Files) EnumerateXlsxFiles (string directory) {
      var files = Directory.EnumerateFiles (directory, "*.xlsx", SearchOption.AllDirectories)
        .Where (file => !file.Contains ("Processed"))
        .OrderBy (file => file);
      return (Count: files.Count (), Files: files);
    }
  }
}