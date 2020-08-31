using System;

namespace SteamData.HardwareSoftwareSurvey {
  public class HWSurvey {
    public int HWSurveyId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public int WorkWeek { get; set; }
    public int Day { get; set; }
    public DateTime Time { get; set; }
    public string Category { get; set; }
    public string Item { get; set; }
    public decimal Percentage { get; set; }

  }
}