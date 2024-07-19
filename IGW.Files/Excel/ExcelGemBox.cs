using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GemBox.Spreadsheet;
using System.Threading.Tasks;
using System.Data;
using System.Web;
using System.IO;
using Newtonsoft.Json;

namespace ItchGamingLibrary.Files.Excel
{
    public class GemBoxFileOutPut
    {
        public string FileName { get; private set; }
        public string FileType { get; private set; }
        public GemBoxFileOutPut(string fileName, string fileType)
        {
            FileName = fileName;
            FileType = fileType;
        }
    }

    public class ExcelGemBox
    {
        public static byte[] GetBytes(ExcelFile file, SaveOptions options)
        {
            using (var stream = new MemoryStream())
            {
                file.Save(stream, options);
                return stream.ToArray();
            }
        }

        public static SaveOptions GetSaveOptions(string format)
        {
            switch (format.ToUpperInvariant())
            {
                case "XLSX":
                    return SaveOptions.XlsxDefault;
                case "XLS":
                    return SaveOptions.XlsxDefault;
                case "ODS":
                    return SaveOptions.OdsDefault;
                case "CSV":
                    return SaveOptions.CsvDefault;
                case "HTML":
                    return SaveOptions.HtmlDefault;
                case "PDF":
                    return SaveOptions.PdfDefault;
                case "XPS":
                    return SaveOptions.XpsDefault;
                case "BMP":
                    return new ImageSaveOptions() { Format = ImageSaveFormat.Bmp };
                case "GIF":
                    return new ImageSaveOptions() { Format = ImageSaveFormat.Gif };
                case "JPG":
                    return new ImageSaveOptions() { Format = ImageSaveFormat.Jpeg };
                case "PNG":
                    return new ImageSaveOptions() { Format = ImageSaveFormat.Png };
                case "TIF":
                    return new ImageSaveOptions() { Format = ImageSaveFormat.Tiff };
                case "WMP":
                    return new ImageSaveOptions() { Format = ImageSaveFormat.Wmp };
                default:
                    throw new NotSupportedException("Format '" + format + "' is not supported.");
            }
        }
        public DataTable WordSheetToDataTable(DataTable dt, HttpPostedFileBase fileExcel, string InfoExcelImport)
        {
            SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
            var workbook = new ExcelFile();
            dynamic Osc = Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(InfoExcelImport);
            string extFile = Path.GetExtension(fileExcel.FileName).Substring(1).ToLower();
            if(extFile == "xlsx")
            {
                workbook = ExcelFile.Load(fileExcel.InputStream, LoadOptions.XlsxDefault);
            }
            else
            {
                workbook = ExcelFile.Load(fileExcel.InputStream, LoadOptions.XlsDefault);
            }
            string ExcelSheet = Osc.ExcelSheet.ToString();
            string ExcelFirstRow = Osc.ExcelFirstRow.ToString();

            var worksheet = workbook.Worksheets[ExcelSheet];
            int countRowExcelFile = worksheet.Rows.Count;
            int rowStart;
            int.TryParse(ExcelFirstRow, out rowStart);
            rowStart = (rowStart == 0 ? 5 : rowStart);
            if(rowStart > countRowExcelFile)
            {
                return null;
            }
            //Create DataTable from an Excel worksheet
            var dataTable = worksheet.CreateDataTable(new CreateDataTableOptions()
            {
                ColumnHeaders = false,
                StartRow = rowStart,
                NumberOfColumns = worksheet.Columns.Count,
                NumberOfRows = worksheet.Rows.Count - 1,
                Resolution = ColumnTypeResolution.AutoPreferStringCurrentCulture
            });

            //worksheet.ExtractToDataTable(dt, dataTable, ExtractDataOptions.StopAtFirstEmptyRow, ws.Rows[0], ws.Columns[0]);
            for(int i = 0; i< dt.Rows.Count; i++)
            {
                var item = dt.Columns[i];
                dataTable.Columns[i].ColumnName = item.ColumnName;
            }
            return dt;
        }
    }

    public class ExcelItem
    {
        //Tieu de cua cot Header
        public string HeaderName { get; set; }
        //Do dai cua cot
        public double HeaderWidth { get; set; }
        //Thuoc tinh lay du lieu trong IEnumerable<Object> Data truyen vao
        public string ColumnNameData { get; set; }
        public ExcelItem(string name, double width, string nameData) {
            HeaderName = name;
            HeaderWidth = width;
            ColumnNameData = nameData;
        }
    } 
}
