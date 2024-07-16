using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;

namespace ItchGamingWiki.ItchCommon
{
    public class WebConfigKey
    {
        public static string MailHost = WebConfigurationManager.AppSettings["MailHost"].ToString();
        public static int MailPort = Convert.ToInt32(WebConfigurationManager.AppSettings["MailPort"] ?? "0");
        public static string MailUser = WebConfigurationManager.AppSettings["MailUser"].ToString();
        public static string MailPass = WebConfigurationManager.AppSettings["MailPass"].ToString();
        public static bool IsWriteLog = Convert.ToInt32(WebConfigurationManager.AppSettings["IsWriteLog"].ToString()) == 1 ? true : false;
        public static bool IsDisplayMenu = Convert.ToInt32(WebConfigurationManager.AppSettings["IsDisplayMenu"].ToString()) == 1 ? true : false;

        //Dung luong toi da cho cac file
        public static int FileSizeMax = WebConfigurationManager.AppSettings["FileSizeMax"] == null ? 4 : int.Parse(WebConfigurationManager.AppSettings["FileSizeMax"].ToString());

        //Dung luong toi da cho file .CSV
        public static int FileCSVSizeMax = WebConfigurationManager.AppSettings["FileCSVSizeMax"] == null ? 10 : int.Parse(WebConfigurationManager.AppSettings["FileCSVSizeMax"].ToString());

        //Dung luong toi da cho file .zip, .rar
        public static int FileZipSizeMax = WebConfigurationManager.AppSettings["FileZipSizeMax"] == null ? 50 : int.Parse(WebConfigurationManager.AppSettings["FileZipSizeMax"].ToString());

        //Link url server file
        public static string URLFileUpdate = WebConfigurationManager.AppSettings["URLFileUpdate"] == null ? string.Empty : WebConfigurationManager.AppSettings["URLFileUpdate"].ToString();

        //Thu muc goc server file
        public static string FolderFileUpdate = WebConfigurationManager.AppSettings["FolderFileUpdate"] == null ? string.Empty : WebConfigurationManager.AppSettings["FolderFileUpdate"].ToString();

        //Mat khau mac dinh khi reset tai khoan
        public static string DefaultPassword = WebConfigurationManager.AppSettings["DefaultPassword"] ?? "123";

    }
}