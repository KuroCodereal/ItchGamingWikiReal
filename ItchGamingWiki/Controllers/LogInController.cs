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
            return View();
        }


    }
}