using Microsoft.AspNetCore.Mvc;

namespace ItchGamingWiki.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }
    }
}
