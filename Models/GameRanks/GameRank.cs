using System;

namespace SteamData.GameRanks {
  public class GameRank {
    public int GameRankId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public DateTime Time { get; set; }
    public int Ranks { get; set; }
    public int Players { get; set; }
    public int Peak { get; set; }
    public string Game { get; set; }
    public string Links { get; set; }
    public int DetailsGameId { get; set; }
    public virtual DetailsGame DetailsGame { get; set; }

  }
}