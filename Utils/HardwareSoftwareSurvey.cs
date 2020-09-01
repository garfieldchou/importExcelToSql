using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using SteamData.Utils;
using static System.Console;

namespace SteamData.HardwareSoftwareSurvey {
  public class HardwareSoftwareSurveyUtils : ExcelContent {
    public static void ImportHWSurvey (SteamDataContext db) {
      var hwsSurvey = content.Tables[0];
      string category = string.Empty;

      for (int i = 1; i < hwsSurvey.Rows.Count; i++) {
        DataRow survey = hwsSurvey.Rows[i];
        if (survey[1].ToString () != "") {
          category = (string) survey[1];
          continue;
        }
        string item = survey[2].ToString ();

        db.HWSurveys.Add (new HWSurvey {
          Year = reportDate.Year,
            Month = reportDate.Month,
            WorkWeek = reportDate.GetIso8601WeekOfYear (),
            Day = reportDate.Day,
            Time = reportDate,
            Category = category +
            (new Regex (@"^(Windows|OSX|Linux)$").IsMatch (item) &&
              "OS Version" == category ?
              "_SubTotal" : ""),
            Item = item,
            Percentage = Decimal.Parse (survey[3].ToString ().Split ('%') [0])
        });
      }
      int affected = db.SaveChanges ();
      WriteLine ($"{affected} items are imported");
    }

    public static void ImportPCVideoCardUsageDetail (SteamDataContext db) {
      var usageDetails = content.Tables[1];
      int monthStart = usageDetails.Rows[0][2].ToString ().MonthStringToInt ();
      int startYear = reportDate.Year;
      string category = usageDetails.Rows[0][1].ToString ();
      int latestYearInDb = int.MinValue;
      int latestYearMonthInDb = int.MinValue;

      if (monthStart > 8) startYear -= 1;

      var detailInDb = (
        from d in db.PCVideoCardUsageDetails orderby d.Year descending, d.Month descending select d).FirstOrDefault ();

      if (detailInDb != null) {
        latestYearInDb = detailInDb.Year;
        latestYearMonthInDb = detailInDb.Month;
      }

      WriteLine ($"{ latestYearInDb }, { latestYearMonthInDb }");

      WriteLine ($"**********");
      WriteLine ($"{category}");
      WriteLine ($"**********");

      for (int i = 1; i < usageDetails.Rows.Count; i++) {
        DataRow detail = usageDetails.Rows[i];

        if (detail[0].ToString () == string.Empty) {
          if (detail[1].ToString () != string.Empty) {
            category = detail[1].ToString ();
            WriteLine ($"**********");
            WriteLine ($"{category}");
            WriteLine ($"**********");
          }
          continue;
        }
        string item = detail[1].ToString ();

        for (int col = 2; col < 7; col++) {
          decimal percentage;
          if (!Decimal.TryParse (
              detail[col].ToString ().Split ('%') [0],
              NumberStyles.AllowDecimalPoint,
              CultureInfo.CurrentCulture,
              out percentage)) continue;

          int month = monthStart + col - 2;
          int year = month > 12 ? startYear + 1 : startYear;
          month = month > 12 ? month - 12 : month;

          if (year < latestYearInDb ||
            (year == latestYearInDb && month <= latestYearMonthInDb)) continue;

          WriteLine ($"row {i}: {year}-{month}-{item}-{percentage}");

          db.PCVideoCardUsageDetails.Add (new PCVideoCardUsageDetail {
            Year = year,
              Month = month,
              Category = category,
              Item = item,
              Percentage = percentage
          });
        }
        WriteLine ($"==============");
      }
      int affected = db.SaveChanges ();
      WriteLine ($"{affected} PCVideoCardUsageDetail are imported");
    }

    public static void ImportDirectXOS (SteamDataContext db) {
      var usageDetails = content.Tables[2];
      int monthStart = usageDetails.Rows[0][2].ToString ().MonthStringToInt ();
      int startYear = reportDate.Year;
      string category = usageDetails.Rows[0][1].ToString ();
      int latestYearInDb = int.MinValue;
      int latestYearMonthInDb = int.MinValue;

      if (monthStart > 8) startYear -= 1;

      var detailInDb = (
        from d in db.DirectXOSs orderby d.Year descending, d.Month descending select d).FirstOrDefault ();

      if (detailInDb != null) {
        latestYearInDb = detailInDb.Year;
        latestYearMonthInDb = detailInDb.Month;
      }

      WriteLine ($"{ latestYearInDb }, { latestYearMonthInDb }");

      WriteLine ($"**********");
      WriteLine ($"{category}");
      WriteLine ($"**********");

      for (int i = 1; i < usageDetails.Rows.Count; i++) {
        DataRow detail = usageDetails.Rows[i];

        if (detail[0].ToString () == string.Empty) {
          if (detail[1].ToString () != string.Empty) {
            category = detail[1].ToString ();
            WriteLine ($"**********");
            WriteLine ($"{category}");
            WriteLine ($"**********");
          }
          continue;
        }
        string item = detail[1].ToString ();

        for (int col = 2; col < 7; col++) {
          decimal percentage;
          if (!Decimal.TryParse (
              detail[col].ToString ().Split ('%') [0],
              NumberStyles.AllowDecimalPoint,
              CultureInfo.CurrentCulture,
              out percentage)) continue;

          int month = monthStart + col - 2;
          int year = month > 12 ? startYear + 1 : startYear;
          month = month > 12 ? month - 12 : month;

          if (year < latestYearInDb ||
            (year == latestYearInDb && month <= latestYearMonthInDb)) continue;

          WriteLine ($"row {i}: {year}-{month}-{item}-{percentage}");

          db.DirectXOSs.Add (new DirectXOS {
            Year = year,
              Month = month,
              Category = category,
              Item = item,
              Percentage = percentage
          });
        }
        WriteLine ($"==============");
      }
      int affected = db.SaveChanges ();
      WriteLine ($"{affected} DirectXOS are imported");
    }
  }
}