using System;
using System.Data;
using static System.Globalization.NumberStyles;
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

    public static void ImportGameRanks(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      var gameRank = dataSet.Tables[1];

      for (int i = 1; i < gameRank.Rows.Count; i++)
      {
        int rank = Int32.Parse(gameRank.Rows[i][0].ToString());
        int players = Int32.Parse(gameRank.Rows[i][1].ToString(), AllowThousands);
        int peak = Int32.Parse(gameRank.Rows[i][2].ToString(), AllowThousands);
        string game = gameRank.Rows[i][3].ToString();

        db.GameRanks.Add(new GameRank
        {
          Year = reportDate.Year,
          Month = reportDate.Month,
          WorkWeek = reportDate.GetIso8601WeekOfYear(),
          Day = reportDate.Day,
          Time = reportDate,
          Ranks = rank,
          Players = players,
          Peak = peak,
          Game = game
        });
      }
      db.SaveChanges();
    }
  }
}