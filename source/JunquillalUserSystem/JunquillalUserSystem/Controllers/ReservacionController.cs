using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Numerics;
using System.Text.Json;
using JunquillalUserSystem.Handlers;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Html;

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
            reservacion = reservacionHandler.LlenarCantidadPersonas(reservacion,Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);

            var reservedDates = reservacionHandler.BuscarDiasNoDisponibles(reservacion);
            ViewBag.reservedDates = reservedDates;
            return View();
        }


        [HttpPost]
        public IActionResult Reservaciones()
        {
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacionHandler.LlenarFechas(reservacion,Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            return View();
        }

        [HttpPost]
        public IActionResult FinalizarReserva()
        {
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacionHandler.LlenarInformacionResarva(reservacion,Request.Form);
            HospederoModelo hospedero = reservacionHandler.LlenarHospedero(Request.Form);
            string confirmacion = reservacionHandler.CrearConfirmacionMensaje(reservacion,hospedero);
            reservacionHandler.EnviarEmail(confirmacion,hospedero.Email);
            ViewBag.mensaje = new HtmlString(confirmacion);


            reservacionHandler.InsertarEnBaseDatos(hospedero,reservacion);
            ViewBag.costoTotal = reservacionHandler.CostoTotal(reservacion.Identificador).ToString();




            return View();
        }
        
    }
}
