using System;

namespace SteamData.GameRanks {
  public class DetailsGamesReviewerHistory {
    public int DetailsGamesReviewerHistoryId { get; set; }
    public int DetailsGameId { get; set; }
    public virtual DetailsGame DetailsGame { get; set; }
    public int RecordYear { get; set; }
    public int RecordMonth { get; set; }
    public int RecordWorkWeek { get; set; }
    public DateTime DateTime { get; set; }
    public string RecentReviews { get; set; }
    public string AllReviews { get; set; }
    public int? RecentReviewsPosCount { get; set; }
    public decimal? RecentReviewsPosPercent { get; set; }
    public int? AllReviewsPosCount { get; set; }
    public decimal? AllReviewsPosPercent { get; set; }
  }
}