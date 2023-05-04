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
    
           ReservacionModelo reservacion = new ReservacionModelo();
           reservacion = reservacionHandler.llenarCantidadPersonas(reservacion, Request.Form);
           TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
           var reservedDates = new[] { "2023-05-02", "2023-05-03", "2023-05-04", "2023-05-30" };
           ViewBag.reservedDates = reservedDates;
            
           return View();

        }


        [HttpPost]
        public IActionResult Reservaciones()
        {

            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion.PrimerDia = Request.Form["fecha-entrada"];  
            //System.Diagnostics.Debug.Write($"Cantidad de personas: {reservacion.Identificador}");
            //var fechaEntrada = Request.Form["fecha-entrada"];
            // var fechaSalida = Request.Form["fecha-salida"];
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);

            return View();
        }

        [HttpPost]
        public IActionResult FinalizarReserva()
        {

            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            System.Diagnostics.Debug.Write($"Reservacion adulto Nacional : {reservacion.cantTipoPersona[0]}");
            System.Diagnostics.Debug.Write($"Reservacion ninno Nacional : {reservacion.cantTipoPersona[1]}");
            System.Diagnostics.Debug.Write($"Reservacion adulto extranjero : {reservacion.cantTipoPersona[2]}");
            System.Diagnostics.Debug.Write($"Reservacion ninno extranjero : {reservacion.cantTipoPersona[3]}");



            return View();
        }
    }
}
