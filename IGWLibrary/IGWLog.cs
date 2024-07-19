using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    public class IGWLog
    {
        // Ghi logs thong tin chuong trinh
        /*
         <param name="Msg">Noi dung</param>
        name="ExtensionFile": Them duoi file log mo rong (neu co)
        name="GuildSession": Session de phan biet cac lan ghi log neu co(duoc ghi trong noi dung file log)
         */

        public static void WriteLog(string Msg, string ExtensionFile = "", string GuiIdSession = "")
        {
            try
            {
                ExtensionFile = (string.IsNullOrEmpty(ExtensionFile) ? string.Empty : "_" + ExtensionFile);
                string pathFolderLog = AppDomain.CurrentDomain.BaseDirectory + "FileLogs";
                if (!Directory.Exists(pathFolderLog))
                {
                    Directory.CreateDirectory(pathFolderLog);
                }
                string ngayLog = IGWCommon.DateToStringDdMmYyyy(DateTime.Now);
                StreamWriter sw = new StreamWriter(pathFolderLog + "\\Log_" + ngayLog + ExtensionFile + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString("G") + ":" + GuiIdSession + Msg);
                sw.Flush();
                sw.Close();
            }
            catch { }
        }

        ///<summary>
        ///Ghi log noi dung Exception
        ///</summary>
        ///<param name="ex">Exception Object</param>
        ///<param name="ExtensionFile">Them duoi file log mo rong (neu co)</param>
        ///<param name="GuiIdSession">Session de phan biet cac lan ghi log neu co (dc ghi trong noi dung file log)</param>
    
        public static void WriteLogException(Exception ex, string ExtensionFile="",string GuiIdSession = "")
        {
            try
            {
                ExtensionFile = (string.IsNullOrEmpty(ExtensionFile) ? "" : "_" + ExtensionFile);
                string pathFolderLog = AppDomain.CurrentDomain.BaseDirectory + "FileLogs";
                if (!Directory.Exists(pathFolderLog))
                {
                    Directory.CreateDirectory(pathFolderLog);
                }
                string ngayLog = IGWCommon.DateToStringDdMmYyyy(DateTime.Now);
                StreamWriter sw = new StreamWriter(pathFolderLog + "\\LogException_" + ngayLog + ExtensionFile + ".txt", true);
                sw.WriteLine(DateTime.Now.ToString("G") + ":" + GuiIdSession + ex.Source + ";" + ex.Message + ";" + ex.StackTrace);
                //sw.WriteLine(DateTime.Now.ToString("G") + ":" + JsonConvert.SerializeObject(ex));
                sw.Flush();
                sw.Close();
            }
            catch {}
        }

        ///<summary>
        ///Ghi log noi dung DBEntityValidationException
        ///</summary>
        ///<param name="ex">DBEntityValidationException Obj</param>
        ///<param name="ExtensionFile">Them duoi file log mo rong (neu co)</param>
        ///<param name="GuiIdSession">Session de phan biet cac lan ghi log neu co (dc ghi trong noi dung file log)</param>
        ///

        public static void WriteLogException(DbEntityValidationException ex, string ExtensionFile = "", string GuiIdSession = "")
        {
            try
            {
                ExtensionFile = (string.IsNullOrEmpty(ExtensionFile) ? "" : "_" + ExtensionFile);
                string pathFolderLog = AppDomain.CurrentDomain.BaseDirectory + "FileLogs";
                if (!Directory.Exists(pathFolderLog))
                {
                    Directory.CreateDirectory(pathFolderLog);
                }
                string ngayLog = IGWCommon.DateToStringDdMmYyyy(DateTime.Now);
                StreamWriter sw = new StreamWriter(pathFolderLog + "\\LogException_" + ngayLog + ExtensionFile + ".txt", true);
                string errorValidationException = string.Empty;
                foreach(var valid in ex.EntityValidationErrors)
                {
                    foreach(var item in valid.ValidationErrors)
                    {
                        string error = item.PropertyName + "/" + item.ErrorMessage;
                        errorValidationException += string.IsNullOrEmpty(errorValidationException) ? error : error + " \n";
                    }
                }

                errorValidationException = ex.Message + " \n" + errorValidationException;
                sw.WriteLine(DateTime.Now.ToString("G") + ":" + GuiIdSession + ex.Source + ";" + ex.Message + ";" + ex.StackTrace);
                //sw.WriteLine(DateTime.Now.ToString("G") + ":" + JsonConvert.SerializeObject(ex));
                sw.Flush();
                sw.Close();
            }
            catch { }
        }
    }

}
 