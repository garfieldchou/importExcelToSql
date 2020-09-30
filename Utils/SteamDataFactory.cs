using System.Text.RegularExpressions;
using SteamData.DownloadedStatistics;
using SteamData.GameRanks;
using SteamData.HardwareSoftwareSurvey;
using SteamData.Utils;

namespace SteamData {
  public static class SteamDataFactory {
    public static ExcelContent GetStreamData (string fileName) {
      string category =
        new Regex (@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_\d{8}.xlsx$")
        .Match (fileName).Groups[1].Captures[0]
        .ToString ();

      return category
      switch {
        "DownloadedStatistics" => new DownloadedStatisticsUtils (fileName),
          "GameRanks" => new GameRanksUtils (fileName),
          var c when c == "Hardware_Software" ||
            c == "HWSSurvey" => new HardwareSoftwareSurveyUtils (fileName),
            _ => null
      };
    }
  }
}