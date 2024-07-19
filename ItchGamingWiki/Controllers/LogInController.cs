using ItchGamingLibrary;
using ItchGamingWiki.ItchCommon;
using ItchGamingWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItchGamingWiki.Controllers
{
    public class LogInController : Controller
    {
        private readonly ItchGamingWikiEntities db = new ItchGamingWikiEntities ();
        //GET: Login
        public ActionResult Index(string captchaText)
        {

            Random random = new Random();
            for (int i = 0; i < 3; i++)
            {
                
            }

            User us = SessionHelper.getSession();
            if(us != null)
            {
                return RedirectToAction("Index","Home");
            }

            if (!string.IsNullOrEmpty(captchaText)){
                ViewBag.captchaError = captchaText;
            }

            HttpCookie getTokenRemember = Request.Cookies[KEYWORD_PAGE.TokenRememberName];
            if (getTokenRemember == null) return View();
            if (string.IsNullOrEmpty(getTokenRemember.Value))
            {
                return View();
            }

            System.Web.Security.FormsAuthenticationTicket accountToken = System.Web.Security.FormsAuthentication.Decrypt(getTokenRemember.Value);

            if (accountToken.Expired)
            {
                return View();
            }

            string getBCryptPass = string.Empty;
            if(GetStatusLogIn(accountToken.Name, accountToken.UserData, ref getBCryptPass, KEYWORD_PAGE.Yes))
            {
                if(IGWCrypt.verifyBcrypt(accountToken.Name, getBCryptPass))
                {
                    return Redirect("/SYSUser/ChangePassword?defaultPassword=" + KEYWORD_PAGE.defaultPassDescription);
                }
                return RedirectToAction("Index","Home");
            }
            return View();
        }

        private bool GetStatusLogIn(string Username, string Password, ref string refMessage, int isRemember = 0)
        {
            var sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            SYSUser accUser = db.SYSUsers.FirstOrDefault(x => x.Username.Trim() == Username.Trim());

            if (accUser == null)
            {
                return false;
            }

            if (!IGWCrypt.verifyBcrypt(Password, accUser.Password))
            {
                return false;
            }

            if(accUser.Enable != KEYWORD_PAGE.Yes)
            {
                refMessage = "Account <b>" + Username + "</b> still locked, More details if you ";
                return false;
            }

            User userSession = new User(accUser,db);
            ApplicationInfo.ApplicationName = userSession.FullName;
            ApplicationInfo.ApplicationName = string.IsNullOrEmpty(ApplicationInfo.ApplicationName) ? ApplicationInfo.ApplicationNameDefault : ApplicationInfo.ApplicationName;
            SessionHelper.setSession(userSession);
            this.SaveCookiesAccountLogin(accUser.Username, Password, isRemember);
            string TIMETAKEN2 = "Time Taken: " + sw.Elapsed.TotalMilliseconds.ToString("#,##0.00 'miliseconds'");
            return true;
        }

        private void SaveCookiesAccountLogin(string userName, string passWord, int isRemember = 0)
        {
            HttpCookie getTokenRemember = Request.Cookies[KEYWORD_PAGE.TokenRememberName];
            if(isRemember == KEYWORD_PAGE.Yes)
            {
                int numberOfDayRememberAccount = 3;
                if(getTokenRemember == null)
                {
                    System.Web.Security.FormsAuthenticationTicket ticket = new System.Web.Security.FormsAuthenticationTicket(
                        1, //ticket version
                        userName,//authenticated username
                        DateTime.Now,//issueDate
                        DateTime.Now.AddDays(numberOfDayRememberAccount),//expiredDate
                        true, //true to persist across browser sessions
                        passWord,//can be used to store additional user data
                        System.Web.Security.FormsAuthentication.FormsCookiePath //the path for the cookie
                        );
                    string encryptedTicket = System.Web.Security.FormsAuthentication.Encrypt(ticket);
                    HttpCookie newCookie = new HttpCookie(System.Web.Security.FormsAuthentication.FormsCookieName, encryptedTicket);
                    newCookie.HttpOnly = true;
                    newCookie.Expires = DateTime.Now.AddDays(numberOfDayRememberAccount);
                    Response.Cookies.Add(newCookie);
                }
                else
                {
                    //getTokenRemember.Expires = DateTime.Now.AddDays(numberOfDayRememberAccount);
                }
            }
            else
            {
                if(getTokenRemember != null)
                {
                    getTokenRemember.Expires = DateTime.Now.AddDays(-1);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}