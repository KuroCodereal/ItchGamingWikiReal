using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItchGamingLibrary
{
    class IGWAntiSqlInjection
    {
        public static Boolean CheckSqlInjection(string str)
        {
            str = (string.IsNullOrEmpty(str) ? string.Empty : str);
            str = str.ToLower();
            string[] arrayInjection = new string[] { "--",";","create","select","insert","update","delete","drop","truncate","alter","execute","exec","backup","restore","waitfor","delay","database","table" };
            foreach(string item in arrayInjection)
            {
                if (str.Contains(item)) return false;
            }
            return true;
        }

        public static string RemoveStringInjection(string str)
        {
            str = (string.IsNullOrEmpty(str) ? string.Empty : str);
            str.Replace("'","\"");
            return str;
        }
    }
}
