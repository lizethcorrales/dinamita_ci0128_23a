using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using JunquillalUserSystem.Handlers;

namespace JunquillalUserSystem.Controllers
{
    public class ReservacionController : Controller
    {
        private ReservacionHandler reservacionHandler = new ReservacionHandler();
        public IActionResult FormularioCantidadPersonas()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Calendario()
        {
    
            System.Diagnostics.Debug.Write(reservacionHandler.CostoTotal("9149985005"));
            ReservacionModelo reservacion = new ReservacionModelo();
           reservacion.Identificador = int.Parse(Request.Form["cantidad_Adultos_Nacional"]);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            // Imprimir los valores recibidos del formulario
          
            var reservedDates = new[] { "2023-05-02", "2023-05-03", "2023-05-04", "2023-05-30" };
            ViewBag.reservedDates = reservedDates;
            return View();

        }


        [HttpPost]
        public IActionResult Reservaciones()
        {

            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            System.Diagnostics.Debug.Write($"Cantidad de personas: {reservacion.Identificador}");
            var fechaEntrada = Request.Form["fecha-entrada"];
            var fechaSalida = Request.Form["fecha-salida"];
            System.Diagnostics.Debug.Write($"Fecha entrada: {fechaEntrada}");
            System.Diagnostics.Debug.Write($"Fecha salida: {fechaSalida}");


            return View();
        }
    }
}
