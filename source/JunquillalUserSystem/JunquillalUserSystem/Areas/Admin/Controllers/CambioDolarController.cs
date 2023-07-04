using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace JunquillalUserSystem.Areas.Admin.Views.CambioDolar
{
    [Area("Admin")]
    public class CambioDolarController : Controller
    {
        public IActionResult CambioDolar()
        {
            CambioDolarHandler cambioDolarHandler = new();
            var tarifas = cambioDolarHandler.obtenerValorDeDolarActual();
            return View(tarifas);
        }

        [HttpGet]
        public ActionResult EditarCambioDolar(string? valorDolar)
        {
            ActionResult vista;
            try
            {
                var cambioDolarHandler = new CambioDolarHandler();
                var tarifa = cambioDolarHandler.obtenerValorDeDolarActual();
                if (tarifa == null)
                {
                    vista = RedirectToAction("CambioDolar");
                }
                else
                {
                    vista = View(tarifa);
                }

            }
            catch
            {
                vista = RedirectToAction("CambioDolar");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult EditarCambioDolar(CambioDolarModel cambioDolar)
        {
            ViewBag.ExitoAlEditar = false;
            try
            {
                if (ModelState.IsValid) 
                { 
                    CambioDolarHandler cambioDolarHandler = new();
                    ViewBag.ExitoAlEditar = cambioDolarHandler.modificarValorDeDolarActual(cambioDolar);
                    if (ViewBag.ExitoAlEditar)
                    {
                        return RedirectToAction("CambioDolar", "CambioDolar");
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
    }
}
