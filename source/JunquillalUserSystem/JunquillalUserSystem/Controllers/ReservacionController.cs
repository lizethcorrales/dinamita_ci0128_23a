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
            reservacion = reservacionHandler.LlenarCantidadPersonas(reservacion,Request.Form);
            TempData["Reservacion"] = JsonSerializer.Serialize(reservacion);
            
          
            var reservedDates = new[] { "2023-05-02", "2023-05-03", "2023-05-04", "2023-05-30" };
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

            System.Diagnostics.Debug.Write($"Reservacion identificador : {reservacion.Identificador}\n");
            System.Diagnostics.Debug.Write($"Reservacion adulto Nacional : {reservacion.cantTipoPersona[0]}\n");
            System.Diagnostics.Debug.Write($"Reservacion ninno Nacional : {reservacion.cantTipoPersona[1]}\n");
            System.Diagnostics.Debug.Write($"Reservacion adulto extranjero : {reservacion.cantTipoPersona[2]}\n");
            System.Diagnostics.Debug.Write($"Reservacion ninno extranjero : {reservacion.cantTipoPersona[3]}\n");
            System.Diagnostics.Debug.Write($"Reservacion  primerDia : {reservacion.PrimerDia}\n");
            System.Diagnostics.Debug.Write($"Reservacion  ultimoDia : {reservacion.UltimoDia}\n");

            for(int i = 0; i < reservacion.placasVehiculos.LongCount(); i++){

                System.Diagnostics.Debug.Write($"Placa{i} : {reservacion.placasVehiculos[i]}\n");

            }


            System.Diagnostics.Debug.Write($"\n\nHospedero Nombre: {hospedero.Nombre}\n");
            System.Diagnostics.Debug.Write($"Hospedero Apellido1: {hospedero.Apellido1}\n");
            System.Diagnostics.Debug.Write($"Hospedero Apellido2: {hospedero.Apellido2}\n");
            System.Diagnostics.Debug.Write($"Hospedero Identificacion: {hospedero.Identificacion}\n");
            System.Diagnostics.Debug.Write($"Hospedero Nacionalidad: {hospedero.Nacionalidad}\n");
            System.Diagnostics.Debug.Write($"Hospedero Estado: {hospedero.Estado}\n");
            System.Diagnostics.Debug.Write($"Hospedero Telefono: {hospedero.Telefono}\n");
            System.Diagnostics.Debug.Write($"Hospedero Email: {hospedero.Email}\n");
            System.Diagnostics.Debug.Write($"Hospedero TipoIdentificacion: {hospedero.TipoIdentificacion}\n");
            System.Diagnostics.Debug.Write($"Hospedero Motivo: {hospedero.Motivo}\n");

            return View();
        }
    }
}
