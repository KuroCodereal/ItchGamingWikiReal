using GemBox.Spreadsheet;
using IGW.Files.Excel;
using ItchGamingLibrary;
using ItchGamingLibrary.Files;
using ItchGamingLibrary.Files.Excel;
using ItchGamingLibrary.User;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

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
            catch(Exception ex)
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
                if(FileExcel == null)
                {
                    msg.Description = "Khong co file Excel nao duoc tai len. Vui long kiem tra lai.";
                    return msg;
                }
                if (!base.IsValidFileLength(FileExcel.ContentLength))
                {
                    msg.Description = "File tai len khong dc lon hon" + base.LimitSizeFile + "MB.";
                    return msg;
                }
                if(!base.IsValidationExcelFile(FileExcel.FileName, FileExcel.ContentType))
                {
                    msg.Description = "File Excel danh sach yeu cau khong hop le";
                    return msg;
                }
                SpreadsheetInfo.SetLicense(Common.LicenseGemBox);
                var workbook = new ExcelFile();
                string extFile = System.IO.Path.GetExtension(FileExcel.FileName).Substring(1).ToLower();
                if(extFile == "xlsx")
                {
                    workbook = ExcelFile.Load(FileExcel.InputStream, LoadOptions.XlsxDefault);
                }
                else
                {
                    workbook = ExcelFile.Load(FileExcel.InputStream,LoadOptions.XlsDefault);
                }
                var workSheet = workbook.Worksheets[0];
                int countRowsExcelFile = workSheet.Rows.Count;
                var rowStart = (FirstRowData < 1 ? 1 : FirstRowData); //mac dinh
                if(rowStart > countRowsExcelFile)
                {
                    msg.Description = "File Excel tai len khong co du lieu. Vui long kiem tra lai,";
                    return msg;
                }
                DateTime dCurrent = DateTime.Now;
                int i = 0;
                for(i = rowStart - 1; i < countRowsExcelFile; i++)
                {
                    ExcelRow row = workSheet.Rows[i];
                    T ItemDm = new T();
                    foreach(ExcelImport propExcel in LstExcelImport)
                    {
                        string value = GetValuePropertyObject(row, propExcel.ExcelColumn);
                        if(propExcel.IsBatBuoc && string.IsNullOrEmpty(value))
                        {
                            msg.Description = "<b>" + propExcel.ColumnName + "<b>, cot <b>[" + propExcel.ExcelColumn + (i + 1).ToString() + "]</b> khong hop le. Vui long kiem tra lai";
                            return msg;
                        }
                        if(!propExcel.IsBatBuoc && string.IsNullOrEmpty(value))
                        {
                            ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, null);
                            continue;
                        }
                        var _typeColumn = propExcel.TypeColumn.ToLower();
                        switch (_typeColumn)
                        {
                            case "datetime":
                                DateTime? d = IGWCommon.ConvertStringToDateTime(value, IsDDMMYYYY);
                                if(d == null && propExcel.IsBatBuoc)
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ExcelColumn + (i + 1).ToString() + "]</b> co du lieu Kieu ngay thang ko hop le (dinh dang dung dd/MM/yyyy HH:mm:ss). Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm,d);
                                break;
                            case "decimal":
                                decimal dValue = 0;
                                if(!decimal.TryParse(value, out dValue))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ExcelColumn + (i + 1).ToString() + "]</b> co du lieu Kieu Number ko hop le (vi du dinh dang dung 119.128). Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, dValue);
                                break;
                            case "int":
                                int iValue = 0;
                                if(!int.TryParse(value, out iValue))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ColumnCode + (i + 1).ToString() + "]</b> dong thu <b>" + (i+1).ToString().PadLeft(2,'0') + "</b> co du lieu kieu so Nguyen ko hop le. Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, iValue);
                                break;
                            case "short":
                                short iValueShort = 0;
                                if(!short.TryParse(value, out iValueShort))
                                {
                                    msg.Description = "<b>" + propExcel.ColumnName + "</b>, cot <b>[" + propExcel.ColumnCode + (i + 1).ToString() + "]</b> dong thu <b>" + (i + 1).ToString().PadLeft(2, '0') + "</b> co du lieu kieu so Nguyen ko hop le. Vui long kiem tra lai.";
                                    return msg;
                                }
                                ItemDm.GetType().GetProperty(propExcel.ColumnCode).SetValue(ItemDm, iValueShort);
                                break;
                            case "double":
                                double isDoubleShort = 0;
                                if(!double.TryParse(value, out isDoubleShort))
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
            catch(Exception ex)
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
                {1, "A" },{2, "B"},{3,"C"}
            };
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
                    return string.Format("{0:#0.##}",dValue);
                }
                return (OItem.GetType().GetProperty(Prop).GetValue(OItem, null) == null ? string.Empty : OItem.GetType().GetProperty(Prop).GetValue(OItem, null).ToString());
            }
            catch(Exception ex)
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
                    return string.Format("{0:#0.##}",dValue);
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
