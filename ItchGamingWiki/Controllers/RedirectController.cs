using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ItchGamingWiki.Controllers
{
    public class RedirectController : Controller
    {
        // GET: Redirect

        public ActionResult Notification()
        {
            return View();
        }

        public ActionResult PageNotExist()
        {
            return View();
        }

        public ActionResult PageError(string Content = "")
        {
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }
    }
}