using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using SteamData.Utils;

namespace SteamData.HardwareSoftwareSurvey {
  public sealed class HardwareSoftwareSurveyContent : ExcelContent, ICheckDuplicateHandling {
    public HardwareSoftwareSurveyContent (string filename) : base (filename) { }

    private void ExportHWSurveyTo (SteamDataContext db) {
      var hwsSurvey = Content.Tables[0];
      string category = string.Empty;

      for (int i = 1; i < hwsSurvey.Rows.Count; i++) {
        DataRow survey = hwsSurvey.Rows[i];
        if (survey[1].ToString () != "") {
          category = (string) survey[1];
          continue;
        }
        string item = survey[2].ToString ();

        db.HWSurveys.Add (new HWSurvey {
          Year = ReportYear,
            Month = ReportMonth,
            WorkWeek = ReportWeek,
            Day = ReportDay,
            Time = ReportDate,
            Category = category +
            (new Regex (@"^(Windows|OSX|Linux)$").IsMatch (item) &&
              category is "OS Version" ?
              "_SubTotal" : ""),
            Item = item,
            Percentage = Decimal.Parse (survey[3].ToString ().Split ('%') [0])
        });
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (HWSurvey),-28}| import {affected,6:N0} items"));
    }

    private void ExportPCVideoCardUsageDetailTo (SteamDataContext db) {
      ImportUsageDetail (db.PCVideoCardUsageDetails, Content.Tables[1]);
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (PCVideoCardUsageDetail),-28}| import {affected,6:N0} items"));
    }

    private void ExportDirectXOSTo (SteamDataContext db) {
      ImportUsageDetail (db.DirectXOSs, Content.Tables[2]);
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (DirectXOS),-28}| import {affected,6:N0} items"));
    }

    private void ExportProceUsageDetailTo (SteamDataContext db) {
      ImportUsageDetail (db.ProceUsageDetails, Content.Tables[3]);
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (ProceUsageDetail),-28}| import {affected,6:N0} items"));
    }

    private void ExportPcPhyCpuDetailTo (SteamDataContext db) {
      ImportUsageDetail (db.PcPhyCpuDetails, Content.Tables[4]);
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (PcPhyCpuDetail),-28}| import {affected,6:N0} items"));
    }

    private void ImportUsageDetail<T> (DbSet<T> set, DataTable usageDetails)
    where T : class, ISurveyDetail, new () {

      int monthStart = MonthStringToInt (usageDetails.Rows[0][2].ToString ());
      int startYear = ReportYear;
      string category = usageDetails.Rows[0][1].ToString ();

      if (monthStart > 8) startYear -= 1;

      var detailInDb = (
        from d in set orderby d.Year descending, d.Month descending select d).FirstOrDefault ();

      int latestYearInDb = detailInDb?.Year?? int.MinValue;
      int latestYearMonthInDb = detailInDb?.Month?? int.MinValue;

      for (int i = 1; i < usageDetails.Rows.Count; i++) {
        DataRow detail = usageDetails.Rows[i];

        if (detail[0].ToString () == string.Empty) {
          if (detail[1].ToString () != string.Empty) {
            category = detail[1].ToString ();
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

          set.Add (new T {
            Year = year,
              Month = month,
              Category = category,
              Item = item,
              Percentage = percentage
          });
        }
      }
    }

    bool ICheckDuplicateHandling.IsHandledBefore (SteamDataContext targetDb) =>
      targetDb.HWSurveys.Any (o => o.Time == ReportDate);

    public override void ExportTo (SteamDataContext db) {
      if (!((ICheckDuplicateHandling) this).IsHandledBefore (db)) {
        ExportHWSurveyTo (db);
        ExportPCVideoCardUsageDetailTo (db);
        ExportDirectXOSTo (db);
        ExportProceUsageDetailTo (db);
        ExportPcPhyCpuDetailTo (db);
      } else {
        Trace.WriteLine ($"Skip importing file handled before");
      }
    }

    private int MonthStringToInt (string month) => new Dictionary<string, int> { { "JAN", 1 },
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
}