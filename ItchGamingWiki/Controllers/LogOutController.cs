using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ItchGamingWiki.Authorization;
using ItchGamingWiki.ItchCommon;

namespace ItchGamingWiki.Controllers
{
    public class LogOutController : Controller
    {
        // GET: LogOut
        public ActionResult Index()
        {
            return View();
        }

        [AuthorizeSession]
        public ActionResult LogOut()
        {
            Session["GET_USER"] = null;
            Session["IS_TOGGLE_VERTICAL_MENU_SHOW"] = null;
            Session["THONG_BAO"] = null;
            //SessionHelper.setSession = null;
            Session.Remove("GET_USER");
            Session.RemoveAll();
            Session.Clear();
            HttpContext.Request.Cookies.Clear();
            HttpContext.Session.Abandon();
            HttpContext.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId",null));
            HttpContext.Response.Cookies.Add(new HttpCookie("__RequestVerificationToken", null));
            HttpContext.Response.Cookies.Add(new HttpCookie(KEYWORD_PAGE.TokenRememberName, null));
            ClearCookieAll();
            return RedirectToAction("Index","LogIn");
        }

        private void ClearCookieAll()
        {
            int limit = Request.Cookies.Count;
            HttpCookie aCookie; //Instantiate a cookie placeholder
            string cookieName;
            for (int i = 0; i < limit; i++)
            {
                cookieName = Request.Cookies[i].Name; //Get the name of the current cookie
                aCookie = new HttpCookie(cookieName); //create a new cookie with the same name as the one you're deleting
                aCookie.Value = ""; //set a blank value to the cookie
                aCookie.Expires = DateTime.Now.AddDays(-1); //Setting the expiration date in the past deletes the cookie
                Response.Cookies.Add(aCookie); //Set the cookie to delete it
            }
        }
    }
}