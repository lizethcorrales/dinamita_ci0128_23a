using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;


namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdministrarReservasController : Controller
    {

        private AdministrarReservasHandler administrarHandler = new AdministrarReservasHandler();

        public IActionResult Reservas()
        {
            List<ReservacionModelo> listaReservas = new List<ReservacionModelo>();
            return View(listaReservas);
        }

        public IActionResult ReservasPorFecha(string fecha)
        {
            List<ReservacionModelo> listaReservas = administrarHandler.ObtenerReservas(fecha,"fecha");
            return View("Reservas", listaReservas);
        }

        public IActionResult ReservasPorIdentificador(string identificador)
        {
            List<ReservacionModelo> listaReservas = administrarHandler.ObtenerReservas(identificador,"identificador");
            return View("Reservas", listaReservas);
        }

        public IActionResult EliminarReserva(string identificador)
        {
            administrarHandler.EliminarReservacion(identificador);
            return RedirectToAction("Reservas");

        }

    }
}
