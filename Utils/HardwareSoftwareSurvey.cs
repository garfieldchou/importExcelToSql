using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SteamData.Utils;
using static System.Console;

namespace SteamData.HardwareSoftwareSurvey {
  public class HardwareSoftwareSurveyUtils : ExcelContent, IDisposable {
    public HardwareSoftwareSurveyUtils (string filename) {
      GetExcelContent (filename);
    }

    private void ImportHWSurvey (SteamDataContext db) {
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
      WriteLine (string.Format ("{0,-24}| import {1,6:N0} items", "HWSurvey", affected));
    }

    private void ImportPCVideoCardUsageDetail (SteamDataContext db) {
      ImportUsageDetail (db.PCVideoCardUsageDetails, content.Tables[1]);
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-24}| import {1,6:N0} items", "PCVideoCardUsageDetail", affected));
    }

    private void ImportDirectXOS (SteamDataContext db) {
      ImportUsageDetail (db.DirectXOSs, content.Tables[2]);
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-24}| import {1,6:N0} items", "DirectXOS", affected));
    }

    private void ImportProceUsageDetail (SteamDataContext db) {
      ImportUsageDetail (db.ProceUsageDetails, content.Tables[3]);
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-24}| import {1,6:N0} items", "ProceUsageDetail", affected));
    }

    private void ImportPcPhyCpuDetail (SteamDataContext db) {
      ImportUsageDetail (db.PcPhyCpuDetails, content.Tables[4]);
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-24}| import {1,6:N0} items", "PcPhyCpuDetail", affected));
    }

    private void ImportUsageDetail<T> (DbSet<T> set, DataTable usageDetails)
    where T : class, ISurveyDetail, new () {

      int monthStart = usageDetails.Rows[0][2].ToString ().MonthStringToInt ();
      int startYear = reportDate.Year;
      string category = usageDetails.Rows[0][1].ToString ();
      int latestYearInDb = int.MinValue;
      int latestYearMonthInDb = int.MinValue;

      if (monthStart > 8) startYear -= 1;

      var detailInDb = (
        from d in set orderby d.Year descending, d.Month descending select d).FirstOrDefault ();

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
          if (!Decimal.TryParse (
              detail[col].ToString ().Split ('%') [0],
              NumberStyles.AllowDecimalPoint,
              CultureInfo.CurrentCulture,
              out decimal percentage)) continue;

          int month = monthStart + col - 2;
          int year = month > 12 ? startYear + 1 : startYear;
          month = month > 12 ? month - 12 : month;

          if (year < latestYearInDb ||
            (year == latestYearInDb && month <= latestYearMonthInDb)) continue;

          WriteLine ($"row {i}: {year}-{month}-{item}-{percentage}");

          set.Add (new T {
            Year = year,
              Month = month,
              Category = category,
              Item = item,
              Percentage = percentage
          });
        }
        WriteLine ($"==============");
      }
    }

    public override void ImportTo (SteamDataContext db) {
      ImportHWSurvey (db);
      ImportPCVideoCardUsageDetail (db);
      ImportDirectXOS (db);
      ImportProceUsageDetail (db);
      ImportPcPhyCpuDetail (db);
    }

    public void Dispose () { }
  }
}