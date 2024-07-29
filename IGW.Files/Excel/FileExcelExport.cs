using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IGW.Files.Excel
{
    public class FileExcelExport
    {
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
        public string FileDownloadName { get; set; }
        public FileExcelExport(byte[] _fileContents, string _contentType, string _fileDownloadName)
        {
            FileContents = _fileContents;
            ContentType = _contentType;
            FileDownloadName = _fileDownloadName;
        }
    }
}
