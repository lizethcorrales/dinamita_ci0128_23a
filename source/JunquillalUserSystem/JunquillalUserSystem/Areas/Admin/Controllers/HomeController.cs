using Microsoft.AspNetCore.Mvc;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            TempData["IsAdminArea"] = "Admin"; // O establece el valor según corresponda
            return View();
        }
    }
}
