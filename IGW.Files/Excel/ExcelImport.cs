using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace IGW.Files.Excel
{

    public class ExcelImport
    {
        // Ma cot trong Database
        public string ColumnCode { get; set; }
        // Ten cot
        public string ColumnName { get; set; } 
        // Kieu du lieu ["string","int","decimal","short","double","datetime"]
        public string TypeColumn {  get; set; }
        // Co bat buoc hay ko
        public bool IsBatBuoc {  get; set; }
        // Ten cot excel
        public string ExcelColumn {  get; set; }
        // Gia tri mac dinh khi khong co gia tri trong file excel
        public string DefaulValue {  get; set; }
        #region Đối với trường hợp import file excel để update thì sử dụng 2 thuộc tính này
        // Đánh dấu column cần join để update
        public bool IsColumnjoinToUpdate {  get; set; }
        // Đánh dấu column cần update
        public bool IsColumnUpdate {  get; set; }
        #endregion

        ///<summary>
        ///Khởi tạo đối tượng ExcelImport
        /// </summary>
        /// <param name="ColumCode">Mã cột trong database</param>
        /// <param name="ColumName">Tên/ Ý nghĩa của các cột trong database</param>
        /// <param name="ExcelColumn">Tên ô excel tương ứng cần lấy dữ liệu cho ColumnCode</param>
        /// <param name="IsBatBuoc">ExcelColumn có bắt buộc có data hay ko ?</param>
        /// <param name="TypeColumn">Kiểu dữ liệu của MaColumn["string","int","decimal","double","datetime","short"]</param>
        /// <param name="DefaultValue">Giá trị mặc định khi ko có giá trị trong ô excel tương ứng</param>
        /// <param name="IsColumnJoinUpdate">Đánh dấu là ColumnCode cần Join để Update (True/False)</param>
        /// <param name="IsColumnUpdate">Đánh dấu là ColumnCode cần Update</param>
        /// 
        public ExcelImport(string ColumnCode, string ColumnName, string _ExcelColumn, bool IsBatBuoc = false, string TypeColumn = "string", string DefaultValue = "", bool IsColumnJoinUpdate = false, bool IsColumnUpdate = false)
        {
            if(string.IsNullOrEmpty(ColumnCode) || string.IsNullOrEmpty(TypeColumn) || string.IsNullOrEmpty(_ExcelColumn))
            {
                throw new Exception("ColumnCode, TypeColumn, ExcelColumn la bat buoc. Vui long kiem tra lai");
            }
            this.ColumnCode = ColumnCode;
            this.ColumnName = ColumnName;
            this.IsBatBuoc  = IsBatBuoc;
            this.ExcelColumn = ExcelColumn;
            List<string> LstTypeColumn = new List<string>() { "string", "int", "decimal", "datetime", "short" };
            this.TypeColumn = LstTypeColumn.Contains(TypeColumn) ? TypeColumn : "string";
            this.DefaulValue = DefaultValue;
            this.IsColumnjoinToUpdate = IsColumnJoinUpdate;
            this.IsColumnUpdate = IsColumnUpdate;
        }
    }
}
