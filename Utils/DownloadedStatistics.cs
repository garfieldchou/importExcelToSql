using System.Data;
using static System.Console;

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

  }

}