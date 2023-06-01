using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JunquillalUserSystem.Controllers
{
    public class VisitaController : Controller
    {
        private MetodosGeneralesModel metodosGenerales = new MetodosGeneralesModel();
        private VisitaHandler visitaHandler = new VisitaHandler();
        public IActionResult FormularioCantidadPersonas()
        {
         
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }

        [HttpPost]
        public IActionResult Calendario()
        {
            ReservacionModelo reservacion = new ReservacionModelo("Picnic");
            reservacion = reservacion.LlenarCantidadPersonas(reservacion, Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);

            var reservedDates = visitaHandler.BuscarDiasNoDisponiblesVisita(reservacion);
            ViewBag.reservedDates = reservedDates;
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            
            return View();
        }

        [HttpPost]
        public IActionResult Reservaciones()
        {
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacion.LlenarFechas(reservacion, Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            ViewBag.Tipo = "Picnic";
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }

        [HttpPost]
        public IActionResult FinalizarReserva()
        {
            HospederoModelo hospedero = new HospederoModelo();
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacion.LlenarInformacionResarva(reservacion, Request.Form);
            hospedero = hospedero.LlenarHospedero(Request.Form);
            visitaHandler.InsertarEnBaseDatosVisita(hospedero, reservacion);
            ViewBag.costoTotal = visitaHandler.CostoTotal(reservacion.Identificador).ToString();
            List<PrecioReservacionDesglose> desglose = visitaHandler.obtenerDesgloseReservaciones(reservacion.Identificador);
            string confirmacion = metodosGenerales.CrearConfirmacionMensaje(reservacion, hospedero, desglose);
            metodosGenerales.EnviarEmail(confirmacion, hospedero.Email);
            ViewBag.mensaje = new HtmlString(confirmacion);
           



            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }


    }
}
