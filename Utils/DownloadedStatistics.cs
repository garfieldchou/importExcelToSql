using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;
using static System.Console;
using SteamData.Utils;

namespace SteamData.DownloadedStatistics
{
  public static class DownloadedStatisticsUtils
  {
    private static Dictionary<string, int> CountryIdMapping { get; set; } = new Dictionary<string, int>();
    public static void ImportCountryList(SteamDataContext db, DataSet dataSet)
    {
      int affected = 0;

      foreach (DataTable item in dataSet.Tables)
      {
        if (item.ToString() == "BandWidth Data") continue;

        string country = item.Rows[1][1].ToString();

        if (!db.CountryLists.Any(c => c.Country == country))
        {
          db.CountryLists.Add(new CountryList
          {
            Country = country
          });
        }
      }
      affected += db.SaveChanges();

      WriteLine($"{affected} items are imported");

      // Populate the latest Country-ID mapping for the future uses
      foreach (var country in db.CountryLists)
      {
        CountryIdMapping.Add(country.Country, country.CountryListId);
      }
    }
    public static void ImportRegionDLStatDetail(SteamDataContext db, DataSet dataSet)
    {
      var bwDetails = dataSet.Tables[0];
      int columnCount = bwDetails.Columns.Count;

      DateTime[] detail = (
        from d in db.RegionDLStatDetails
        let dt = d.Full_DateTime
        orderby dt descending
        select dt).ToArray();

      DateTime latestInDB = detail.Length > 0 ? detail[0] : DateTime.MinValue;

      foreach (DataRow row in bwDetails.Rows)
      {
        // skip the header
        if (row[0].ToString() == "") continue;
        // ignore the summary
        if (row[columnCount - 1].ToString() == "") break;

        for (int i = 0; i < columnCount / 4; i++)
        {
          var dateTime = DateTime.Parse(row[4 * i + 2].ToString());

          if (dateTime > latestInDB)
          {
            db.RegionDLStatDetails.Add(new RegionDLStatDetail
            {
              Year = dateTime.Year,
              Month = dateTime.Month,
              WorkWeek = dateTime.GetIso8601WeekOfYear(),
              Day = dateTime.Day,
              Date = dateTime.Date.ToString("MM/dd"),
              Time = dateTime.TimeOfDay,
              Full_DateTime = dateTime,
              Country = row[4 * i + 1].ToString(),
              BandWidthGbps = Int32.Parse(row[4 * i + 3].ToString())
            });
          }
        }
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }
    public static void ImportCountryDLStatOverview(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      foreach (DataTable table in dataSet.Tables)
      {
        string country = table.ToString();
        if (country == "BandWidth Data") continue;

        var dlStat = new CountryDLStatOverview
        {
          Year = reportDate.Year,
          Month = reportDate.Month,
          WorkWeek = reportDate.GetIso8601WeekOfYear(),
          Day = reportDate.Day,
          Time = reportDate,
          TotalTb = table.Rows[4][1].ToString().ConvertTotalBytesTB(),
          AvgDlSpeedMbps = Decimal.Parse(((string)(table.Rows[4][2])).Split(' ')[0]),
          SteamPercent = Decimal.Parse(((string)(table.Rows[4][3])).Split('%')[0])
        };

        db.CountryDLStatOverviews.Add(dlStat);
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }
    public static void ImportCountryNetworkDLStat(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      foreach (DataTable table in dataSet.Tables)
      {
        string sheetName = table.ToString();
        if (sheetName == "BandWidth Data") continue;

        string country = (string)table.Rows[1][1];
        for (int i = 9; i < table.Rows.Count; i++)
        {
          string network = table.Rows[i][1].ToString();
          var networkDlStat = new CountryNetworkDLStat
          {
            Time = reportDate,
            Network = network,
            AvgDlSpeedMbps = Decimal.Parse(((string)(table.Rows[i][2])).Split(' ')[0])
          };
          db.CountryNetworkDLStats.Add(networkDlStat);
        }
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }
  }

}