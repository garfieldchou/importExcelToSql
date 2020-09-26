using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using SteamData.Utils;
using static System.Globalization.NumberStyles;

namespace SteamData.GameRanks {
  public sealed class GameRanksUtils : ExcelContent, IDisposable, ICheckDuplicateHandling {
    public GameRanksUtils (string filename) : base (filename) { }

    private void ImportOnlineStat (SteamDataContext db) {
      var onlineStat = Content.Tables[0];

      DateTime[] stat = (
        from d in db.OnlineStats
        let dt = d.DateTime
        orderby dt descending select dt).ToArray ();

      DateTime latestStatInDB = stat.Length > 0 ? stat[0] : DateTime.MinValue;

      for (int i = 4; i < onlineStat.Rows.Count; i++) {
        DateTime dateTime = DateTime.Parse (onlineStat.Rows[i][1].ToString ());

        if (dateTime > latestStatInDB) {
          db.OnlineStats.Add (new OnlineStat {
            Year = dateTime.Year,
              Month = dateTime.Month,
              WorkWeek = dateTime.GetIso8601WeekOfYear (),
              Day = dateTime.Day,
              Time = dateTime.TimeOfDay,
              DateTime = dateTime,
              Players = Int32.Parse (onlineStat.Rows[i][2].ToString ())
          });
        }
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "OnlineStat", affected));
    }

    private void ImportGameRanks (SteamDataContext db) {
      var gameRank = Content.Tables[1];
      var gameDetailsDict = new Dictionary<string, int> ();
      IQueryable<DetailsGame> gameDetails = db.DetailsGames;

      foreach (var detail in gameDetails) {
        gameDetailsDict.Add (detail.Game, detail.DetailsGameId);
      }

      for (int i = 1; i < gameRank.Rows.Count; i++) {
        int rank = Int32.Parse (gameRank.Rows[i][0].ToString ()) + 1;
        int players = Int32.Parse (gameRank.Rows[i][1].ToString (), AllowThousands);
        int peak = Int32.Parse (gameRank.Rows[i][2].ToString (), AllowThousands);
        string game = gameRank.Rows[i][3].ToString ();
        string link = gameRank.Columns.Count == 5 ?
          gameRank.Rows[i][4].ToString () :
          string.Empty;

        if (!gameDetailsDict.TryGetValue (game, out int gameId)) gameId = 1;

        db.GameRanks.Add (new GameRank {
          Year = ReportYear,
            Month = ReportMonth,
            WorkWeek = ReportWeek,
            Day = ReportDay,
            Time = ReportDate,
            Ranks = rank,
            Players = players,
            Peak = peak,
            Game = game,
            DetailsGameId = gameId,
            Links = link
        });
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "GameRank", affected));
    }

    private void ImportDetailsGame (SteamDataContext db) {
      var gameDetails = Content.Tables[2];
      for (int i = 1; i < gameDetails.Rows.Count; i++) {
        DataRow detail = gameDetails.Rows[i];
        DateTime releaseDate = DateTime.Parse (detail[4].ToString ());
        string game = detail[0].ToString ();
        string description = detail[1].ToString ();
        string recentReviews = detail[2].ToString ();
        string allReviews = detail[3].ToString ();
        string gameLink = gameDetails.Columns.Count == 8 ? detail[7].ToString () : string.Empty;
        string hotTags = detail[5].ToString ();
        string systemRequirements = detail[6].ToString ();

        DetailsGame detailGame = db.DetailsGames.FirstOrDefault (d => d.Game == game);

        if (detailGame == default (DetailsGame)) {
          db.DetailsGames.Add (new DetailsGame {
            Game = game,
              GameDescription = description,
              RecentReviews = recentReviews,
              AllReviews = allReviews,
              ReleaseDate = releaseDate,
              HotTags = hotTags,
              SystemRequirements = systemRequirements,
              Links = gameLink
          });
        } else {
          detailGame.GameDescription = description;
          detailGame.RecentReviews = recentReviews;
          detailGame.AllReviews = allReviews;
          detailGame.ReleaseDate = releaseDate;
          detailGame.HotTags = hotTags;
          detailGame.SystemRequirements = systemRequirements;
          detailGame.Links = gameLink;
        }
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "DetailsGame", affected));
    }

    private void ImportDetailsGamesReviewerHistory (SteamDataContext db) {
      var gameDetailsDict = new Dictionary<string, int> ();
      foreach (var detail in db.DetailsGames) {
        gameDetailsDict.Add (detail.Game, detail.DetailsGameId);
      }

      var gameDetails = Content.Tables[2];
      for (int i = 1; i < gameDetails.Rows.Count; i++) {
        DataRow detail = gameDetails.Rows[i];
        DateTime releaseDate = DateTime.Parse (detail[4].ToString ());
        string game = detail[0].ToString ();

        if (!gameDetailsDict.TryGetValue (game, out int gameId)) gameId = 1;

        db.DetailsGamesReviewerHistory.Add (new DetailsGamesReviewerHistory {
          DetailsGameId = gameId,
            RecordYear = ReportYear,
            RecordMonth = ReportMonth,
            RecordWorkWeek = ReportWeek,
            DateTime = ReportDate,
            RecentReviews = detail[2].ToString (),
            AllReviews = detail[3].ToString ()
        });
      }
      int affected = db.SaveChanges ();
      Trace.WriteLine (string.Format ("{0,-28}| import {1,6:N0} items", "DetailsGamesReviewerHistory", affected));
    }

    public bool IsHandledBefore (SteamDataContext targetDb) => targetDb.DetailsGamesReviewerHistory.Any (o => o.DateTime == ReportDate);

    public override void ExportTo (SteamDataContext db) {
      if (!IsHandledBefore (db)) {
        ImportOnlineStat (db);
        ImportDetailsGame (db);
        ImportDetailsGamesReviewerHistory (db);
        ImportGameRanks (db);
      } else {
        Trace.WriteLine ($"Skip importing file handled before");
      }
    }

    public void Dispose () { }
  }
}