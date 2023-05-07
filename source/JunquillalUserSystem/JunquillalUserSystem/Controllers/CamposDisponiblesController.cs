using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;

namespace JunquillalUserSystem.Controllers
{
    public class CamposDisponiblesController : Controller
    {
        private HandlerCamposDisponibles handlerCampos = new HandlerCamposDisponibles();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CamposDisponibles()
        {
            int resultado = 80 - handlerCampos.ReservasTotal("2023-05-13");
            ViewBag.camposDisponibles = resultado.ToString();
            
            return View();
        }
    }
}
