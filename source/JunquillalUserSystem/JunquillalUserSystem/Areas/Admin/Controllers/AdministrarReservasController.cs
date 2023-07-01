using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Handlers;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdministrarReservasController : Controller
    {

        private AdministrarReservasHandler administrarHandler = new AdministrarReservasHandler();
        private CheckInOutHandler checkInOutHandler  = new CheckInOutHandler();
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

        public IActionResult CheckOutReserva(string identificador)
        {
          
            checkInOutHandler.CheckInOutReserva(identificador,"CheckOut");
            return RedirectToAction("Reservas");

        }

        public IActionResult CheckInReserva(string identificador)
        {

            List<ReservacionModelo> listaReservas = administrarHandler.ObtenerReservas(identificador, "identificador");
            ReservacionModelo reservacion = listaReservas.Find(model => model.Identificador == identificador);

            return View(reservacion);

        }

        public IActionResult RealizarCheckIn()
        {
            int numeroParcela = int.Parse(Request.Form["numeroParcela"]);
            string identificador = Request.Form["identificador"];

            checkInOutHandler.CheckInOutReserva(identificador, "CheckIn");
            checkInOutHandler.insertarHospedaje(identificador, numeroParcela);
            


            return RedirectToAction("Reservas");

        }

    }
}
