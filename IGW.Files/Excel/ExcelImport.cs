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
        /// <param name="ColumCode">Mã cột trong database</param>
        /// <param name="ColumCode">Mã cột trong database</param>
        /// <param name="ColumCode">Mã cột trong database</param>
        /// <param name="ColumCode">Mã cột trong database</param>
    }
}
