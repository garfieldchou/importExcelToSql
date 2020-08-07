using System;
using System.Data;
using static System.Console;
using SteamData.Utils;

namespace SteamData.DownloadedStatistics
{
  public static class DownloadedStatisticsUtils
  {
    public static void ImportCountryList(SteamDataContext db, DataSet dataSet)
    {
      foreach (var item in dataSet.Tables)
      {
        try
        {
          if (item.ToString() == "BandWidth Data") continue;

          var country = new CountryList
          {
            Country = item.ToString()
          };
          db.CountryLists.Add(country);
          db.SaveChanges();
        }
        catch (System.Exception ex)
        {
          if (((Microsoft.Data.Sqlite.SqliteException)ex.InnerException).SqliteErrorCode == 19)
          {
            WriteLine($"{item.ToString(),32} already exists.");
          }
        }
      }
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
            WorkWeek = CommonUtils.GetIso8601WeekOfYear(dateTime),
            Day = dateTime.Day,
            Time = dateTime,
            Country = row[4 * i + 1].ToString(),
            BandWidthGbps = Int32.Parse(row[4 * i + 3].ToString())
          };

          db.RegionDLStatDetails.Add(dlDetail);
        }
      }
      db.SaveChanges();
    }

  }

}