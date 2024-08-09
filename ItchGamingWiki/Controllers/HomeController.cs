using ItchGamingWiki.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ItchGamingWiki.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Index1()
        {
            return View();
        }

        public ActionResult Index2()
        {
            var webClient = new WebClient();
            ItchGamingWikiEntities itchGamingWikiEntities1 = new ItchGamingWikiEntities();
            IEnumerable<SYSUser> a = itchGamingWikiEntities1.SYSUsers.ToList().Where(x => x.Username == "hoan");
            //how to upload image from address link (from other url)
            byte[] imageBytes = null;
            List<string> list = new List<string>();
            foreach (var user in a)
            {
                imageBytes = webClient.DownloadData(user.Image);
                string strBase64 = Convert.ToBase64String(imageBytes);
                string url = "data:Image/png;base64," + strBase64;
                list.Add(url);
            }

            //imageBytes = webClient.DownloadData("https://dfstudio-d420.kxcdn.com/wordpress/wp-content/uploads/2019/06/digital_camera_photo-1080x675.jpg");
            //imageBytes = webClient.DownloadData("C:\\Users\\hoandv\\Desktop\\2.jpg");
            //string strBase64 = Convert.ToBase64String(imageBytes);
            //string url = "data:Image/png;base64,"+strBase64;
           
            ViewBag.list = list;
            return View();
        }
        public ActionResult NhanVien()
        {
            return View();
        }

        public ActionResult Index3()
        {
            return View();
        }
    }
}