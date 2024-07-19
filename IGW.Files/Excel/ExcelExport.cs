using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary.Files.Excel
{
    //Item data Excel khi export
    public class ExcelExport
    {
        //La ten cot trong bang muon hien thi len bao cao
        public string ColumnCode { get; set; }

        //Cac gia tri hien thi tren excel tuong ung voi ColumnCode
        public ExcelExportColumn ExcelOutput;
        public ExcelExport(string _ColumnCode, ExcelExportColumn _ExcelOutput)
        {
            ColumnCode = _ColumnCode;
            ExcelOutput = _ExcelOutput;
        }
    }

    public class ExcelExportColumn
    {
        //So thu tu hoac ten cot excel tuong ung de hien thi tren file excel cho excelExportColumn
        public string ExcelColumn { get; set; }
        //Do dai o excel
        public int Width { get; set; } = 0;
        //Ten hien thi cho tieu de excel (Header)
        public string Name { get; set; }
        //Co tinh tong cua cot excel hay ko
        public int IsSumValue { get; set; } = 0;

        //Khoi tao cac gia tri hien thi tren excel tuong ung voi ColumnCode

        /// <param name="_ExcelColumn">Ten cot excel tuong ung de hien thi cho ColumnCode</param>
        /// <param name="_Width">Do dai o excel hien thi cho ColumnCode</param>
        /// <param name="_Name">Ten header hien thi cho ColumnCode</param>
        /// <param name="_IsSumValue">Co tinh tong cua ColumnCode hay ko ?</param>
        /// 
        public ExcelExportColumn(string _ExcelColumn, int _Width = 0, string _Name="", int _IsSumValue = 0)
        {
            this.ExcelColumn = _ExcelColumn;
            this.Width = _Width;
            this.Name = _Name;
            this.IsSumValue = _IsSumValue;
        }
    } 
}
