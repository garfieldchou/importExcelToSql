using System;

namespace SteamData.GameRanks
{
  public class DetailsGame
  {
    public int DetailsGameId { get; set; }

    public string Game { get; set; }
    public string GameDescription { get; set; }
    public string RecentReviews { get; set; }
    public string AllReviews { get; set; }
    public DateTime ReleaseDate { get; set; }
    public string HotTags { get; set; }
    public string SystemRequirements { get; set; }

  }
}