using System.Text.RegularExpressions;
using SteamData.DownloadedStatistics;
using SteamData.GameRanks;
using SteamData.HardwareSoftwareSurvey;
using SteamData.Utils;

namespace SteamData {
  public static class SteamDataFactory {
    public static ExcelContent GetStreamDataFrom (string fileName) {
      string category =
        new Regex (@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_\d{8}.xlsx$")
        .Match (fileName).Groups[1].Captures[0]
        .ToString ();

      return category
      switch {
        "DownloadedStatistics" => new DownloadedStatisticsContent (fileName),
          "GameRanks" => new GameRanksContent (fileName),
          var c when c is "Hardware_Software" ||
            c is "HWSSurvey" => new HardwareSoftwareSurveyContent (fileName),
            _ => null
      };
    }
  }
}