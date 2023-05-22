using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace JunquillalUserSystem.Controllers
{
    public class VisitaController : Controller
    {
        public IActionResult FormularioCantidadPersonas()
        {
            ViewBag.TipoTurista = "visita";
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }

        [HttpPost]
        public IActionResult Calendario()
        {
            ReservacionModelo reservacion = new ReservacionModelo();
            //reservacion = reservacionHandler.LlenarCantidadPersonas(reservacion, Request.Form);
            //TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);

            //var reservedDates = reservacionHandler.BuscarDiasNoDisponibles(reservacion);
            //ViewBag.reservedDates = reservedDates;
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }

    }
}
