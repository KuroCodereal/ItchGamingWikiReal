using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;

namespace IGW.Files
{
    public class IGWFileContentType
    {
        //DOC = application/msword
        public static string DOC = "application/msword";

        //DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
        public static string DOCX = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";

        //XLS = "application/vnd.ms-excel";
        public static string XLS = "application/vnd.ms-excel";

        //XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
        public static string XLSX = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

        //PPT = "application/vnd.ms-powerpoint"
        public static string PPT = "application/vnd.ms-powerpoint";

        //TXT = "text/plain";
        public static string TXT = "text/plain";

        //PDF = "application/PDF";
        public static string PDF = "application/pdf";

        //IMG = "image/*";
        public static string IMG = "image/*";

        //JPEG = "image/jpeg";
        public static string JPEG = "image/jpeg";

        //JPG = "image/jpg";
        public static string JPG = "image/jpg";

        //PNG = "image/png";
        public static string PNG = "image/png";

        //DEFAULT = "multipart/form-data"
        public static string DEFAULT = "multipart/form-data";

        public static Dictionary<string, string> Dictionary = new Dictionary<string, string>()
        {
            { "DOC", "application/msword"},
            { "DOCX", "application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            { "XLS", "application/vnd.ms-excel"},
            { "XLSX", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            { "PPT", "application/vnd.ms-powerpoint"},
            { "TXT", "text/plain"},
            { "PDF", "application/pdf"},
            { "IMG","image/*"},
            { "JPEG","image/jpeg"},
            { "JPG","image/jpg"},
            { "PNG","image/png"},
            { "DEFAULT","multipart/form-data" }
        };

        //Ten file full
        public static string Get(string FileName)
        {
            string ExtFile = System.IO.Path.GetExtension(FileName).Substring(1).ToUpper();
            if (!Dictionary.ContainsKey(ExtFile)) return DEFAULT;
            return Dictionary[ExtFile];
        }

    }
}
