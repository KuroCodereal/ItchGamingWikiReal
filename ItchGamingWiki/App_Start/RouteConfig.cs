using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ItchGamingWiki
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);

            //dang nhap
            routes.MapRoute(
                name: "SignIn",
                url: "Login",
                defaults: new {controller = "Login", action = "Index"}
            );


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "LogIn", action = "Index", id = UrlParameter.Optional }
            );

            #region Quan tri
            routes.MapRoute(
                name: "PageNotExist",
                url: "page-nothing",
                defaults: new { controller = "Redirect", action = "PageNotExist" }
            );

            routes.MapRoute(
                name: "PageError",
                url: "error-page",
                defaults: new {controller = "Redirect", action = "PageError", Content = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Notify",
                url: "Notification",
                defaults: new { controller = "Redirect", action = "Notification" }
            );

            routes.MapRoute(
                name: "LogOut",
                url: "LogOut",
                defaults: new { controller = "LogOut", action = "Logout" }
            );

            routes.MapRoute(
                name: "ChangePass",
                url: "ChangePass",
                defaults: new { controller = "Redirect", action = "ChangePassword", defaultPassword = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "AccountInfo",
                url: "Info",
                defaults: new { controller = "Redirect", action = "Account_Info" }
            );

            routes.MapRoute(
                name: "Manage_User",
                url: "ManageUser",
                defaults: new { controller = "Redirect", action = "ManageUsers" }
            );


            #endregion

            //Menu link
            #region MENU LINK
            routes.MapRoute(
                name: "DS_MENU",
                url: "Menu/page-{pageNumber}",
                defaults: new { controller = "SYSMenu", action = "Menu", pageNumber = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "CREATE_MENU_LINK",
                url: "Add-Menu",
                defaults: new { controller = "SYSMenu", action = "AddNew" }
            );

            routes.MapRoute(
                name: "UPDATE_MENU_LINK",
                url: "Update-Menu/{id}",
                defaults: new { controller = "SYSMenu", action = "Update", id = UrlParameter.Optional }
            );

            #endregion

            #region BAO CAO
            routes.MapRoute(
                name: "Statistics",
                url: "Statistics",
                defaults: new { controller = "Statistics", action = "Index" }
            );
            #endregion
        }
    }
}
