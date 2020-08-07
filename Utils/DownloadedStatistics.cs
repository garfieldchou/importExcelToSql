using System.Data;
using System.IO;
using ExcelDataReader;
using static System.Console;

namespace SteamData.DownloadedStatistics
{
  public static class DownloadedStatisticsUtils
  {
    public static void ImportCountryList(SteamDataContext db, DataSet dataSet)
    {
      foreach (var item in dataSet.Tables)
      {
        if (item.ToString() == "BandWidth Data") continue;

        var country = new CountryList
        {
          Country = item.ToString()
        };
        db.CountryLists.Add(country);
      }
      db.SaveChanges();
    }
    public static DataSet GetExcelContent(string filename)
    {
      using (var stream = File.Open(filename, FileMode.Open, FileAccess.Read))
      {
        using (var reader = ExcelReaderFactory.CreateReader(stream))
        {
          var result = reader.AsDataSet(new ExcelDataSetConfiguration()
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

          return result;
        }
      }
    }
  }

}