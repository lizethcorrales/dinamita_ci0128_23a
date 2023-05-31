using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;


namespace JunquillalUserSystem.Controllers
{
    [Area("Admin")]
    public class TarifasController : Controller
    {
        public IActionResult Tarifas()
        {
            TarifasHandler tarifasHandler = new TarifasHandler();
            var tarifas = tarifasHandler.obtenerTarifasActuales();
            return View(tarifas);
        }

        [HttpGet]
        public ActionResult EditarTarifa(string? nacionalidad, string? poblacion, string? actividad)
        {
            ActionResult vista;
            try
            {
                var tarifasHandler = new TarifasHandler();
                var tarifa = tarifasHandler.obtenerTarifasActuales().Find(model => model.Nacionalidad == nacionalidad && model.Poblacion == poblacion && model.Actividad == actividad);
                if (tarifa == null)
                {
                    vista = RedirectToAction("Tarifas");
                }
                else
                {
                    vista = View(tarifa);
                }

            }
            catch
            {
                vista = RedirectToAction("Tarifas");
            }
            return vista;
        }

        [HttpPost]
        public ActionResult EditarTarifa(TarifaModelo tarifa)
        {
            try
            {
                var tarifasHandler = new TarifasHandler();
                tarifasHandler.actualizarPrecioTarifas(tarifa);
                return RedirectToAction("Tarifas", "Tarifas");
            }
            catch
            {
                return View();
            }
        }
    }
}
