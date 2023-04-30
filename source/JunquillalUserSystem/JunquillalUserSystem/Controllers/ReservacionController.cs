using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace JunquillalUserSystem.Controllers
{
    public class ReservacionController : Controller
    {
        public IActionResult Calendario()
        {
            var reservedDates = new[] {"2023-05-02", "2023-05-03", "2023-05-04" };
            ViewBag.reservedDates = reservedDates;
            return View();
            
        }
    }
}
