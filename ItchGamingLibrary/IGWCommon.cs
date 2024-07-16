using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    public class IGWCommon
    {
        // Convert DateTime to string ddMMyyyy
        public static string DateToStringDdMmYyyy(DateTime dateTime)
        {
            if (dateTime == null) return string.Empty;
            //them pad (dem them vao ben trai)
            //neu length string la 8 thi padleft(9) la them ben trai 1 khoang trang
            //padleft(2,'0') -> return original string
            return string.Format("{0}{1}{2}",dateTime.Day.ToString().PadLeft(2,'0'),dateTime.Year.ToString());

            
            
        }

        //Convert datetime to YyMmDdHhMmSs
        public static string DateToYyMmDdHhMmSs(DateTime dateTime)
        { 
            if(dateTime == null) return String.Empty;
            return dateTime.Year.ToString().Substring(2, 2) + dateTime.Month.ToString().PadLeft(2, '0') + dateTime.Day.ToString().PadLeft(2, '0')
                + dateTime.Hour.ToString().PadLeft(2, '0') + dateTime.Minute.ToString().PadLeft(2, '0') + dateTime.Second.ToString().PadLeft(2, '0');
        }

        //Convert datetime to YyyyMmDdHhMmSs
        public static string DateToYyyyMmDdHhMmSs(DateTime dateTime)
        {
            if (dateTime == null) return String.Empty;
            return dateTime.Year.ToString() + dateTime.Month.ToString().PadLeft(2, '0') + dateTime.Day.ToString().PadLeft(2, '0')
                + dateTime.Hour.ToString().PadLeft(2, '0') + dateTime.Minute.ToString().PadLeft(2, '0') + dateTime.Second.ToString().PadLeft(2, '0');
        }

        //Chuyen string dang dd/mm/yyyy sang kieu Datetime
        /*
        bStatus: True:Success False:Fail
        strDate: Chuoi truyen vao
        type: 0: mac dinh, =1: tu ngay, =2: den ngay
        return DateTime?
         */
        
        public static DateTime? ConvertStringToDateSearch(out bool bStatus, string strDate, int type = 0)
        {
            int typeTuNgay = 1, typeDefault = 0;
            if (string.IsNullOrEmpty(strDate))
            {
                bStatus = true;
                return null;
            }
            try
            {
                bStatus = true;
                strDate = strDate.Trim();
                DateTime? dResult = null;
                if (type == typeDefault)
                {
                    dResult = Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("vi-VN").DateTimeFormat);
                    return dResult;
                }
                strDate = strDate + (type == typeTuNgay ? "23:59:59" : "00:00:00");
                dResult = Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("vi-VN").DateTimeFormat);
                dResult = (type == typeTuNgay ? dResult.Value.AddDays(-1) : dResult.Value.AddDays(1));
                return dResult;
            }
            catch
            {
                bStatus = false;
                return null;
            }
        }

        /*
         strDate: Gia tri dau vao co dang dd/mm/yyyy hoac MM/DD/YYYY
        IsDdMmYyyy: gia tri dau vao la dd/mm/yyyy?
        <returns>DateTime</returns>
         */

        public static DateTime? ConvertStringToDateTime(string strDate,bool IsDdMmYyyy = true)
        {
            if (string.IsNullOrEmpty(strDate))
            {
                return null;
            }
            try
            {
                if(IsDdMmYyyy) // format la MM/DD/YYYY HH:MM:SS
                {
                    return Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("vi-VN").DateTimeFormat);
                }
                return Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("en-US").DateTimeFormat);
            }
            catch
            {
                return null;
            }
        }

        /*
         chuyen chuoi dang dd/mm/yyyy sang kieu chuoi truy van trong Sql yyyy-mm-dd hh:mm:ss
         */
        public static string ConvertStringToDateSql(out bool bStatus, string strDate, int type = 0)
        {
            int typeTuNgay = 1, typeDefault = 0;
            if (string.IsNullOrEmpty(strDate))
            {
                bStatus = true;
                return null;
            }
            try
            {
                bStatus = true;
                strDate = strDate.Trim();
                DateTime dResult = DateTime.Now;
                if (type == typeDefault)
                {
                    dResult = Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("vi-VN").DateTimeFormat);
                }
                else
                {
                    strDate = strDate + (type == typeTuNgay ? "23:59:59" : "00:00:00");
                    dResult = Convert.ToDateTime(strDate, new System.Globalization.CultureInfo("vi-VN").DateTimeFormat);
                    dResult = (type == typeTuNgay ? dResult.AddDays(-1) : dResult.AddDays(1));
                }
                return string.Format("{0}-{1}-{2} {3}:{4}:{5}", dResult.Year.ToString(),
                    dResult.Month.ToString(),
                    dResult.Day.ToString(),
                    dResult.Hour.ToString().PadLeft(2, '0'),
                    dResult.Minute.ToString().PadLeft(2, '0'),
                    dResult.Second.ToString().PadLeft(2, '0'));
            }
            catch
            {
                bStatus = false;
                return string.Empty;
            }
        }

        /*
         Convert DateTime to SQl String
        type: =0:yyyy-MM-dd hh:mm:ss, =1: MM/dd/yyyy hh:mm:ss
        return string
         */

        public static string ToStringDateSql(this DateTime dateTime, int type=0)
        {
            try
            {
                if(type == 0)
                {
                    return dateTime.ToString("yyyy-MM-dd hh:mm:ss");
                }
                return dateTime.ToString("MM/dd/yyyy hh:mm:ss");
            }
            catch
            {
                return string.Empty;
            }
        }


    }
}
