using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using System.Reflection;


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
        public IActionResult AgregarTarifa()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AgregarTarifa(TarifaModelo tarifa)
        {
            ViewBag.ExitoAlCrear = false;
            try
            {
                if (ModelState.IsValid)
                {
                    var tarifasHandler = new TarifasHandler();
                    ViewBag.ExitoAlCrear = tarifasHandler.insertarNuevaTarifa(tarifa);
                    if (ViewBag.ExitoAlCrear)
                    {
                        ViewBag.Message = "La tarifa " + tarifa.Poblacion + " fue agregada con éxito";
                        ModelState.Clear();
                    } else
                    {
                        ViewBag.Message = "Algo salió mal y no fue posible agregar la tarifa";
                    }
                }
                return View();

            }
            catch
            {
                ViewBag.Message = "Algo salió mal y no fue posible agregar la tarifa";
                return View();
            }
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

        [HttpGet]
        public ActionResult EliminarTarifa(string? nacionalidad, string? poblacion, string? actividad)
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
        public ActionResult EliminarTarifa(TarifaModelo tarifa)
        {
            try
            {
                var tarifasHandler = new TarifasHandler();
                tarifasHandler.borrarTarifa(tarifa);
                return RedirectToAction("Tarifas", "Tarifas");
            }
            catch
            {
                return View();
            }
        }
    }
}
