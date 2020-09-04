using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using ExcelDataReader;
using SteamData.DownloadedStatistics;
using SteamData.GameRanks;
using SteamData.HardwareSoftwareSurvey;

namespace SteamData.Utils {
  public class ExcelContent {
    protected DataSet content { get; set; }
    public DateTime reportDate { get; set; }
    public void GetExcelContent (string filename) {
      reportDate = DateTime.ParseExact (
        new Regex (@"\w+_(\d{8}).xlsx$")
        .Match (filename).Groups[1].Captures[0]
        .ToString (),
        "yyyyMMdd", null);

      using (var stream = File.Open (filename, FileMode.Open, FileAccess.Read)) {
        using (var reader = ExcelReaderFactory.CreateReader (stream)) {
          content = reader.AsDataSet (new ExcelDataSetConfiguration () {
            // Gets or sets a value indicating whether to set the DataColumn.DataType 
            // property in a second pass.
            UseColumnDataType = true,

              // Gets or sets a callback to determine whether to include the current sheet
              // in the DataSet. Called once per sheet before ConfigureDataTable.
              FilterSheet = (tableReader, sheetIndex) => true,

              // Gets or sets a callback to obtain configuration options for a DataTable. 
              ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration () {
                // Gets or sets a value indicating the prefix of generated column names.
                EmptyColumnNamePrefix = "Column",

                  // Gets or sets a value indicating whether to use a row from the 
                  // data as column names.
                  UseHeaderRow = false,

                  // Gets or sets a callback to determine which row is the header row. 
                  // Only called when UseHeaderRow = true.
                  ReadHeaderRow = (rowReader) => {
                    // F.ex skip the first row and use the 2nd row as column headers:
                    rowReader.Read ();
                  },

                  // Gets or sets a callback to determine whether to include the 
                  // current row in the DataTable.
                  FilterRow = (rowReader) => {
                    return true;
                  },

                  // Gets or sets a callback to determine whether to include the specific
                  // column in the DataTable. Called once per column after reading the 
                  // headers.
                  FilterColumn = (rowReader, columnIndex) => {
                    return true;
                  }
              }
          });
        }
      }
    }
  }
  public static class CommonUtils {
    public static int GetIso8601WeekOfYear (this DateTime time) {
      // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
      // be the same week# as whatever Thursday, Friday or Saturday are,
      // and we always get those right
      DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek (time);
      if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
        time = time.AddDays (3);
      }

      // Return the week of our adjusted day
      return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear (time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    public static decimal ConvertTotalBytesTB (this string totalBytes) {
      string[] numberAndUnit = totalBytes.Split (' ');
      decimal bytesNumber = Decimal.Parse (numberAndUnit[0]);
      string unit = numberAndUnit[1];

      return unit
      switch {
        "PB" => bytesNumber * (decimal) Math.Pow (10, 3),
          "TB" => bytesNumber,
          "GB" => bytesNumber * (decimal) Math.Pow (10, -3),
          "MB" => bytesNumber * (decimal) Math.Pow (10, -6),
          "kB" => bytesNumber * (decimal) Math.Pow (10, -9),
          _ => -1M
      };
    }

    public static decimal ConvertDlSpeedToMB (this string speed) {
      string[] numberAndUnit = speed.Split (' ');
      decimal bytesNumber = Decimal.Parse (numberAndUnit[0]);
      string unit = numberAndUnit[1];

      return unit
      switch {
        "Gbps" => bytesNumber * (decimal) Math.Pow (10, 3),
          "Mbps" => bytesNumber,
          "kbps" => bytesNumber * (decimal) Math.Pow (10, -3),
          _ => -1M
      };
    }

    public static int MonthStringToInt (this string month) => new Dictionary<string, int> { { "JAN", 1 },
      { "FEB", 2 },
      { "MAR", 3 },
      { "APR", 4 },
      { "MAY", 5 },
      { "JUN", 6 },
      { "JUL", 7 },
      { "AUG", 8 },
      { "SEP", 9 },
      { "OCT", 10 },
      { "NOV", 11 },
      { "DEC", 12 }
    }[month];
  }

  public static class DbContextExtensions {
    public static void ImportDownloadedStatistics (this SteamDataContext db, string fileName) {
      using (var downloadedStatistics = new DownloadedStatisticsUtils (fileName)) {
        downloadedStatistics.ImportCountryList (db);
        downloadedStatistics.ImportRegionDLStatDetail (db);
        downloadedStatistics.ImportCountryDLStatOverview (db);
        downloadedStatistics.ImportCountryNetworkDLStat (db);
      }
    }
    public static void ImportGameRank (this SteamDataContext db, string fileName) {
      using (var gameRank = new GameRanksUtils (fileName)) {
        gameRank.ImportOnlineStat (db);
        gameRank.ImportDetailsGame (db);
        gameRank.ImportGameRanks (db);
      }
    }
    public static void ImportHardwareSoftwareSurvey (this SteamDataContext db, string fileName) {
      using (var hardwareSoftwareSurvey = new HardwareSoftwareSurveyUtils (fileName)) {
        hardwareSoftwareSurvey.ImportHWSurvey (db);
        hardwareSoftwareSurvey.ImportPCVideoCardUsageDetail (db);
        hardwareSoftwareSurvey.ImportDirectXOS (db);
        hardwareSoftwareSurvey.ImportProceUsageDetail (db);
        hardwareSoftwareSurvey.ImportPcPhyCpuDetail (db);
      }
    }
  }
}