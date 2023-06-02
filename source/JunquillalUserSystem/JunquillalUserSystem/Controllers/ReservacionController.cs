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
        private MetodosGeneralesModel metodosGenerales = new MetodosGeneralesModel();
        public IActionResult FormularioCantidadPersonas()
        {
            ViewBag.TipoTurista = "reserva";
            ViewData["IsAdminArea"] = TempData["IsAdminArea"] ;
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }

        [HttpPost]
        public IActionResult Calendario()
        {
            ReservacionModelo reservacion = new ReservacionModelo("Camping");
            reservacion = reservacion.LlenarCantidadPersonas(reservacion,Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);

            var reservedDates = reservacionHandler.BuscarDiasNoDisponibles(reservacion);
            ViewBag.reservedDates = reservedDates;
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            ViewBag.tipo = "Camping";
            return View();
        }


        [HttpPost]
        public IActionResult Reservaciones()
        {
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacion.LlenarFechas(reservacion,Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }

        [HttpPost]
        public IActionResult FinalizarReserva()
        {
            HospederoModelo hospedero = new HospederoModelo();
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacion.LlenarInformacionResarva(reservacion,Request.Form);
            hospedero = hospedero.LlenarHospedero(Request.Form);
            string confirmacion = metodosGenerales.CrearConfirmacionMensaje(reservacion,hospedero);
           // metodosGenerales.EnviarEmail(confirmacion,hospedero.email);
            ViewBag.mensaje = new HtmlString(confirmacion);
            reservacionHandler.InsertarEnBaseDatos(hospedero,reservacion);
            ViewBag.costoTotal = reservacionHandler.CostoTotal(reservacion.Identificador).ToString();



            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }
        
    }
}
