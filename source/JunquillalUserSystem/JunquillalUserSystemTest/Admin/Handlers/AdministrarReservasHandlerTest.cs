/*
 * Pruebas unitarias creadas por Andrés Matarrita C04668
 */

using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Admin.Handlers
{
    [TestClass]
    public  class AdministrarReservasHandlerTest
    {

        /*
         * Comprueba con la base de datos que se 
         * recuperan correctamente las reservaciones 
         * de acuerdo con la fecha que se consulta
         */ 
        [TestMethod]
        public void ObtenerReservasPorFecha_DebeRetornarReservasCorrectas()
        {
            // Arrange
            AdministrarReservasHandler handler = new AdministrarReservasHandler();
            string datoBusqueda = "2023-05-20";
            string tipoBusqueda = "fecha";

            // Act
            List<ReservacionModelo> reservas = handler.ObtenerReservas(datoBusqueda, tipoBusqueda);

            // Assert
            Assert.IsNotNull(reservas);

            if (reservas != null)
            {
                foreach (var reserva in reservas)
                {
                    Assert.IsNotNull(reserva.PrimerDia);
                    Assert.IsNotNull(reserva.UltimoDia);

                    DateTime primerDia;
                    DateTime ultimoDia;

                    Assert.IsTrue(DateTime.TryParse(reserva.PrimerDia, out primerDia));
                    Assert.IsTrue(DateTime.TryParse(reserva.UltimoDia, out ultimoDia));

                    // Verificar que el primer día sea menor o igual a la fecha de búsqueda
                    Assert.IsTrue(primerDia <= DateTime.Parse(datoBusqueda));

                    // Verificar que el último día sea mayor o igual a la fecha de búsqueda
                    Assert.IsTrue(ultimoDia >= DateTime.Parse(datoBusqueda));
                }
            }
        }


        /*
         * Comprueba si no se genera errores al buscar
         * reservas por identificador y este identificador no existe en la 
         * base de datos
         */ 
        [TestMethod]
        public void ObtenerReservas_IdentificadorNoExistente_NoDebeGenerarError()
        {
            // Arrange
            AdministrarReservasHandler handler = new AdministrarReservasHandler();
            string datoBusqueda = "identificador_no_existente";
            string tipoBusqueda = "identificador";

            // Act
            List<ReservacionModelo> reservas = handler.ObtenerReservas(datoBusqueda, tipoBusqueda);

            // Assert
            Assert.IsNotNull(reservas);
            Assert.AreEqual(0, reservas.Count); // Verificar que no se hayan encontrado reservas

     
        }


        /*
         * Comprueba si al tratar de borrar una reserva con un identificador
         * invalido falla o no
         */
        [TestMethod]
        public void EliminarReservacion_IdentificadorInvalido_NoDebeGenerarError()
        {
            // Arrange
            AdministrarReservasHandler handler = new AdministrarReservasHandler();
            string identificadorReserva = "identificador_invalido";
            bool exceptionThrown = false;

            // Act
            try
            {
                handler.EliminarReservacion(identificadorReserva);
            }
            catch (Exception)
            {
                exceptionThrown = true;
            }

            // Assert
            Assert.IsFalse(exceptionThrown);
        }


        /*
         * Se prueba el método ObtenerPersonasReservacion en dos escenarios 
         * diferentes: cuando no hay personas (identificadorReservaSinPersonas)
         * y cuando hay personas (identificadorReservaConPersonas)
         */
        [TestMethod]
        public void ObtenerPersonasReservacion_NoDebeGenerarError()
        {
            // Arrange
            AdministrarReservasHandler handler = new AdministrarReservasHandler();
            string identificadorReservaInexistente = "identificador_sin_personas";
            string identificadorReservaConPersonas = "13t2tdxZ6q";
            bool exceptionThrownSinPersonas = false;
            bool exceptionThrownConPersonas = false;

            // Act - Sin personas
            try
            {
                var resultSinPersonas = handler.ObtenerPersonasReservacion(identificadorReservaInexistente);
            }
            catch (Exception)
            {
                exceptionThrownSinPersonas = true;
            }

            // Act - Con personas
            try
            {
                var resultConPersonas = handler.ObtenerPersonasReservacion(identificadorReservaConPersonas);
            }
            catch (Exception)
            {
                exceptionThrownConPersonas = true;
            }

            // Assert
            Assert.IsFalse(exceptionThrownSinPersonas);
            Assert.IsFalse(exceptionThrownConPersonas);
        }

        /*
         * Se prueba el método  ObtenerPlacasReservacion en dos escenarios 
         * diferentes: cuando no hay placas (identificadorReservaNoExistente)
         * y cuando hay placas ( identificadorReservaExistente)
         */
        [TestMethod]
        public void ObtenerPlacasReservacion_NoDebeGenerarError()
        {
            // Arrange
            AdministrarReservasHandler handler = new AdministrarReservasHandler();
            string identificadorReservaExistente = "6pw5R8YhzY";
            string identificadorReservaNoExistente = "identificador_no_existente";
            bool exceptionThrownExistente = false;
            bool exceptionThrownNoExistente = false;

            // Act - Identificador existente
            try
            {
                var resultExistente = handler.ObtenerPlacasReservacion(identificadorReservaExistente);
            }
            catch (Exception)
            {
                exceptionThrownExistente = true;
            }

            // Act - Identificador no existente
            try
            {
                var resultNoExistente = handler.ObtenerPlacasReservacion(identificadorReservaNoExistente);
            }
            catch (Exception)
            {
                exceptionThrownNoExistente = true;
            }

            // Assert
            Assert.IsFalse(exceptionThrownExistente);
            Assert.IsFalse(exceptionThrownNoExistente);
        }


        /*
         * Se prueba el método ObtenerPlacasReservacion para un identificador de reserva específico (identificadorReserva). 
         * Se asume que solo se espera una placa para ese identificador, 
         *  se verifica que la lista de placas no sea nula, tenga un tamaño de 1 y la placa obtenida sea
         *  igual a la placa esperada (placaEsperada).
         */

        [TestMethod]
        public void ObtenerPlacasReservacion_DebeRetornarPlacaCorrecta()
        {
            // Arrange
            AdministrarReservasHandler handler = new AdministrarReservasHandler();
            string identificadorReserva = "6pw5R8YhzY";
            string placaEsperada = "fsdscsadf ";

            // Act
            var placas = handler.ObtenerPlacasReservacion(identificadorReserva);

            // Assert
            Assert.IsNotNull(placas);
            Assert.AreEqual(1, placas.Count);
            Assert.AreEqual(placaEsperada, placas[0]);
        }





    }
}
