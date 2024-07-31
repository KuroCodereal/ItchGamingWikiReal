using GemBox.Spreadsheet;
using IGW.Files.Excel;
using ItchGamingLibrary;
using ItchGamingLibrary.Files;
using ItchGamingLibrary.Files.Excel;
using ItchGamingLibrary.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using System.Windows;

namespace IGW.Files
{
    public class IGWFileExcel : IGWFile
    {
        public IGWUser User = new IGWUser();

        //public IGWFileExcel() : base() { }

        public IGWFileExcel(IGWUser _IGWUser) : base()
        {
            User = _IGWUser;
        }

        public IGWFileExcel(IGWUser _IGWUser, int _LimitSizeFile) : base(_LimitSizeFile)
        {
            User = _IGWUser;
        }

        #region IMPORT EXCEL
        // Xu li file excel khi import theo sheet
        /*
        FileExcel: FileBase Excel Import
        LstExcelImport: List object ExcelImport
        LstResult: List object muon tra ve
        ExcelInfoImport: Thong tin lay du lieu file excel gui len (Client: var ExcelInfoImport = JSON.stringify({ExcelSheet:"Sheet1",ExcelFirstRow:"5",MaColumn1:"B",MaColumn2:"C",MaColumn...:"..."}))
        IsDDMMYYYY: Dinh dang kieu DateTime doi voi cac o Excel la ngay thang (DD/MM/YYYY or MM/DD/YYYY)
        return: IGWMessage
         */
        public IGWMessage ProcessImportExcel<T>(HttpPostedFileBase FileExcel, List<ExcelImport> LstExcelImport, ref List<T> lstResult, string ExcelInfoImport, bool IsDDMMYYYY = true) where T : new()
        {
            IGWMessage msg = new IGWMessage();
            try
            {

                msg.isSuccess = true;
                return msg;
            }
            catch (Exception ex)
            {
                msg.isSuccess = false;
                msg.Description = ex.GetMessage();
                return msg;
            }
        }

        // Xu li excel khi import[mac dinh chon sheet dau tien]
        /*
        FileExcel: FileBase Excel Import
        LstExcelImport: List Object ExcelImport
        LstResult: List Object muon tra ve
        FirstRowData: Dong dau tien lay du lieu cua file excel
        IsDDMMYYYY: Dinh dang kieu DateTime doi voi cac o excel la ngay thang (DD/MM/YYYY or MM/DD/YYYY)
        return IGWMessage
         */

