using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;

namespace JunquillalUserSystemTest.Handlers
{
    [TestClass]
    public class TestsCheckInOut
    {
        private CheckInOutHandler checkInOutHandler = new CheckInOutHandler();
        private HandlerPruebas handlerPruebas = new HandlerPruebas();

        /*
         * UPDATE Reservacion
           SET Reservacion.Estado = 0
           WHERE Reservacion.IdentificadorReserva ='MX4HW9HYED';
         * */
        [TestMethod]
        public void hacerCheckInDeUnaReservaValida()
        {
            string identificadorReserva = "MX4HW9HYED";
            int resultadoCheckIn = 1;

            checkInOutHandler.CheckInOutReserva(identificadorReserva, "CheckIn");

            List<ReservacionModeloPruebas> reservacionModelos = handlerPruebas.obtenerReservacion(identificadorReserva);

            Assert.AreEqual(Convert.ToInt32(reservacionModelos[0].Estado), resultadoCheckIn);
        }

        /*
         * DELETE Hospedaje
            WHERE Hospedaje.IdentificadorReserva = 'MX4HW9HYED';
         */

        [TestMethod]
        public void asignarParcelaValidaAUnaReservacion()
        {
            string identificadorReserva = "MX4HW9HYED";
            int parcelaEsperada = 1;
            checkInOutHandler.insertarHospedaje(identificadorReserva, 1);

            List<HospedajePruebas> hospedaje = handlerPruebas.obtenerParcelaHospedaje(identificadorReserva);

            Assert.AreEqual(hospedaje[0].IdentificadorReserva, identificadorReserva);
            Assert.AreEqual(Convert.ToInt32(hospedaje[0].NumeroParcela), parcelaEsperada);
        }

        [TestMethod]
        public void asignarParcelaNoValidaAUnaReservacion()
        {
            string identificadorReserva = "MX4HW9HYED";
            int contadorReservaConParcelaInvalida = 0;
            checkInOutHandler.insertarHospedaje(identificadorReserva, 15);

            List<HospedajePruebas> hospedaje = handlerPruebas.obtenerParcelaHospedaje(identificadorReserva);
            for(int i = 0; i < hospedaje.Count; i++)
            {
                if (hospedaje[i].NumeroParcela.Equals(15))
                {
                    contadorReservaConParcelaInvalida++;
                }
            }

            Assert.AreEqual(contadorReservaConParcelaInvalida, 0);
        }

        [TestMethod]
        public void asignarParcelaValidaAUnaReservacionNoExistente()
        {
            string identificadorReserva = "MX4HW9Hyui";
            checkInOutHandler.insertarHospedaje(identificadorReserva, 1);

            List<HospedajePruebas> hospedaje = handlerPruebas.obtenerParcelaHospedaje(identificadorReserva);

            Assert.AreEqual(hospedaje.Count, 0);
        }

        /*
        * UPDATE Reservacion
          SET Reservacion.Estado = 0
          WHERE Reservacion.IdentificadorReserva ='MX4HW9HYED';
        * */
        [TestMethod]
        public void hacerCheckOutDeUnaReservaValida()
        {
            string identificadorReserva = "MX4HW9HYED";
            int resultadoCheckOut = 3;

            checkInOutHandler.CheckInOutReserva(identificadorReserva, "CheckOut");

            List<ReservacionModeloPruebas> reservacionModelos = handlerPruebas.obtenerReservacion(identificadorReserva);

            Assert.AreEqual(Convert.ToInt32(reservacionModelos[0].Estado), resultadoCheckOut);
        }
    }
}
