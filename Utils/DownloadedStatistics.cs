using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using static System.Console;
using SteamData.Utils;

namespace SteamData.DownloadedStatistics {
  public class DownloadedStatisticsUtils : ExcelContent, IDisposable {
    public DownloadedStatisticsUtils (string filename) {
      GetExcelContent (filename);
    }
    private Dictionary<string, int> CountryIdMapping { get; set; } = new Dictionary<string, int> ();
    private void ImportCountryList (SteamDataContext db) {
      int affected = 0;

      foreach (DataTable item in content.Tables) {
        if (item.ToString () == "BandWidth Data") continue;

        string country = item.Rows[1][1].ToString ();

        if (!db.CountryLists.Any (c => c.Country == country)) {
          db.CountryLists.Add (new CountryList {
            Country = country
          });
        }
      }
      affected += db.SaveChanges ();

      WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "CountryList", affected));

      // Populate the latest Country-ID mapping for the future uses
      foreach (var country in db.CountryLists) {
        CountryIdMapping.TryAdd (country.Country, country.CountryListId);
      }
    }
    private void ImportRegionDLStatDetail (SteamDataContext db) {
      var bwDetails = content.Tables[0];
      int columnCount = bwDetails.Columns.Count;

      DateTime[] detail = (
        from d in db.RegionDLStatDetails
        let dt = d.Full_DateTime
        orderby dt descending select dt).ToArray ();

      DateTime latestInDB = detail.Length > 0 ? detail[0] : DateTime.MinValue;

      foreach (DataRow row in bwDetails.Rows) {
        // skip the header
        if (row[0].ToString () == "") continue;
        // ignore the summary
        if (row[columnCount - 1].ToString () == "") break;

        for (int i = 0; i < columnCount / 4; i++) {
          var dateTime = DateTime.Parse (row[4 * i + 2].ToString ());

          if (dateTime > latestInDB) {
            db.RegionDLStatDetails.Add (new RegionDLStatDetail {
              Year = dateTime.Year,
                Month = dateTime.Month,
                WorkWeek = dateTime.GetIso8601WeekOfYear (),
                Day = dateTime.Day,
                Date = dateTime.Date.ToString ("MM/dd"),
                Time = dateTime.TimeOfDay,
                Full_DateTime = dateTime,
                Country = row[4 * i + 1].ToString (),
                BandWidthGbps = Int32.Parse (row[4 * i + 3].ToString ())
            });
          }
        }
      }
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "RegionDLStatDetail", affected));

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

        if (overview == null)
          db.RegionDLStatOverviews.Add (result);
        else {
          overview.Average = result.Average;
          overview.Max = result.Max;
        }
      }
      affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "RegionDLStatOverview", affected));
    }
    private void ImportCountryDLStatOverview (SteamDataContext db) {
      foreach (DataTable table in content.Tables) {
        if ("BandWidth Data" == table.ToString ()) continue;

        string country = (string) table.Rows[1][1];
        if (!CountryIdMapping.TryGetValue (country, out int countryId)) {
          WriteLine ($"{country} is not found in CountryList");
        }

        var dlStat = new CountryDLStatOverview {
          Year = reportDate.Year,
          Month = reportDate.Month,
          WorkWeek = reportDate.GetIso8601WeekOfYear (),
          Day = reportDate.Day,
          Time = reportDate,
          TotalTb = ConvertTotalBytesTB (table.Rows[4][1].ToString ()),
          AvgDlSpeedMbps = ConvertDlSpeedToMB (table.Rows[4][2].ToString ()),
          SteamPercent = Decimal.Parse (((string) (table.Rows[4][3])).Split ('%') [0]),
          CountryListId = countryId
        };

        db.CountryDLStatOverviews.Add (dlStat);
      }
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "CountryDLStatOverview", affected));
    }
    private void ImportCountryNetworkDLStat (SteamDataContext db) {
      foreach (DataTable table in content.Tables) {
        if ("BandWidth Data" == table.ToString ()) continue;

        string country = (string) table.Rows[1][1];
        if (!CountryIdMapping.TryGetValue (country, out int countryId)) {
          WriteLine ($"{country} is not found in CountryList");
        }

        for (int i = 9; i < table.Rows.Count; i++) {
          string network = table.Rows[i][1].ToString ();
          var networkDlStat = new CountryNetworkDLStat {
            Time = reportDate,
            Network = network,
            AvgDlSpeedMbps = Decimal.Parse (((string) (table.Rows[i][2])).Split (' ') [0]),
            CountryListId = countryId
          };
          db.CountryNetworkDLStats.Add (networkDlStat);
        }
      }
      int affected = db.SaveChanges ();
      WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "CountryNetworkDLStat", affected));
    }

    public override void ImportTo (SteamDataContext db) {
      ImportCountryList (db);
      ImportRegionDLStatDetail (db);
      ImportCountryDLStatOverview (db);
      ImportCountryNetworkDLStat (db);
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

    public void Dispose () { }
  }
}