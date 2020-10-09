using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using SteamData.Utils;

namespace SteamData.DownloadedStatistics {
  public sealed class DownloadedStatisticsContent : ExcelContent, ICheckDuplicateHandling {
    public DownloadedStatisticsContent (string filename) : base (filename) { }
    private Dictionary<string, int> CountryIdMapping { get; } = new Dictionary<string, int> ();
    private void ExportCountryListTo (SteamDataContext db) {
      int affected = 0;

      foreach (DataTable table in Content.Tables) {
        if (table.TableName is "BandWidth Data") continue;

        string country = table.Rows[1][1].ToString ();

        if (!db.CountryLists.Any (c => c.Country == country)) {
          db.CountryLists.Add (new CountryList {
            Country = country
          });
        }
      }
      affected += db.SaveChanges ();

      Trace.WriteLine (($"{nameof (CountryList),-28}| import {affected,6:N0} items"));

      // Populate the latest Country-ID mapping for the future uses
      foreach (var country in db.CountryLists) {
        CountryIdMapping.TryAdd (country.Country, country.CountryListId);
      }
    }
    private void ExportRegionDLStatDetailTo (SteamDataContext db) {
      var bwDetails = Content.Tables[0];
      int columnCount = bwDetails.Columns.Count;

      DateTime[] detail = (
        from d in db.RegionDLStatDetails
        let dt = d.Full_DateTime
        orderby dt descending select dt).ToArray ();

      DateTime latestInDB = detail.Length > 0 ? detail[0] : DateTime.MinValue;

      foreach (DataRow row in bwDetails.Rows) {
        // skip the header
        if (row[0].ToString () is "") continue;
        // ignore the summary
        if (row[columnCount - 1].ToString () is "") break;

        for (int i = 0; i < columnCount / 4; i++) {
          var (dateTime, year, month, day, date, timeOfDay, workWeek) =
          DateTime.Parse (row[4 * i + 2].ToString ());

          if (dateTime > latestInDB) {
            db.RegionDLStatDetails.Add (new RegionDLStatDetail {
              Year = year, Month = month, WorkWeek = workWeek,
                Day = day, Date = date.ToString ("MM/dd"), Time = timeOfDay,
                Full_DateTime = dateTime, Country = row[4 * i + 1].ToString (),
                BandWidthGbps = Int32.Parse (row[4 * i + 3].ToString ())
            });
          }
        }
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (RegionDLStatDetail),-28}| import {affected,6:N0} items"));

      // aggregate before save changes
      var query = db.RegionDLStatDetails
        .Where (dt => dt.Full_DateTime.Date >= latestInDB.Date)
        .GroupBy (dt => new { dt.Year, dt.Month, dt.WorkWeek, dt.Day, dt.Country },
          dt => dt.BandWidthGbps,
          (key, bw) => new RegionDLStatOverview {
            Year = key.Year,
              Month = key.Month,
              WorkWeek = key.WorkWeek,
              Day = key.Day,
              Region = key.Country,
              Average = bw.Average (),
              Max = bw.Max ()
          });

      foreach (var result in query) {
        var overview = db.RegionDLStatOverviews.FirstOrDefault (ov =>
          ov.Year == result.Year &&
          ov.Month == result.Month &&
          ov.Day == result.Day &&
          ov.Region == result.Region);

        if (overview is null)
          db.RegionDLStatOverviews.Add (result);
        else {
          overview.Average = result.Average;
          overview.Max = result.Max;
        }
      }
      affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (RegionDLStatOverview),-28}| import {affected,6:N0} items"));
    }
    private void ExportCountryDLStatOverviewTo (SteamDataContext db) {
      foreach (DataTable table in Content.Tables) {
        if (table.TableName is "BandWidth Data") continue;

        string country = (string) table.Rows[1][1];
        if (!CountryIdMapping.TryGetValue (country, out int countryId)) {
          Trace.WriteLine ($"{country} is not found in CountryList");
        }

        var dlStat = new CountryDLStatOverview {
          Year = ReportYear,
          Month = ReportMonth,
          WorkWeek = ReportWeek,
          Day = ReportDay,
          Time = ReportDate,
          TotalTb = ConvertTotalBytesTB (table.Rows[4][1].ToString ()),
          AvgDlSpeedMbps = ConvertDlSpeedToMB (table.Rows[4][2].ToString ()),
          SteamPercent = Decimal.Parse (((string) (table.Rows[4][3])).Split ('%') [0]),
          CountryListId = countryId
        };

        db.CountryDLStatOverviews.Add (dlStat);
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (CountryDLStatOverview),-28}| import {affected,6:N0} items"));
    }
    private void ExportCountryNetworkDLStatTo (SteamDataContext db) {
      foreach (DataTable table in Content.Tables) {
        if (table.TableName is "BandWidth Data") continue;

        string country = (string) table.Rows[1][1];
        if (!CountryIdMapping.TryGetValue (country, out int countryId)) {
          Trace.WriteLine ($"{country} is not found in CountryList");
        }

        for (int i = 9; i < table.Rows.Count; i++) {
          string network = table.Rows[i][1].ToString ();
          var networkDlStat = new CountryNetworkDLStat {
            Time = ReportDate,
            Network = network,
            AvgDlSpeedMbps = Decimal.Parse (((string) (table.Rows[i][2])).Split (' ') [0]),
            CountryListId = countryId
          };
          db.CountryNetworkDLStats.Add (networkDlStat);
        }
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (($"{nameof (CountryNetworkDLStat),-28}| import {affected,6:N0} items"));
    }

    bool ICheckDuplicateHandling.IsHandledBefore (SteamDataContext targetDb) =>
      targetDb.CountryDLStatOverviews.Any (o => o.Time == ReportDate);

    public override void ExportTo (SteamDataContext db) {
      if (!((ICheckDuplicateHandling) this).IsHandledBefore (db)) {
        ExportCountryListTo (db);
        ExportRegionDLStatDetailTo (db);
        ExportCountryDLStatOverviewTo (db);
        ExportCountryNetworkDLStatTo (db);
      } else {
        Trace.WriteLine ($"Skip importing file handled before");
      }
    }

    private decimal ConvertTotalBytesTB (string totalBytes) {
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

    private decimal ConvertDlSpeedToMB (string speed) {
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
  }
}