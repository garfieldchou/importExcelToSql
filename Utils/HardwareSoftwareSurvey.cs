using System;
using System.Data;
using SteamData.Utils;
using static System.Console;

namespace SteamData.HardwareSoftwareSurvey
{
  public static class HardwareSoftwareSurveyUtils
  {
    public static void ImportHWSurvey(SteamDataContext db, DataSet dataSet, DateTime reportDate)
    {
      var hwsSurvey = dataSet.Tables[0];
      string category = string.Empty;

      for (int i = 1; i < hwsSurvey.Rows.Count; i++)
      {
        DataRow survey = hwsSurvey.Rows[i];
        if (survey[1].ToString() != "") category = (string)survey[1];

        db.HWSurveys.Add(new HWSurvey
        {
          Year = reportDate.Year,
          Month = reportDate.Month,
          WorkWeek = reportDate.GetIso8601WeekOfYear(),
          Day = reportDate.Day,
          Time = reportDate,
          Category = category,
          Item = survey[2].ToString(),
          Percentage = Decimal.Parse(survey[3].ToString().Split('%')[0])
        });
      }
      int affected = db.SaveChanges();
      WriteLine($"{affected} items are imported"); ;
    }
  }
}