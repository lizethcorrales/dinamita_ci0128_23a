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

            //var reservedDates = reservacionHandler.BuscarDiasNoDisponibles(reservacion);
            //ViewBag.reservedDates = reservedDates;
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
            reservacion = reservacion.LlenarPlacasResarva(reservacion, Request.Form);
            hospedero = hospedero.LlenarHospedero(Request.Form);
            string confirmacion = metodosGenerales.CrearConfirmacionMensaje(reservacion, hospedero);
            metodosGenerales.EnviarEmail(confirmacion, hospedero.Email);
            ViewBag.mensaje = new HtmlString(confirmacion);
            //reservacionHandler.InsertarEnBaseDatos(hospedero, reservacion);
            //ViewBag.costoTotal = reservacionHandler.CostoTotal(reservacion.Identificador).ToString();



            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }


    }
}
