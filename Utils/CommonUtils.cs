using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using ExcelDataReader;
using SteamData.DownloadedStatistics;
using SteamData.GameRanks;
using SteamData.HardwareSoftwareSurvey;
using static System.IO.Path;

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

    public virtual void ImportTo (SteamDataContext db) { }
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
  }

  public static class DbContextExtensions {
    public static void ImportDownloadedStatistics (this SteamDataContext db, string fileName) {
      using (var downloadedStatistics = new DownloadedStatisticsUtils (fileName)) {
        downloadedStatistics.ImportTo (db);
      }
    }

    public static void ImportGameRank (this SteamDataContext db, string fileName) {
      using (var gameRank = new GameRanksUtils (fileName)) {
        gameRank.ImportTo (db);
      }
    }

    public static void ImportHardwareSoftwareSurvey (this SteamDataContext db, string fileName) {
      using (var hardwareSoftwareSurvey = new HardwareSoftwareSurveyUtils (fileName)) {
        hardwareSoftwareSurvey.ImportTo (db);
      }
    }

    public static void ImportSteamDataFrom (this SteamDataContext db, string fileName) {
      string category =
        new Regex (@"(DownloadedStatistics|GameRanks|HWSSurvey|Hardware_Software)_\d{8}.xlsx$")
        .Match (fileName).Groups[1].Captures[0]
        .ToString ();

      switch (category) {
        case "DownloadedStatistics":
          db.ImportDownloadedStatistics (fileName);
          break;
        case "GameRanks":
          db.ImportGameRank (fileName);
          break;
        case "HWSSurvey":
        case "Hardware_Software":
          db.ImportHardwareSoftwareSurvey (fileName);
          break;
        default:
          Trace.WriteLine ("Category not found.");
          break;
      }

      string targetDirectory = Combine (
        GetDirectoryName (fileName),
        "..", "Processed");

      string targetFileName = Combine (
        targetDirectory,
        GetFileName (fileName));

      if (!Directory.Exists (targetDirectory)) Directory.CreateDirectory (targetDirectory);

      Trace.WriteLine ($"Move\n{fileName}\nto\n{targetFileName}\n");
      try {
        File.Move (fileName, targetFileName);
      } catch (System.Exception ex) {
        Trace.WriteLine ($"{ex.GetType()}: {ex.Message}");
      }
    }
  }
}