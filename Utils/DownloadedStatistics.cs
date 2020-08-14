using System;
using System.Data;
using System.Linq;
using static System.Console;
using SteamData.Utils;

namespace SteamData.DownloadedStatistics
{
  public static class DownloadedStatisticsUtils
  {
    public static void ImportCountryList(SteamDataContext db, DataSet dataSet)
    {
      int affected = 0;

      foreach (DataTable item in dataSet.Tables)
      {
        try
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
        catch (System.Exception ex)
        {
          if (((Microsoft.Data.Sqlite.SqliteException)ex.InnerException).SqliteErrorCode == 19)
          {
            WriteLine($"{item.ToString(),32} already exists.");
          }
        }
      }
      affected += db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }
    public static void ImportRegionDLStatDetail(SteamDataContext db, DataSet dataSet)
    {
      var bwDetails = dataSet.Tables[0];
      int columnCount = bwDetails.Columns.Count;

      foreach (DataRow row in bwDetails.Rows)
      {
        // skip the header
        if (row[0].ToString() == "") continue;
        // ignore the summary
        if (row[columnCount - 1].ToString() == "") break;

        for (int i = 0; i < columnCount / 4; i++)
        {
          var dateTime = DateTime.Parse(row[4 * i + 2].ToString());

          var dlDetail = new RegionDLStatDetail
          {
            Year = dateTime.Year,
            Month = dateTime.Month,
            WorkWeek = dateTime.GetIso8601WeekOfYear(),
            Day = dateTime.Day,
            Time = dateTime,
            Country = row[4 * i + 1].ToString(),
            BandWidthGbps = Int32.Parse(row[4 * i + 3].ToString())
          };

          db.RegionDLStatDetails.Add(dlDetail);
        }
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported"); ;
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
          Country = country,
          TotalTb = table.Rows[4][1].ToString().ConvertTotalBytesTB(),
          AvgDlSpeedMbps = Decimal.Parse(((string)(table.Rows[4][2])).Split(' ')[0]),
          SteamPercent = Decimal.Parse(((string)(table.Rows[4][3])).Split('%')[0])
        };

        db.CountryDLStatOverviews.Add(dlStat);
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported"); ;
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
            Country = country,
            Network = network,
            AvgDlSpeedMbps = Decimal.Parse(((string)(table.Rows[i][2])).Split(' ')[0])
          };
          db.CountryNetworkDLStats.Add(networkDlStat);
        }
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported"); ;
    }
  }

}