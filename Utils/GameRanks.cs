using System;
using System.Data;
using static System.Console;
using SteamData.Utils;

namespace SteamData.GameRanks
{
  public static class GameRanksUtils
  {
    public static void ImportOnlineStat(SteamDataContext db, DataSet dataSet)
    {
      var onlineStat = dataSet.Tables[0];

      for (int i = 4; i < onlineStat.Rows.Count; i++)
      {
        DateTime dateTime = DateTime.Parse(onlineStat.Rows[i][1].ToString());
        int players = Int32.Parse(onlineStat.Rows[i][2].ToString());

        db.OnlineStats.Add(new OnlineStat
        {
          Year = dateTime.Year,
          Month = dateTime.Month,
          WorkWeek = dateTime.GetIso8601WeekOfYear(),
          Day = dateTime.Day,
          Time = dateTime.TimeOfDay,
          DateTime = dateTime,
          Players = players
        });
      }
      db.SaveChanges();
    }
  }
}