using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;

namespace JunquillalUserSystem.Controllers
{
    public class ReservacionController : Controller
    {
        public IActionResult FormularioCantidadPersonas()
        {

            return View();
        }

        [HttpPost]
        public IActionResult Calendario()
        {
          

            // Imprimir los valores recibidos del formulario
            System.Diagnostics.Debug.Write($"Cantidad adultos nacionales: {Request.Form["cantidad_Adultos_Nacional"]}");
            var reservedDates = new[] { "2023-05-02", "2023-05-03", "2023-05-04", "2023-05-30" };
            ViewBag.reservedDates = reservedDates;
            return View();

        }


        [HttpPost]
        public IActionResult Reservaciones(string startDate, string endDate)
        {
            var fechaEntrada = Request.Form["fecha-entrada"];
            var fechaSalida = Request.Form["fecha-salida"];
            System.Diagnostics.Debug.Write($"Fecha entrada: {fechaEntrada}");
            System.Diagnostics.Debug.Write($"Fecha salida: {fechaSalida}");


            return View();
        }
    }
}
