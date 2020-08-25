using System;
using System.Data;
using System.Text.RegularExpressions;
using SteamData.Utils;
using static System.Console;

namespace SteamData.HardwareSoftwareSurvey
{
  public class HardwareSoftwareSurveyUtils : ExcelContent
  {
    public static void ImportHWSurvey(SteamDataContext db)
    {
      var hwsSurvey = content.Tables[0];
      string category = string.Empty;

      for (int i = 1; i < hwsSurvey.Rows.Count; i++)
      {
        DataRow survey = hwsSurvey.Rows[i];
        if (survey[1].ToString() != "")
        {
          category = (string)survey[1];
          continue;
        }
        string item = survey[2].ToString();

        db.HWSurveys.Add(new HWSurvey
        {
          Year = reportDate.Year,
          Month = reportDate.Month,
          WorkWeek = reportDate.GetIso8601WeekOfYear(),
          Day = reportDate.Day,
          Time = reportDate,
          Category = category +
            (new Regex(@"^(Windows|OSX|Linux)$").IsMatch(item)
              && "OS Version" == category
              ? "_SubTotal" : ""),
          Item = item,
          Percentage = Decimal.Parse(survey[3].ToString().Split('%')[0])
        });
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported");
    }
  }
}