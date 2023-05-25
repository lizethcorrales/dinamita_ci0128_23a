using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdministrarReservasController : Controller
    { 
        
        public IActionResult Reservas()
        {
            List<ReservacionModelo> listaReservas = new List<ReservacionModelo>();
            return View(listaReservas);
        }
    }
}
