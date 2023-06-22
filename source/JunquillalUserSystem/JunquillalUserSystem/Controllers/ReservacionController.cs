using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using JunquillalUserSystem.Handlers;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Html;
using JunquillalUserSystem.Models.Dependency_Injection;
using JunquillalUserSystem.Models.Patron_Bridge;

namespace JunquillalUserSystem.Controllers
{
    public class ReservacionController : Controller
    {
        private CampingHandler reservacionHandler = new CampingHandler();
        

        // Dependency Injection de servicio email
        private readonly IEmailService _emailService;

        // Dependency Injection de servicio mensaje htm
        private readonly MensajeConfirmacionImplementacionHTML _mensajeConfirmacionImplementacion;


        /*
         *  el constructor con parámetro en el controlador se utiliza para permitir la 
         *  inyección de dependencias del servicio requerido. 
         */
        public ReservacionController(IEmailService emailService , IMensajeConfirmacionImplementacion mensajeConfirmacionImplementacion )
        {
            _emailService = emailService;
            _mensajeConfirmacionImplementacion = mensajeConfirmacionImplementacion as MensajeConfirmacionImplementacionHTML;
        }
        public IActionResult FormularioCantidadPersonas()
        {

            ReservacionModelo reservacion = new ReservacionModelo("Camping");
            reservacion.tarifas = reservacionHandler.cargarTarifasCamping();
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            ViewBag.TipoTurista = "reserva";
            ViewData["IsAdminArea"] = TempData["IsAdminArea"] ;
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View(reservacion.tarifas);
        }

        [HttpPost]
        public IActionResult Calendario()
        {
            ReservacionModelo reservacion = JsonSerializer.Deserialize<ReservacionModelo>((string)TempData["Reservacion"]);
            reservacion = reservacion.LlenarCantidadPersonas(reservacion,Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);

            var reservedDates = reservacionHandler.BuscarDiasNoDisponiblesReserva(reservacion);
            ViewBag.reservedDates = reservedDates;
            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            ViewBag.Tipo = "Camping";
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
            reservacion.Identificador = reservacionHandler.crearIdentificador(10);
            reservacion = reservacion.LlenarPlacasResarva(reservacion,Request.Form);
            hospedero = hospedero.LlenarHospedero(Request.Form);
            reservacionHandler.InsertarEnBaseDatos(hospedero, reservacion);
            List<PrecioReservacionDesglose> desglose = reservacionHandler.obtenerDesgloseReservaciones(reservacion.Identificador);
            string confirmacion = _mensajeConfirmacionImplementacion.CrearConfirmacionMensaje(reservacion, hospedero, desglose);
            ViewBag.mensaje = new HtmlString(confirmacion);
            //_emailService.EnviarEmail(confirmacion, hospedero.Email);
            ViewBag.costoTotal = reservacionHandler.CostoTotal(reservacion.Identificador).ToString();



            ViewData["IsAdminArea"] = TempData["IsAdminArea"];
            TempData["IsAdminArea"] = TempData["IsAdminArea"];
            return View();
        }
        
    }
}
