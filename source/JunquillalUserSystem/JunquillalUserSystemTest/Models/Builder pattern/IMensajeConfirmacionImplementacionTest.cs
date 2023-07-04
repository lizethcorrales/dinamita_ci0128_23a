/*
 * Pruebas unitarias creadas por Andrés Matarrita C04668
 */

using JunquillalUserSystem.Models;
using JunquillalUserSystem.Models.Builder_pattern;
using JunquillalUserSystem.Models.Patron_Bridge;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Models.Builder_pattern
{
    [TestClass]
    public class IMensajeConfirmacionImplementacionTest
    {
        /*
         * Prueba si el método AgregarEncabezado retorna el HTML
         * correcto para el encabezado de confirmación de reserva 
         * cuando se proporciona el tipo de actividad
         */
        [TestMethod]
        public void AgregarEncabezado_ReturnsCorrectHtml()
        {
            // Arrange
            var implementacionHTML = new MensajeConfirmacionImplementacionHTML();
            var tipoActividad = "Picnic";
            var expected = "<h2 style='text-align:center;'>Confirmación de Reserva para picnic</h2><br><br>";

            // Act
            var result = implementacionHTML.AgregarEncabezado(tipoActividad);

            // Assert
            Assert.AreEqual(expected, result);
        }


        /*
         *  Prueba si el método AgregarDatosHospedero retorna el HTML es
         *  correcto para los datos del hospedero en el mensaje de 
         *  confirmación de reserva. Se verifica si el resultado 
         *  obtenido es igual al resultado esperado.
         */
        [TestMethod]
        public void AgregarDatosHospedero_ReturnsCorrectHtml()
        {
            // Arrange
            var implementacionHTML = new MensajeConfirmacionImplementacionHTML();
            var hospedero = new HospederoModelo()
            {
                Identificacion = "123456789",
                Nacionalidad = "Costa Rica",
                TipoIdentificacion = "Cédula",
                Motivo = "Vacaciones",
                Nombre = "John",
                Apellido1 = "Doe",
                Apellido2 = "Smith"
            };
            var expected = "<h3>Datos del hospedero: </h3><br>" +
                           "<h6>Identificación: 123456789</h6>" +
                           "<h6>Nacionalidad: Costa Rica</h6>" +
                           "<h6>Tipo de identificación: Cédula</h6>" +
                           "</ul><br>" +
                           "<h6>Motivo de la visita: Vacaciones</h6>" +
                           "<br><h6>John Doe Smith, es un placer darles la bienvenida a Junquillal. " +
                           "Le deseamos un disfrute de su estadia,\n a continuación adjuntamos informacion de " +
                           "su reserva: </h6><br>";

            // Act
            var result = implementacionHTML.AgregarDatosHospedero(hospedero);

            // Assert
            Assert.AreEqual(expected, result);
        }


        /*
         * Crea un mensaje de confirmación en formato HTML utilizando 
         * la clase MensajeConfirmacionImplementacionHTML y
         * verifica si el resultado obtenido coincide con el mensaje HTML esperado.
         */

        [TestMethod]
        public void CrearConfirmacionMensaje_ReturnsCorrectHtml()
        {
            // Arrange
            var implementacionHTML = new MensajeConfirmacionImplementacionHTML();
            var reservacion = new ReservacionModelo();
            var hospedero = new HospederoModelo();
            var desglose = new List<PrecioReservacionDesglose>();

            // Configurar los datos de prueba en los objetos reservacion, hospedero y desglose
            reservacion.TipoActividad = "Picnic";
            hospedero.Identificacion = "123456789";
            hospedero.Nacionalidad = "Costa Rica";
            hospedero.TipoIdentificacion = "Cédula";
            hospedero.Motivo = "Vacaciones";
            hospedero.Nombre = "John";
            hospedero.Apellido1 = "Doe";
            hospedero.Apellido2 = "Smith";

            // Establecer la cantidad de personas en 3
            reservacion.tarifas = new List<TarifaModelo> {
             new TarifaModelo { Cantidad = 2 }, // Niños
             new TarifaModelo { Cantidad = 1 }  // Adulto
            };

            // Agregar los objetos desglose necesarios
            var desglose1 = new PrecioReservacionDesglose
            {
                Poblacion = "Niño menor 6 años",
                Cantidad = 2,
                PrecioAlHacerReserva = 10,
                Nacionalidad = "Costa Rica"
            };

            var desglose2 = new PrecioReservacionDesglose
            {
                Poblacion = "Adulto",
                Cantidad = 1,
                PrecioAlHacerReserva = 20,
                Nacionalidad = "Costa Rica"
            };

            desglose.Add(desglose1);
            desglose.Add(desglose2);

            // Definir el fragmento esperado en el mensaje HTML
            var expectedFragment = "<h2 style='text-align:center;'>Confirmación de Reserva para picnic</h2><br><br>";

            // Act
            var result = implementacionHTML.CrearConfirmacionMensaje(reservacion, hospedero, desglose);

            // Assert
            StringAssert.Contains(result, expectedFragment);
        }

        /* 
         * Prueba que verifica si la implementación de MensajeConfirmacionImplementacionHTML
         * cumple con la interfaz IMensajeConfirmacionImplementacion
         */

        [TestMethod]
        public void MensajeConfirmacionImplementacionHTML_ImplementsIMensajeConfirmacionImplementacion()
        {
            // Arrange
            var implementacionHTML = new MensajeConfirmacionImplementacionHTML();

            // Act
            var implementsInterface = implementacionHTML is IMensajeConfirmacionImplementacion;

            // Assert
            Assert.IsTrue(implementsInterface);
        }


        /* 
         * Prueba que verifica si la clase abstracta MensajeConfirmacionAbstraccion
         * contiene  el método abstracto CrearConfirmacionMensaje 
         */
        [TestMethod]
        public void MensajeConfirmacionAbstraccion_ContainsCrearConfirmacionMensajeMethod()
        {
            // Arrange
            var abstraccionType = typeof(MensajeConfirmacionAbstraccion);
            var crearConfirmacionMensajeMethod = abstraccionType.GetMethod("CrearConfirmacionMensaje");

            // Act & Assert
            Assert.IsNotNull(crearConfirmacionMensajeMethod);
        }
    }
}