        public IGWMessage ProcessImportExcel<T>(HttpPostedFileBase FileExcel, List<ExcelImport> LstExcelImport, ref List<T> LstResult, int FirstRowData = 5, bool IsDDMMYYYY = false) where T : new()
        {
            IGWMessage msg = new IGWMessage();
            try
            {
                msg.isSuccess = false;
                if (FileExcel == null)
                {
                    msg.Description = "Khong co file Excel nao duoc tai len. Vui long kiem tra lai.";
                    return msg;
                }
                if (!base.IsValidFileLength(FileExcel.ContentLength))
                {
                    msg.Description = "File tai len khong dc lon hon" + base.LimitSizeFile + "MB.";
                    return msg;
                }
                if (!base.IsValidationExcelFile(FileExcel.FileName, FileExcel.ContentType))
                {
                    msg.Description = "File Excel danh sach yeu cau khong hop le";
                    return msg;
                }
                SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
                var workbook = new ExcelFile();
                string extFile = System.IO.Path.GetExtension(FileExcel.FileName).Substring(1).ToLower();
                if (extFile == "xlsx")
                {
                    workbook = ExcelFile.Load(FileExcel.InputStream, LoadOptions.XlsxDefault);
                }
                else
                {
                    workbook = ExcelFile.Load(FileExcel.InputStream, LoadOptions.XlsDefault);
                }
                var workSheet = workbook.Worksheets[0];
                int countRowsExcelFile = workSheet.Rows.Count;
                var rowStart = (FirstRowData < 1 ? 1 : FirstRowData); //mac dinh
                if (rowStart > countRowsExcelFile)
                {
                    msg.Description = "File Excel tai len khong co du lieu. Vui long kiem tra lai,";
                    return msg;
                }
                DateTime dCurrent = DateTime.Now;
                int i = 0;
                for (i = rowStart - 1; i < countRowsExcelFile; i++)
                {
                    ExcelRow row = workSheet.Rows[i];
                    T ItemDm = new T();
                    foreach (ExcelImport propExcel in LstExcelImport)
                    {
                        string value = GetValuePropertyObject(row, propExcel.ExcelColumn);
                        if (propExcel.IsBatBuoc && string.IsNullOrEmpty(value))
                        {
                            msg.Description = "<b>" + propExcel.ColumnName + "<b>, cot <b>[" + propExcel.ExcelColumn + (i + 1).ToString() + "]</b> khong hop le. Vui long kiem tra lai";
                            return msg;
                        }
                        if (!propExcel.IsBatBuoc && string.IsNullOrEmpty(value))
                        {
                            ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, null);
                            continue;
                        }
                        var _typeColumn = propExcel.TypeColumn.ToLower();
                        switch (_typeColumn)
                        {
                            case "datetime":
                                DateTime? d = IGWCommon.ConvertStringToDateTime(value, IsDDMMYYYY);
                                if (d == null && propExcel.IsBatBuoc)
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ExcelColumn + (i + 1).ToString() + "]</b> co du lieu Kieu ngay thang ko hop le (dinh dang dung dd/MM/yyyy HH:mm:ss). Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, d);
                                break;
                            case "decimal":
                                decimal dValue = 0;
                                if (!decimal.TryParse(value, out dValue))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ExcelColumn + (i + 1).ToString() + "]</b> co du lieu Kieu Number ko hop le (vi du dinh dang dung 119.128). Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, dValue);
                                break;
                            case "int":
                                int iValue = 0;
                                if (!int.TryParse(value, out iValue))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ColumnCode + (i + 1).ToString() + "]</b> dong thu <b>" + (i + 1).ToString().PadLeft(2, '0') + "</b> co du lieu kieu so Nguyen ko hop le. Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, iValue);
                                break;
                            case "short":
                                short iValueShort = 0;
                                if (!short.TryParse(value, out iValueShort))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ColumnCode + (i + 1).ToString() + "]</b> dong thu <b>" + (i + 1).ToString().PadLeft(2, '0') + "</b> co du lieu kieu so Nguyen ko hop le. Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, iValueShort);
                                break;
                            case "double":
                                double isDoubleShort = 0;
                                if (!double.TryParse(value, out isDoubleShort))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ColumnCode + (i + 1).ToString() + "]</b> dong thu <b>" + (i + 1).ToString().PadLeft(2, '0') + "</b> co du lieu kieu so Thap Phan ko hop le. Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, isDoubleShort);
                                break;
                            default:
                                value = (string.IsNullOrEmpty(value) ? "" : value.Trim());
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, value);
                                break;

                        }
                    }
                    PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    //if(Props.Where(x => x.Name == "").FirstOrDefault() != null) { ItemDm.GetType().GetProperty("").SetValue(ItemDm, User.); }

                    //...
                    //LstResult.Add(ItemDm);
                }
                msg.isSuccess = true;
                return msg;
            }
            catch (Exception ex)
            {
                msg.isSuccess = false;
                msg.Description = ex.GetMessage();
                return msg;
            }
        }

        #endregion

        #region EXPORT EXCEL
        /// <summary>
        /// Export Excel bao cao
        /// </summary>
        /// <param name="Data">Data xuat bao cao</param>
        /// <param name="LstExcelExport">List thong tin ExcelExport tuong ung[STT: Width = 7, MÃ NGẮN: Width = 10, MÃ DÀI: Width = 15, TÊN NGẮN: 30, TÊN DÀI VỪA: 40, TÊN CÓ ĐỘ DÀI LỚN: 80]]</param>
        /// <param name="Title">Tiêu đề</param>
        /// <param name="FileName">Tên file</param>
        /// <param name="Description"></param>
        /// <param name="workbook"></param>
        /// <returns></returns>
        /// 
        public FileExcelExport ExportExcelReport<T>(List<T> Data, List<ExcelExport> LstExcelExport, string Title, string FileName, string Description, ref ExcelFile workbook)
        {
            //link tham khao GemboxExcel https://www.gemboxsoftware.com/spreadsheet/examples/c-sharp-create-write-excel-file/402
            // va https://www.gemboxsoftware.com/spreadsheet/examples/c-sharp-vb-net-excel-style-formatting/202
            Dictionary<int, string> DicExcelColumn = new Dictionary<int, string>()
            {
                {1, "A" },{2, "B"},{3,"C"},{4,"D"},{5,"E"},{6,"F"},{7,"G"},{8,"H"},{9,"I"},{10,"J"},{11,"K"},{12,"L"},{13,"M"},{14,"N"},{15,"O"},{16,"P"},{17,"Q"},{18,"R"},{19,"S"},{20,"T"},{21,"U"},{22,"V"},{23,"W"},{24,"X"},{25,"Y"},{26,"Z"}
                ,{27,"AA"},{28,"AB"},{29,"AC"},{30,"AD"},{31,"AE"},{32,"AF"},{33,"AG"},{34,"AH"},{35,"AI"},{36,"AJ"},{37,"AK"},{38,"AL"},{39,"AM"},{40,"AN"},{41,"AO"},{42,"AP"},{43,"AQ"},{44,"AR"},{45,"AS"},{46,"AT"},{47,"AU"},{48,"AV"},{49,"AW"},{50,"AX"},{51,"AY"},{52,"AZ"}
            };
            //DicColumnData.Add("", new {ExcelColumn = "", Width = 10, Name = ""});
            SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
            workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Report");
            //Set header cho cac o excel
            int rowHeader = 4;
            worksheet.Rows[rowHeader - 1].Height = 25 * 20; //Set Height cho header Excel 25 point
            var styleHeader = new CellStyle();
            styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleHeader.VerticalAlignment = VerticalAlignmentStyle.Center;
            styleHeader.FillPattern.SetSolid(Color.DarkBlue);
            styleHeader.Font.Color = Color.White;
            styleHeader.Font.Weight = ExcelFont.BoldWeight;
            styleHeader.Font.Size = 12 * 20; // 12px
            styleHeader.Font.Name = "Times New Roman";
            Dictionary<string, string> DicPropertyAndColumnExcel = new Dictionary<string, string>();
            string LastExcelColumn = "A";
            worksheet.Columns["A"].SetWidth(7, LengthUnit.ZeroCharacterWidth); //SO THU TU
            worksheet.Cells["A" + rowHeader.ToString()].Value = "STT";
            worksheet.Cells["A" + rowHeader.ToString()].Style = styleHeader;
            Dictionary<string, decimal> DicSumValue = new Dictionary<string, decimal>();



            foreach (ExcelExport Item in LstExcelExport)
            {
                if (Item == null) continue;
                if (string.IsNullOrEmpty(Item.ColumnCode)) continue;
                object O = Item.ExcelOutput;
                string ExcelColumn = GetValuePropertyObject(O, "ExcelColumn");
                if (string.IsNullOrEmpty(ExcelColumn) || ExcelColumn == "0")
                {
                    continue;
                }
                int PositionExcelColumn = 0;
                if (int.TryParse(ExcelColumn, out PositionExcelColumn))
                {
                    ExcelColumn = DicExcelColumn[PositionExcelColumn + 1]; //Cong them 1 vi lúc truyền vào thứ tự các ô bắt đầu từ 1, mà vị trí số 1 là cột A đã hiển thị số thứ tự

                }
                // Gợi Ý Set Độ Dài Cho Các Ô Excel
                // STT: Width = 7, MÃ NGẮN: Width = 10, MÃ DÀI: Width = 15, TÊN NGẮN: 30, TÊN DÀI VỪA: 40, TÊN CÓ ĐỘ DÀI LỚN: 80
                double WidthColumn = double.Parse(GetValuePropertyObject(0, "Width"));
                worksheet.Columns[ExcelColumn].SetWidth(WidthColumn, LengthUnit.ZeroCharacterWidth);
                string Name = GetValuePropertyObject(0, "Name");
                worksheet.Cells[ExcelColumn + rowHeader.ToString()].Value = Name;

                string IsSumValue = GetValuePropertyObject(0, "IsSumValue");
                int HasSumValue = 0;
                if (int.TryParse(IsSumValue, out HasSumValue))
                {
                    if (HasSumValue == Common.YES)
                    {
                        DicSumValue.Add(ExcelColumn, 0);
                    }
                }
                worksheet.Cells[ExcelColumn + rowHeader.ToString()].Style = styleHeader;
                DicPropertyAndColumnExcel.Add(Item.ColumnCode, ExcelColumn);
                LastExcelColumn = ExcelColumn;
            }
            ////Thiết lập style cho ô
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Name = "Times New Roman";
            style.WrapText = true;

            //Set Title cho bao cao
            int rowTitle = 2;
            style.Font.Size = 17 * 20;
            CellRange crTitle = worksheet.Cells.GetSubrange("A2", LastExcelColumn + "2");
            crTitle.Merged = true;
            crTitle.Style = style;
            worksheet.Cells["A" + rowTitle.ToString()].Value = Title;
            //worksheet.Cells.GetSubrangeAbsolute(1, 0, 1, 2).Merged = true;
            //worksheet.Cells[LstColumnExcel[0] + "2"].Value = "Từ  đến ; Mức độ: ";

            //Hiển thị thông tin tìm kiếm để xuất báo cáo

            int rowDescription = rowTitle + 1;
            worksheet.Rows[rowDescription - 1].Height = 25 * 20; //Set Height cho header Excel 25 point
            CellRange crDescription = worksheet.Cells.GetSubrange("A3", LastExcelColumn + "3");
            crDescription.Merged = true;
            style.Font.Size = 13 * 20;
            style.Font.Italic = true;
            style.Font.Weight = ExcelFont.NormalWeight;
            crDescription.Style = style;
            worksheet.Cells["A" + rowDescription.ToString()].Value = Description;

            //Start Push data to Excel file
            style.Font.Italic = false;
            style.Font.Size = 12 * 20;
            int rowStart = 5, stt = 1;
            int getRow = rowStart;

            foreach (object item in Data)
            {
                worksheet.Rows.InsertCopy(getRow, worksheet.Rows[getRow - 1]);
                worksheet.Cells["A" + getRow].Value = stt;
                style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                worksheet.Cells["A" + getRow].Style = style;
                foreach (string prop in DicPropertyAndColumnExcel.Keys)
                {
                    if (string.IsNullOrEmpty(prop) || string.IsNullOrEmpty(DicPropertyAndColumnExcel[prop])) continue;
                    style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    worksheet.Cells[DicPropertyAndColumnExcel[prop] + getRow].Value = GetValuePropertyObject(item, prop);
                    // Tính tổng
                    if (DicSumValue.ContainsKey(DicPropertyAndColumnExcel[prop]))
                    {
                        style.HorizontalAlignment = HorizontalAlignmentStyle.Right;
                        DicSumValue[DicPropertyAndColumnExcel[prop]] += decimal.Parse(string.IsNullOrEmpty(GetValuePropertyObject(item, prop)) ? Common.NO.ToString() : GetValuePropertyObject(item, prop));
                    }
                    worksheet.Cells[DicPropertyAndColumnExcel[prop] + getRow].Style = style;
                }
                getRow++;
                stt++;
            }
            // Set footer for excel file
            int rowFooter = getRow + 1;
            styleHeader.Font.Color = Color.White;
            styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Right;
            foreach (string columnExcel in DicSumValue.Keys)
            {
                worksheet.Cells[columnExcel + rowFooter.ToString()].Value = string.Format("{0:#0.##}", DicSumValue[columnExcel]);
                worksheet.Cells[columnExcel + rowFooter.ToString()].Style = styleHeader;
            }
            FileName = (string.IsNullOrEmpty(FileName) ? "Report" : FileName) + IGWCommon.DateToStringYyyyMmDdHhMmSs(DateTime.Now);
            GemBoxFileOutPut gemBoxFile = new GemBoxFileOutPut(FileName, "xlsx");
            SaveOptions options = ExcelGemBox.GetSaveOptions(gemBoxFile.FileType);
            return new FileExcelExport(ExcelGemBox.GetBytes(workbook, options), options.ContentType, gemBoxFile.FileName + "." + gemBoxFile.FileType.ToLowerInvariant());
        }

        ///<summary>
        ///</summary>
        ///
        public FileExcelExport ExportExcel<T>(IEnumerable<T> Data, List<ExcelExport> LstExcelExport, string Title, string FileName, string Description)
        {
            //link tham khao GemboxExcel https://www.gemboxsoftware.com/spreadsheet/examples/c-sharp-create-write-excel-file/402
            // va https://www.gemboxsoftware.com/spreadsheet/examples/c-sharp-vb-net-excel-style-formatting/202
            Dictionary<int, string> DicExcelColumn = new Dictionary<int, string>()
            {
                {1, "A" },{2, "B"},{3,"C"},{4,"D"},{5,"E"},{6,"F"},{7,"G"},{8,"H"},{9,"I"},{10,"J"},{11,"K"},{12,"L"},{13,"M"},{14,"N"},{15,"O"},{16,"P"},{17,"Q"},{18,"R"},{19,"S"},{20,"T"},{21,"U"},{22,"V"},{23,"W"},{24,"X"},{25,"Y"},{26,"Z"}
                ,{27,"AA"},{28,"AB"},{29,"AC"},{30,"AD"},{31,"AE"},{32,"AF"},{33,"AG"},{34,"AH"},{35,"AI"},{36,"AJ"},{37,"AK"},{38,"AL"},{39,"AM"},{40,"AN"},{41,"AO"},{42,"AP"},{43,"AQ"},{44,"AR"},{45,"AS"},{46,"AT"},{47,"AU"},{48,"AV"},{49,"AW"},{50,"AX"},{51,"AY"},{52,"AZ"}
            };
            //DicColumnData.Add("", new {ExcelColumn = "", Width = 10, Name = ""});
            Dictionary<string, string> DicPropertyAndColumnExcel = new Dictionary<string, string>();
            string LastExcelColumn = "A";

            SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Report");
            //Set header cho cac o excel
            int rowHeader = 4;
            worksheet.Rows[rowHeader - 1].Height = 18 * 20; //Set Height cho header Excel 18 point
            var styleHeader = new CellStyle();
            styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleHeader.VerticalAlignment = VerticalAlignmentStyle.Center;
            styleHeader.FillPattern.SetSolid(Color.DarkBlue);//Color.DeepSkyBlue//RoyalBlue//DarkSlateBlue//DarkBlue
            styleHeader.Font.Color = Color.White;
            styleHeader.Font.Weight = ExcelFont.BoldWeight;
            styleHeader.Font.Size = 10 * 20; // 10px
            styleHeader.Font.Name = "Times New Roman";
            worksheet.Columns["A"].SetWidth(7, LengthUnit.ZeroCharacterWidth); //SO THU TU
            worksheet.Cells["A" + rowHeader.ToString()].Value = "STT";
            worksheet.Cells["A" + rowHeader.ToString()].Style = styleHeader;

            foreach (ExcelExport Item in LstExcelExport)
            {
                if (Item == null) continue;
                if (string.IsNullOrEmpty(Item.ColumnCode)) continue;
                object O = Item.ExcelOutput;
                //string ExcelColumn = GetValuePropertyObject(O, "ExcelColumn");
                string ExcelColumn = Item.ExcelOutput.ExcelColumn;
                if (string.IsNullOrEmpty(ExcelColumn) || ExcelColumn == "0")
                {
                    continue;
                }
                int PositionExcelColumn = 0;
                if (int.TryParse(ExcelColumn, out PositionExcelColumn))
                {
                    ExcelColumn = DicExcelColumn[PositionExcelColumn + 1]; //Cong them 1 vi lúc truyền vào thứ tự các ô bắt đầu từ 1, mà vị trí số 1 là cột A đã hiển thị số thứ tự

                }
                // Gợi Ý Set Độ Dài Cho Các Ô Excel
                // STT: Width = 7, MÃ NGẮN: Width = 10, MÃ DÀI: Width = 15, TÊN NGẮN: 30, TÊN DÀI VỪA: 40, TÊN CÓ ĐỘ DÀI LỚN: 80
                double WidthColumn = double.Parse(GetValuePropertyObject(0, "Width"));
                worksheet.Columns[ExcelColumn].SetWidth(WidthColumn, LengthUnit.ZeroCharacterWidth);
                string Name = GetValuePropertyObject(0, "Name");
                worksheet.Cells[ExcelColumn + rowHeader.ToString()].Value = Name;
                worksheet.Cells[ExcelColumn + rowHeader.ToString()].Style = styleHeader;
                DicPropertyAndColumnExcel.Add(Item.ColumnCode, ExcelColumn);
                LastExcelColumn = ExcelColumn;
            }
            ////Thiết lập style cho ô
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Name = "Times New Roman";
            style.WrapText = true;

            //Set Title cho báo cáo
            worksheet.Rows[0].Height = 7 * 20;
            int rowTitle = 2;
            style.Font.Size = 12 * 20;
            CellRange crTitle = worksheet.Cells.GetSubrange("A2", LastExcelColumn + "2");
            crTitle.Merged = true;
            crTitle.Style = style;
            worksheet.Cells["A" + rowTitle.ToString()].Value = Title;
            //worksheet.Cells.GetSubrangeAbsolute(1, 0, 1, 2).Merged = true;
            //worksheet.Cells[LstColumnExcel[0] + "2"].Value = "Từ  đến ; Mức độ: ";

            //Hiển thị thông tin tìm kiếm để xuất báo cáo

            int rowDescription = rowTitle + 1;
            worksheet.Rows[rowDescription - 1].Height = 25 * 20; //Set Height cho header Excel 25 point
            CellRange crDescription = worksheet.Cells.GetSubrange("A3", LastExcelColumn + "3");
            crDescription.Merged = true;
            style.Font.Size = 12 * 20;
            style.Font.Italic = true;
            style.Font.Weight = ExcelFont.NormalWeight;
            crDescription.Style = style;
            worksheet.Cells["A" + rowDescription.ToString()].Value = Description;

            //Start Push data to Excel file
            style.Font.Italic = false;
            style.Font.Size = 12 * 20;
            int rowStart = 5, stt = 1;
            int getRow = rowStart;
            //var watch = new Stopwatch();
            //watch.Start();

            if(Data.Count() > 0)
            {
                foreach (object item in Data)
                {
                    worksheet.Rows.InsertCopy(getRow, worksheet.Rows[getRow - 1]);
                    worksheet.Cells["A" + getRow].Value = stt;
                    style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                    worksheet.Cells["A" + getRow].Style = style;
                    foreach (string prop in DicPropertyAndColumnExcel.Keys)
                    {
                        if (string.IsNullOrEmpty(prop) || string.IsNullOrEmpty(DicPropertyAndColumnExcel[prop])) continue;
                        style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                        worksheet.Cells[DicPropertyAndColumnExcel[prop] + getRow].Value = GetValuePropertyObject(item, prop);
                        worksheet.Cells[DicPropertyAndColumnExcel[prop] + getRow].Style = style;
                    }
                    getRow++;
                    stt++;
                }
                //watch.Stop();
                //var elap = watch.EllapseMilliseconds;
                // set border ca bao cao
                worksheet.Cells.GetSubrange("A" + rowStart, LastExcelColumn + (getRow - 1)).Style.Borders.SetBorders(MultipleBorders.All,SpreadsheetColor.FromName(ColorName.Black),LineStyle.Thin);

            }
            string extraFile = IGWCommon.DateToStringYyyyMmDdHhMmSs(DateTime.Now);
            GemBoxFileOutPut gemBoxFile = new GemBoxFileOutPut(FileName, "xlsx");
            SaveOptions options = ExcelGemBox.GetSaveOptions(gemBoxFile.FileType);
            return new FileExcelExport(ExcelGemBox.GetBytes(workbook, options), options.ContentType, gemBoxFile.FileName + "." + gemBoxFile.FileType.ToLowerInvariant());
        }

        public FileExcelExport ExportExcel(DataTable TableData, List<ExcelExport> LstExcelExport, string Title, string FileName, string Description)
        {
            //link tham khao GemboxExcel https://www.gemboxsoftware.com/spreadsheet/examples/c-sharp-create-write-excel-file/402
            // va https://www.gemboxsoftware.com/spreadsheet/examples/c-sharp-vb-net-excel-style-formatting/202
            Dictionary<int, string> DicExcelColumn = new Dictionary<int, string>()
            {
                {1, "A" },{2, "B"},{3,"C"},{4,"D"},{5,"E"},{6,"F"},{7,"G"},{8,"H"},{9,"I"},{10,"J"},{11,"K"},{12,"L"},{13,"M"},{14,"N"},{15,"O"},{16,"P"},{17,"Q"},{18,"R"},{19,"S"},{20,"T"},{21,"U"},{22,"V"},{23,"W"},{24,"X"},{25,"Y"},{26,"Z"}
                ,{27,"AA"},{28,"AB"},{29,"AC"},{30,"AD"},{31,"AE"},{32,"AF"},{33,"AG"},{34,"AH"},{35,"AI"},{36,"AJ"},{37,"AK"},{38,"AL"},{39,"AM"},{40,"AN"},{41,"AO"},{42,"AP"},{43,"AQ"},{44,"AR"},{45,"AS"},{46,"AT"},{47,"AU"},{48,"AV"},{49,"AW"},{50,"AX"},{51,"AY"},{52,"AZ"}
            };
            //DicColumnData.Add("", new {ExcelColumn = "", Width = 10, Name = ""});
            Dictionary<string, string> DicPropertyAndColumnExcel = new Dictionary<string, string>();
            string LastExcelColumn = "A";

            SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
            var workbook = new ExcelFile();
            var worksheet = workbook.Worksheets.Add("Danh sách");
            //Set header cho cac o excel
            int rowHeader = 4;
            worksheet.Rows[rowHeader - 1].Height = 18 * 20; //Set Height cho header Excel 18 point
            var styleHeader = new CellStyle();
            styleHeader.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            styleHeader.VerticalAlignment = VerticalAlignmentStyle.Center;
            styleHeader.FillPattern.SetSolid(Color.DarkBlue);//Color.DeepSkyBlue//RoyalBlue//DarkSlateBlue//DarkBlue
            styleHeader.Font.Color = Color.White;
            styleHeader.Font.Weight = ExcelFont.BoldWeight;
            styleHeader.Font.Size = 10 * 20; // 10px
            styleHeader.Font.Name = "Times New Roman";
            worksheet.Columns["A"].SetWidth(7, LengthUnit.ZeroCharacterWidth); //SO THU TU
            worksheet.Cells["A" + rowHeader.ToString()].Value = "STT";
            worksheet.Cells["A" + rowHeader.ToString()].Style = styleHeader;

            foreach (ExcelExport Item in LstExcelExport)
            {
                if (Item == null) continue;
                if (string.IsNullOrEmpty(Item.ColumnCode)) continue;
                object O = Item.ExcelOutput;
                //string ExcelColumn = GetValuePropertyObject(O, "ExcelColumn");
                string ExcelColumn = Item.ExcelOutput.ExcelColumn;
                if (string.IsNullOrEmpty(ExcelColumn) || ExcelColumn == "0")
                {
                    continue;
                }
                int PositionExcelColumn = 0;
                if (int.TryParse(ExcelColumn, out PositionExcelColumn))
                {
                    ExcelColumn = DicExcelColumn[PositionExcelColumn + 1]; //Cong them 1 vi lúc truyền vào thứ tự các ô bắt đầu từ 1, mà vị trí số 1 là cột A đã hiển thị số thứ tự

                }
                // Gợi Ý Set Độ Dài Cho Các Ô Excel
                // STT: Width = 7, MÃ NGẮN: Width = 10, MÃ DÀI: Width = 15, TÊN NGẮN: 30, TÊN DÀI VỪA: 40, TÊN CÓ ĐỘ DÀI LỚN: 80
                double WidthColumn = double.Parse(GetValuePropertyObject(0, "Width"));
                worksheet.Columns[ExcelColumn].SetWidth(WidthColumn, LengthUnit.ZeroCharacterWidth);
                string Name = GetValuePropertyObject(0, "Name");
                worksheet.Cells[ExcelColumn + rowHeader.ToString()].Value = Name;
                worksheet.Cells[ExcelColumn + rowHeader.ToString()].Style = styleHeader;
                DicPropertyAndColumnExcel.Add(Item.ColumnCode, ExcelColumn);
                LastExcelColumn = ExcelColumn;
            }
            ////Thiết lập style cho ô
            var style = new CellStyle();
            style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
            style.VerticalAlignment = VerticalAlignmentStyle.Center;
            style.Font.Weight = ExcelFont.BoldWeight;
            style.Font.Name = "Times New Roman";
            style.WrapText = true;

            //Set Title cho báo cáo
            worksheet.Rows[0].Height = 7 * 20;
            int rowTitle = 2;
            style.Font.Size = 12 * 20;
            CellRange crTitle = worksheet.Cells.GetSubrange("A2", LastExcelColumn + "2");
            crTitle.Merged = true;
            crTitle.Style = style;
            worksheet.Cells["A" + rowTitle.ToString()].Value = Title;
            //worksheet.Cells.GetSubrangeAbsolute(1, 0, 1, 2).Merged = true;
            //worksheet.Cells[LstColumnExcel[0] + "2"].Value = "Từ  đến ; Mức độ: ";

            //Hiển thị thông tin tìm kiếm để xuất báo cáo

            int rowDescription = rowTitle + 1;
            worksheet.Rows[rowDescription - 1].Height = 25 * 20; //Set Height cho header Excel 25 point
            CellRange crDescription = worksheet.Cells.GetSubrange("A3", LastExcelColumn + "3");
            crDescription.Merged = true;
            style.Font.Size = 12 * 20;
            style.Font.Italic = true;
            style.Font.Weight = ExcelFont.NormalWeight;
            crDescription.Style = style;
            worksheet.Cells["A" + rowDescription.ToString()].Value = Description;

            //Start Push data to Excel file
            style.Font.Italic = false;
            style.Font.Size = 12 * 20;
            int rowStart = 5, stt = 1;
            int getRow = rowStart;
            //var watch = new Stopwatch();
            //watch.Start();

            foreach(DataRow item in TableData.Rows)
            {
                worksheet.Rows.InsertCopy(getRow, worksheet.Rows[getRow - 1]);
                worksheet.Cells["A" + getRow].Value = stt;
                style.HorizontalAlignment = HorizontalAlignmentStyle.Center;
                worksheet.Cells["A" + getRow].Style = style;
                foreach (string prop in DicPropertyAndColumnExcel.Keys)
                {
                    if (string.IsNullOrEmpty(prop) || string.IsNullOrEmpty(DicPropertyAndColumnExcel[prop])) continue;
                    style.HorizontalAlignment = HorizontalAlignmentStyle.Left;
                    worksheet.Cells[DicPropertyAndColumnExcel[prop] + getRow].Value = GetValuePropertyObject(item, prop);
                    worksheet.Cells[DicPropertyAndColumnExcel[prop] + getRow].Style = style;
                }
                getRow++;
                stt++;
            }
            int rowEnd = (getRow - 1);
            if(rowEnd >= rowStart)
            {
                worksheet.Cells.GetSubrange("A" + rowStart.ToString(), LastExcelColumn + rowEnd.ToString()).Style.Borders.SetBorders(MultipleBorders.All,SpreadsheetColor.FromName(ColorName.Black),LineStyle.Thin);
            }
            FileName = (string.IsNullOrEmpty(FileName) ? "Report" : FileName) + IGWCommon.DateToStringYyMmDdHhMmSs(DateTime.Now);
            GemBoxFileOutPut gemBoxFile = new GemBoxFileOutPut(FileName, "xlsx");
            SaveOptions options = ExcelGemBox.GetSaveOptions(gemBoxFile.FileType);
            return new FileExcelExport(ExcelGemBox.GetBytes(workbook, options), options.ContentType, gemBoxFile.FileName + "." + gemBoxFile.FileType.ToLowerInvariant());
        }

        /// <summary>
        /// Export Excel theo file mẫu
        /// </summary>
        /// <param name="Data"></param>
        /// <param name="FileTempPath">Đường dẫn file template[ví dụ: ../abc/a.xlsx]</param>
        /// <param name="DicColumnData">Là dictionary có KEY là Property của Data và VALUE là Tên cột trong file excel tương ứng để Push dữ liệu vào [Dictionary<string,string>(){"abc","cba"}</string>]</param>
        /// <param name="FileName">Tên file khi export</param>
        /// <param name="Description">mô tả thông tin dữ liệu tìm kiếm (Fill dữ liệu ở ổ A3)</param>
        /// <param name="rowStart">Dòng excel bắt đầu insert dữ liệu</param>
        /// <returns></returns>
        /// 
        public FileExcelExport ExportExcelWithTemp<T>(string FileTempPath, IEnumerable<T> Data, Dictionary<string,string> DicColumnData, string FileName, string Description, int rowStart = 5)
        {
            SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
            //var workbook = ExcelFile.Load(FileTempPath,LoadOptions.XlsxDefault);
            string extFile = System.IO.Path.GetExtension(FileTempPath).Substring(1).ToLower();
            var workbook = new ExcelFile();
            if(extFile == "xlsx")
            {
                workbook = ExcelFile.Load(FileTempPath,LoadOptions.XlsxDefault);
            }
            else
            {
                workbook = ExcelFile.Load(FileTempPath,LoadOptions.XlsDefault);
            }
            var worksheet = workbook.Worksheets[0];
            if (string.IsNullOrEmpty(Description))
            {
                worksheet.Cells["A3"].Value = Description;
            }
            int stt = 1, getRow = rowStart;

            foreach (object item in Data)
            {
                worksheet.Rows.InsertCopy(getRow, worksheet.Rows[getRow - 1]);
                worksheet.Cells["A" + getRow].Value = stt;
                foreach (string prop in DicColumnData.Keys)
                {
                    if (string.IsNullOrEmpty(prop) || string.IsNullOrEmpty(DicColumnData[prop])) continue;
                    string GetValue = GetValuePropertyObject(item, prop).Trim();
                    worksheet.Cells[DicColumnData[prop] + getRow].Value = (GetValue == Common.NO.ToString() ? string.Empty : GetValue);
                }
                getRow++;
                stt++;
            }
            FileName = (string.IsNullOrEmpty(FileName) ? "Report" : FileName) + IGWCommon.DateToStringYyMmDdHhMmSs(DateTime.Now);
            GemBoxFileOutPut gemBoxFile = new GemBoxFileOutPut(FileName, "xlsx");
            SaveOptions options = ExcelGemBox.GetSaveOptions(gemBoxFile.FileType);
            return new FileExcelExport(ExcelGemBox.GetBytes(workbook, options), options.ContentType, gemBoxFile.FileName + "." + gemBoxFile.FileType.ToLowerInvariant());
        }
        #endregion
        public string GetValuePropertyObject(object OItem, string Prop, bool IsFormatNumber = false)
        {
            try
            {
                if (OItem == null) return string.Empty;
                if (IsFormatNumber)
                {
                    decimal dValue = (OItem.GetType().GetProperty(Prop).GetValue(OItem, null) == null ? 0 : decimal.Parse(OItem.GetType().GetProperty(Prop).GetValue(OItem, null).ToString()));
                    return string.Format("{0:#0.##}", dValue);
                }
                return (OItem.GetType().GetProperty(Prop).GetValue(OItem, null) == null ? string.Empty : OItem.GetType().GetProperty(Prop).GetValue(OItem, null).ToString());
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public string GetValuePropertyObject(ExcelRow RowItem, string Prop, bool IsFormatNumber = false)
        {
            try
            {
                if (RowItem == null) return string.Empty;
                if (IsFormatNumber)
                {
                    decimal dValue = RowItem.Cells[Prop].Value == null ? 0 : decimal.Parse(RowItem.Cells[Prop].Value.ToString());
                    return string.Format("{0:#0.##}", dValue);
                }
                return (RowItem.Cells[Prop].Value == null ? string.Empty : RowItem.Cells[Prop].Value.ToString());
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetValuePropertyDataRow(DataRow RowItem, string Prop, bool IsFormatNumber = false)
        {
            try
            {
                if (RowItem == null) return string.Empty;
                string strValue = RowItem[Prop].ToString();
                if (IsFormatNumber)
                {
                    decimal dValue = decimal.Parse(strValue);
                    return string.Format("{0:#0.##}", dValue);
                }
                return strValue;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
