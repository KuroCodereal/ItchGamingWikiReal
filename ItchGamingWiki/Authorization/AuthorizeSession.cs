using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItchGamingWiki.ItchCommon;
using System.IO;
using System.Windows.Markup;

namespace ItchGamingWiki.Authorization
{
    public class AuthorizeSession : AuthorizeAttribute, IAuthorizationFilter
    {
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }
            if (filterContext.HttpContext.Session["GET_USER"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(new System.Web.Routing.RouteValueDictionary { { "action", "Index" }, { "controller","LogIn"} });
            }
            //Ghi log lich su qua trinh truy cap
            WriteLogProcessUser(System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values);
            //Kiem tra xem link truy cap co ton tai hay k
            // Do something
            //base.OnAuthorization(filterContext);
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
                System.IO.StreamWriter sw = null;
                sw = new System.IO.StreamWriter(logFile,true);
                string message = string.Empty;
                foreach(var key in route)
                {
                    message += (string.IsNullOrEmpty(message) ? key.ToString() : "/" + key.ToString());
                }
                sw.WriteLine(d.ToString("dd/MM/yyyy hh:mm:ss tt") + ":" + message + "=> ..." + HttpContext.Current.Request.RawUrl + "\r\n");
                sw.Flush();
                sw.Close();
            }
            catch
            {

            }
        }
    }
}