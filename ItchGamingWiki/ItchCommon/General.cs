using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Web;
using System.Data;
using System.Reflection;

namespace ItchGamingWiki.ItchCommon
{
    public class General
    {
        public static void WriteLog(Exception ex, string control, string action)
        {
            try
            {
                User us = SessionHelper.getSession();
                if (!WebConfigKey.IsWriteLog)
                {
                    return;
                }
                string ErrorInfo = ((ex.InnerException != null) ? ex.InnerException.Message + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : "-");
                var st = new StackTrace(ex,true);
                var frame = st.GetFrame(0);
                //add Common namespace
                //CommonLog.ErrorLog.WriteToErrorLog(HttpContext.Current.Server.MapPath("~/FileLog/log.txt"), "[" + us.UserName + "][" + control + "/" + action + "][line" + frame.GetFileLineNumber() + "]:" + ex.Message + "\r\n Inner: " + ErrorInfo, ex.StackTrace, action);
            }
            catch { }
        }

        public static void WriteLog(DbEntityValidationException ex, string control, string action)
        {
            try
            {
                User us = SessionHelper.getSession();
                if (!WebConfigKey.IsWriteLog)
                {
                    return;
                }
                string ErrorInfo = ((ex.InnerException != null) ? ex.InnerException.Message + (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : "") : "-");
                foreach(var validationErrors in ex.EntityValidationErrors)
                {
                    foreach(var validationError in validationErrors.ValidationErrors)
                    {
                        ErrorInfo += "\r\n  " + validationError.PropertyName + "/" + validationError.ErrorMessage;
                    }
                }
                var st = new StackTrace(ex, true);
                var frame = st.GetFrame(0);
                //add Common namespace
                //from VTC intecom GemBox Ltd Jimmy Bogard (AutoMapper GemBox.Spreadsheet)
                //CommonLog.ErrorLog.WriteToErrorLog(HttpContext.Current.Server.MapPath("~/FileLog/log.txt"), "[" + us.UserName + "][" + control + "/" + action + "][line" + frame.GetFileLineNumber() + "]:" + ex.Message + "\r\n Inner: " + ErrorInfo, ex.StackTrace, action);
            }
            catch { return; }
        }

        //Ghi log lich su truy cap cua nguoi dung tren web
        private void WriteLogProcessUser(System.Web.Routing.RouteValueDictionary route)
        {
            try
            {
                DateTime d = DateTime.Now;
                User us = SessionHelper.getSession();
                string pathFolder = HttpContext.Current.Server.MapPath("~/FileLogs");
                if (!Directory.Exists(pathFolder))
                {
                    Directory.CreateDirectory(pathFolder);
                }

                string logFile = HttpContext.Current.Server.MapPath("~/FileLogs/Process_" + us.UserName + ".txt");
                StreamWriter sw = null;
                sw = new StreamWriter(logFile,true);
                string message = string.Empty;
                foreach(var key in route)
                {
                    message += (string.IsNullOrEmpty(message) ? key.ToString() : "/" + key.ToString());
                }
                sw.WriteLine(d.ToString("dd/MM/yyyy hh:mm:ss tt") + ": " + message + "=> ..." + HttpContext.Current.Request.RawUrl + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
        //gui mail
        public void SendMail(string sendTo, string sub, string content, List<Attachment> attachments)
        {
            SmtpClient smtp = new SmtpClient();
            try
            {
                smtp.Host = WebConfigKey.MailHost; //smtp.gmail.com
                smtp.Port = WebConfigKey.MailPort; //587
                smtp.EnableSsl = true;


                //--------------
                /*
                 smtp.Timeout = 200000; //ms ?
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.UseDefaultCredentials = false;
                 */
                //--------------

                smtp.Credentials = new NetworkCredential(WebConfigKey.MailUser, WebConfigKey.MailPass);
                MailMessage mail = new MailMessage(WebConfigKey.MailUser, sendTo, sub, content);
                if (attachments != null)
                {
                    foreach (Attachment attachment in attachments)
                    {
                        mail.Attachments.Add(attachment);
                    }
                }
                mail.IsBodyHtml = true;
                mail.BodyEncoding = UTF8Encoding.UTF8;
                smtp.Send(mail);
            }
            catch
            {
                return;
            }
        }

        //Convert List<T> to DataTable
        public static DataTable ToDataTable<T>(IEnumerable<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column name as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach(T item in items)
            {
                var values = new object[Props.Length];
                for(int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

    }
}