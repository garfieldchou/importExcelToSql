using System.Data;
using System.IO;
using System.Reflection;
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
  [VersionOptionFromMember ("-v|--version", MemberName = nameof (GetVersion))]

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
      var files = Directory.EnumerateFiles (dirt, "*.xlsx", SearchOption.AllDirectories)
        .Where (file => !file.Contains ("Processed"))
        .OrderBy (file => file);
      WriteLine ($"{files.Count()} files to be handled...");

      foreach (var item in files) {
        ImportXlsxFile (item);
      }
    }

    static void ImportXlsxFile (string fileName) {
      WriteLine ($"Handling {fileName}");

      if (!File.Exists (fileName)) {
        WriteLine ($"{fileName} does not exist.");
        return;
      }

      if (!new Regex (@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_(\d{8}).xlsx$")
        .IsMatch (fileName)) {
        WriteLine ($"{fileName} is not supported.");
        return;
      }
      Encoding.RegisterProvider (CodePagesEncodingProvider.Instance);

      using (var steamDb = new SteamDataContext ()) {
        steamDb.ImportSteamDataFrom (fileName);
      }
    }

    private static string GetVersion () => typeof (Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ().InformationalVersion;
  }
}