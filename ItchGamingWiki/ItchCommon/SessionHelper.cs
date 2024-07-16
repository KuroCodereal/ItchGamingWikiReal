using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ItchGamingWiki.ItchCommon
{
    public class SessionHelper
    {
        public static void setSession(User us)
        {
            HttpContext.Current.Session["GET_USER"] = us;
        }

        public static User getSession()
        {
            if (HttpContext.Current.Session["GET_USER"] == null) return null;
            else return HttpContext.Current.Session["GET_USER"] as User;
        }
    }
}