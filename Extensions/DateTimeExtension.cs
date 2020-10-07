using System;
using System.Globalization;

namespace SteamData.Utils {
  internal static class DateTimeExtension {
    public static int GetIso8601WeekOfYear (this DateTime time) {
      // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll
      // be the same week# as whatever Thursday, Friday or Saturday are,
      // and we always get those right
      DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek (time);
      if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday) {
        time = time.AddDays (3);
      }

      // Return the week of our adjusted day
      return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear (time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    public static void Deconstruct (this DateTime dateTime,
        out DateTime fullDateTime,
        out int year, out int month, out int day,
        out DateTime date, out TimeSpan timeOfDay, out int workWeek) =>
      (fullDateTime, year, month,
        day, date, timeOfDay, workWeek) =
      (dateTime, dateTime.Year, dateTime.Month,
        dateTime.Day, dateTime.Date, dateTime.TimeOfDay,
        dateTime.GetIso8601WeekOfYear ());
  }
}