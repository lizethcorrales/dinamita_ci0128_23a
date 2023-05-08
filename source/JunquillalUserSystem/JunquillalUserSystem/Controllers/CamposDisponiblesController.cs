using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;

namespace JunquillalUserSystem.Controllers
{
    public class CamposDisponiblesController : Controller
    {
        private HandlerCamposDisponibles handlerCampos = new HandlerCamposDisponibles();
        private CamposDisponiblesModel camposModelo = new CamposDisponiblesModel();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CamposDisponibles()
        {

            ViewBag.camposDisponibles = TempData["camposDisponibles"];
            
            //int resultado = 80 - handlerCampos.ReservasTotal("2023-05-13");
            //ViewBag.camposDisponibles = resultado.ToString();
            
            return View();
        }

        [HttpPost]
        public IActionResult Edit()
        {
            camposModelo = handlerCampos.LlenarFecha(camposModelo, Request.Form);
            int resultado = 80 - handlerCampos.ReservasTotal(camposModelo.fecha);
            TempData["camposDisponibles"] = resultado.ToString();
            return RedirectToAction("CamposDisponibles");
        }


    }
}
