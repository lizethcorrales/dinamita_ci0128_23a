using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private string usuario = "";
        private string puesto = "";
        public IActionResult Index()
        {
            usuario = HttpContext.Session.GetString("_Nombre");
            puesto = HttpContext.Session.GetString("_Puesto");
            TempData["IsAdminArea"] = "Admin"; // O establece el valor según corresponda
            ViewData["Nombre"] = usuario;
            ViewData["Puesto"] = puesto;
            return View();
        }
    }
}
