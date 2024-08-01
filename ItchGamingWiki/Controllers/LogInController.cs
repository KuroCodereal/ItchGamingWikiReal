using CaptchaMvc.Interface;
using ItchGamingLibrary;
using ItchGamingWiki.ItchCommon;
using ItchGamingWiki.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace ItchGamingWiki.Controllers
{
    public class LogInController : Controller
    {
        private readonly ItchGamingWikiEntities db = new ItchGamingWikiEntities();
        public string LoadCaptcha()
        {
            try
            {
                Bitmap objBitmap = new Bitmap(100, 60);
                Graphics objGraphics = Graphics.FromImage(objBitmap);
                objGraphics.Clear(Color.White);
                Random objRandom = new Random();
                objGraphics.DrawLine(Pens.Black, objRandom.Next(0, 50), objRandom.Next(10, 30), objRandom.Next(0, 200), objRandom.Next(0, 50));
                objGraphics.DrawRectangle(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(0, 20), objRandom.Next(50, 80), objRandom.Next(0, 20));
                objGraphics.DrawLine(Pens.Blue, objRandom.Next(0, 20), objRandom.Next(10, 50), objRandom.Next(100, 200), objRandom.Next(0, 80));
                Brush objBrush = default(Brush);

                //create background style  
                HatchStyle[] aHatchStyles = new HatchStyle[]
                {
                HatchStyle.BackwardDiagonal, HatchStyle.Cross, HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal, HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical,
                HatchStyle.DiagonalBrick, HatchStyle.DiagonalCross, HatchStyle.Divot, HatchStyle.DottedDiamond, HatchStyle.DottedGrid, HatchStyle.ForwardDiagonal, HatchStyle.Horizontal,
                HatchStyle.HorizontalBrick, HatchStyle.LargeCheckerBoard, HatchStyle.LargeConfetti, HatchStyle.LargeGrid, HatchStyle.LightDownwardDiagonal, HatchStyle.LightHorizontal
                };


                ////create rectangular area  
                RectangleF oRectangleF = new RectangleF(0, 0, 300, 300);
                objBrush = new HatchBrush(aHatchStyles[objRandom.Next(aHatchStyles.Length - 3)], Color.FromArgb((objRandom.Next(100, 255)), (objRandom.Next(100, 255)), (objRandom.Next(100, 255))), Color.White);
                objGraphics.FillRectangle(objBrush, oRectangleF);
                //Generate the image for captcha  
                string captchaText = string.Format("{0:X}", objRandom.Next(1000, 9999));
                //add the captcha value in session  
                //Session["CaptchaVerify"] = captchaText;
                Font objFont = new Font("Courier New", 15, FontStyle.Bold);
                //Draw the image for captcha  
                objGraphics.DrawString(captchaText, objFont, Brushes.Black, 20, 20);
                //  objBitmap.Save(HttpContext.Current.Response.OutputStream, ImageFormat.Gif);
                byte[] _bytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    objBitmap.Save(ms, ImageFormat.Bmp);
                    _bytes = ms.ToArray();
                }
                string ImageUrl = "data:image;base64," + Convert.ToBase64String(_bytes);
                return ImageUrl;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //GET: Login
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.captcha = LoadCaptcha();

            return View();
        }

        //POST: Login
        [HttpPost]
        public ActionResult Index(string captchaText,string Username, string Password)
        {

            User us = SessionHelper.getSession();
            if (us != null)
            {
                return View();
            }

            if (!string.IsNullOrEmpty(captchaText))
            {
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
            if (GetStatusLogIn(accountToken.Name, accountToken.UserData, ref getBCryptPass, KEYWORD_PAGE.Yes))
            {
                if (IGWCrypt.verifyBcrypt(accountToken.Name, getBCryptPass))
                {
                    return Redirect("/SYSUser/ChangePassword?defaultPassword=" + KEYWORD_PAGE.defaultPassDescription);
                }
                return RedirectToAction("Index", "Home");
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

            if (accUser.Enable != KEYWORD_PAGE.Yes)
            {
                refMessage = "Account <b>" + Username + "</b> still locked, More details if you ";
                return false;
            }

            User userSession = new User(accUser, db);
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
            if (isRemember == KEYWORD_PAGE.Yes)
            {
                int numberOfDayRememberAccount = 3;
                if (getTokenRemember == null)
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
                if (getTokenRemember != null)
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