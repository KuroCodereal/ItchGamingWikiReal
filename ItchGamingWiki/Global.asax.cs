using ItchGamingWiki.ItchCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace ItchGamingWiki
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //Ẩn X-AspNetMVC-Version trong Response Headers trả về Client
            MvcHandler.DisableMvcResponseHeader = true;
            //Map cac object qua lai
            AutoMapperConfig.Initialize();
            // Để thông báo cho trình duyệt từ chối đưa website của bạn vào khung iframe
            //HttpContext.Current.Response.AddHeader("x-frame-options", "DENY");
            Newtonsoft.Json.JsonConvert.DefaultSettings = () => new Newtonsoft.Json.JsonSerializerSettings
            {
                Formatting = Newtonsoft.Json.Formatting.Indented,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            };
            Z.EntityFramework.Extensions.LicenseManager.AddLicense(KEYWORD_PAGE.ZEFExtensionSQLServerName, KEYWORD_PAGE.ZEFExtensionSQLServerKey);
            Z.EntityFramework.Extensions.LicenseManager.AddLicense(KEYWORD_PAGE.ZBulkOperationSQLServerName,KEYWORD_PAGE.ZBulkOperationSQLServerKey);
        }
        protected void Application_PreSendRequestHeaders()
        {
            Response.Headers.Remove("Server"); //Remove Server Header
            Response.Headers.Remove("X-AspNet-Version"); //Remove X-AspNet-Version
        }
    }
}
