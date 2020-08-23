using System;
using System.Globalization;
using System.IO;
using System.Data;
using ExcelDataReader;
namespace SteamData.Utils
{
  public class ExcelContent
  {
    protected static DataSet content { get; set; }
    public static DateTime reportDate { get; set; }
    public static void GetExcelContent(string filename, DateTime date)
    {
      reportDate = date;

      using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
          content = reader.AsDataSet(new ExcelDataSetConfiguration()
          {
            // Gets or sets a value indicating whether to set the DataColumn.DataType 
            // property in a second pass.
            UseColumnDataType = true,

            // Gets or sets a callback to determine whether to include the current sheet
            // in the DataSet. Called once per sheet before ConfigureDataTable.
            FilterSheet = (tableReader, sheetIndex) => true,

            // Gets or sets a callback to obtain configuration options for a DataTable. 
            ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
            {
              // Gets or sets a value indicating the prefix of generated column names.
              EmptyColumnNamePrefix = "Column",

              // Gets or sets a value indicating whether to use a row from the 
              // data as column names.
              UseHeaderRow = false,

              // Gets or sets a callback to determine which row is the header row. 
              // Only called when UseHeaderRow = true.
              ReadHeaderRow = (rowReader) =>
              {
                // F.ex skip the first row and use the 2nd row as column headers:
                rowReader.Read();
              },

              // Gets or sets a callback to determine whether to include the 
              // current row in the DataTable.
              FilterRow = (rowReader) =>
              {
                return true;
              },

              // Gets or sets a callback to determine whether to include the specific
              // column in the DataTable. Called once per column after reading the 
              // headers.
              FilterColumn = (rowReader, columnIndex) =>
              {
                return true;
              }
            }
          });
        }
      }
    }
  }
  public static class CommonUtils
  {
    public static int GetIso8601WeekOfYear(this DateTime time)
    {
      // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
      // be the same week# as whatever Thursday, Friday or Saturday are,
      // and we always get those right
      DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
      if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
      {
        time = time.AddDays(3);
      }

      // Return the week of our adjusted day
      return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
    }

    public static decimal ConvertTotalBytesTB(this string totalBytes)
    {
      string[] numberAndUnit = totalBytes.Split(' ');
      decimal bytesNumber = Decimal.Parse(numberAndUnit[0]);
      string unit = numberAndUnit[1];

      return unit switch
      {
        "PB" => bytesNumber * (decimal)Math.Pow(10, 3),
        "TB" => bytesNumber,
        "GB" => bytesNumber * (decimal)Math.Pow(10, -3),
        "MB" => bytesNumber * (decimal)Math.Pow(10, -6),
        "kB" => bytesNumber * (decimal)Math.Pow(10, -9),
        _ => -1M
      };
    }
  }
}