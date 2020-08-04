using System;

namespace SteamData.GameRanks
{
  public class GameRank
  {
    public int GameRankId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public TimeSpan Time { get; set; }
    public int Ranks { get; set; }
    public int Players { get; set; }
    public int Peak { get; set; }
    public string Game { get; set; }

  }
}