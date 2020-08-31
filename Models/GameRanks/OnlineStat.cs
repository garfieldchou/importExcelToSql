using System;

namespace SteamData.GameRanks {
  public class OnlineStat {
    public int OnlineStatId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public TimeSpan Time { get; set; }
    public DateTime DateTime { get; set; }
    public int Players { get; set; }

  }
}