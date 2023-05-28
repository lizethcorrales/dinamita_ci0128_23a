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
            List<ReservacionModelo> listaReservas = administrarHandler.ObtenerReservasPorFecha("2023-05-20");
            return View(listaReservas);
        }
    }
}
