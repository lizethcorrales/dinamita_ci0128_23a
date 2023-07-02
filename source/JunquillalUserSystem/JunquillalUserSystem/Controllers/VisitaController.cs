using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Models.Dependency_Injection;
using JunquillalUserSystem.Models.Patron_Bridge;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JunquillalUserSystem.Controllers
{
    public class VisitaController : Controller
    {
        
        private PicnicHandler visitaHandler = new PicnicHandler();

        // Dependency Inyection de servio email
        private readonly IEmailService _emailService;

        // Dependency Injection de servicio mensaje htm
        private readonly MensajeConfirmacionImplementacionHTML _mensajeConfirmacionImplementacion;

        /*
         *  el constructor con parámetro en el controlador se utiliza para permitir la 
         *  inyección de dependencias del servicio requerido. 
         */
        public VisitaController(IEmailService emailService, IMensajeConfirmacionImplementacion mensajeConfirmacionImplementacion)
        {
            _emailService = emailService;
            _mensajeConfirmacionImplementacion = mensajeConfirmacionImplementacion as MensajeConfirmacionImplementacionHTML;
        }
        public IActionResult FormularioCantidadPersonas()
        {

            ReservacionModelo reservacion = new ReservacionModelo("Picnic");
            reservacion.tarifas = visitaHandler.cargarTarifasCamping("Picnic");
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View(reservacion.tarifas);
        }

        [HttpPost]
        public IActionResult Calendario()
        {
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
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
            reservacion.Identificador = visitaHandler.crearIdentificador(10);
            reservacion = reservacion.LlenarPlacasResarva(reservacion, Request.Form);
            hospedero = hospedero.LlenarHospedero(Request.Form);
            visitaHandler.transaccionReservaVisita(hospedero, reservacion);
            visitaHandler.reuniciarConexion();
            ViewBag.costoTotal = "₡" + visitaHandler.CostoTotal(reservacion.Identificador).ToString();
            List<PrecioReservacionDesglose> desglose = visitaHandler.obtenerDesgloseReservaciones(reservacion.Identificador);
            string confirmacion = _mensajeConfirmacionImplementacion.CrearConfirmacionMensaje(reservacion, hospedero, desglose);
            //_emailService.EnviarEmail(confirmacion, hospedero.Email);
            ViewBag.mensaje = new HtmlString(confirmacion);
           



            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }


    }
}
