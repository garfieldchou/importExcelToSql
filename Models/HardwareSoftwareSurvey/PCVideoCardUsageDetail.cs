using System;

namespace SteamData.HardwareSoftwareSurvey
{
  public class PCVideoCardUsageDetail
  {
    public int PCVideoCardUsageDetailId { get; set; }
    public int Year { get; set; }
    public int Month { get; set; }
    public string Category { get; set; }
    public string Item { get; set; }
    public decimal Percentage { get; set; }

  }
}