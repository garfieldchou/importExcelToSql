using System;
using System.Linq;
using System.Data;
using SteamData.Utils;
using System.Collections.Generic;
using static System.Globalization.NumberStyles;
using static System.Console;

namespace SteamData.GameRanks
{
  public static class GameRanksUtils
  {
    public static void ImportOnlineStat(SteamDataContext db, DataSet dataSet)
    {
      var onlineStat = dataSet.Tables[0];
      DateTime latestStatInDB = DateTime.MinValue;

      IQueryable<OnlineStat> stat =
        db.OnlineStats.OrderByDescending(s => s.DateTime).Take(1);
      if (stat.ToArray().Length > 0)
      {
        latestStatInDB = stat.Select(s => s.DateTime).ToArray()[0];
      }

      for (int i = 4; i < onlineStat.Rows.Count; i++)
      {
        DateTime dateTime = DateTime.Parse(onlineStat.Rows[i][1].ToString());

        if (dateTime > latestStatInDB)
        {
          db.OnlineStats.Add(new OnlineStat
          {
            Year = dateTime.Year,
            Month = dateTime.Month,
            WorkWeek = dateTime.GetIso8601WeekOfYear(),
            Day = dateTime.Day,
            Time = dateTime.TimeOfDay,
            DateTime = dateTime,
            Players = Int32.Parse(onlineStat.Rows[i][2].ToString())
          });
        }
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }

    public static void ImportGameRanks(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      var gameRank = dataSet.Tables[1];
      var gameDetailsDict = new Dictionary<string, int>();
      IQueryable<DetailsGame> gameDetails = db.DetailsGames;

      foreach (var detail in gameDetails)
      {
        gameDetailsDict.Add(detail.Game, detail.DetailsGameId);
      }

      for (int i = 1; i < gameRank.Rows.Count; i++)
      {
        int rank = Int32.Parse(gameRank.Rows[i][0].ToString()) + 1;
        int players = Int32.Parse(gameRank.Rows[i][1].ToString(), AllowThousands);
        int peak = Int32.Parse(gameRank.Rows[i][2].ToString(), AllowThousands);
        string game = gameRank.Rows[i][3].ToString();
        int gameId;

        if (!gameDetailsDict.TryGetValue(game, out gameId)) gameId = 1;

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
          Game = game,
          DetailsGameId = gameId
        });
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }

    public static void ImportDetailsGame(SteamDataContext db, DataSet dataSet)
    {
      var gameDetails = dataSet.Tables[2];
      for (int i = 1; i < gameDetails.Rows.Count; i++)
      {
        DataRow detail = gameDetails.Rows[i];
        DateTime releaseDate = DateTime.Parse(detail[4].ToString());

        string game = detail[0].ToString();
        if (!db.DetailsGames.Any(d => d.Game == game))
        {
          db.DetailsGames.Add(new DetailsGame
          {
            Game = game,
            GameDescription = detail[1].ToString(),
            RecentReviews = detail[2].ToString(),
            AllReviews = detail[3].ToString(),
            ReleaseDate = releaseDate,
            HotTags = detail[5].ToString(),
            SystemRequirements = detail[6].ToString()
          });
        }
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }
  }
}