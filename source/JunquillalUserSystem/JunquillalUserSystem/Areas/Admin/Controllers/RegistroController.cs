using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegistroController : Controller
    {
        //private LoginHandler handlerLogin = new LoginHandler();

        public IActionResult Registro()
        {
            ViewData["Puesto"] = HttpContext.Session.GetString("_Puesto");
            return View();
        }

        [HttpPost]
        public IActionResult Registro(TrabajadorModelo empleado)
        {
            ViewData["Puesto"] = HttpContext.Session.GetString("_Puesto");
            return View();
        }
    }
}
