/*
 * Pruebas unitarias creadas por Andrés Matarrita C04668
 */

using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Models
{
    [TestClass]
    public class ReservacionModeloTest
    {

       /*
        * Verifica que el método LlenarInformacionReserva agrega 
        * correctamente las placas de vehículos al modelo de reserva.
        */

       [TestMethod]
        public void LlenarInformacionReserva_DebeAgregarPlacasVehiculos()
        {
            // Arrange
            var form = new Dictionary<string, StringValues>()
            {
                { "placa1", new StringValues("ABC123") },
                { "placa2", new StringValues("DEF456") }
             };

            var mockRequest = new Mock<HttpRequest>();
            mockRequest.Setup(r => r.Form).Returns(new FormCollection(form));

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(c => c.Request).Returns(mockRequest.Object);

            var controllerContext = new ControllerContext()
            {
                HttpContext = mockHttpContext.Object
            };

            var reservacion = new ReservacionModelo();

            // Act
            var result = reservacion.LlenarPlacasResarva(reservacion, controllerContext.HttpContext.Request.Form);

            // Assert
            Assert.AreEqual("ABC123", result.placasVehiculos[0]);
            Assert.AreEqual("DEF456", result.placasVehiculos[1]);
          
        }


        /*
         * Se prueba el motedo  LlenarFechas para verificar
         * si almacena correctamente las fechas de entredad y salida
         * obtenidas del IFormCollection
         */

        [TestMethod]
        public void LlenarFechas_DebeAsignarFechasCorrectas()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "fecha-entrada", new StringValues("2022-06-01") },
               { "fecha-salida", new StringValues("2022-06-05") }
             });

            var reservacion = new ReservacionModelo();

            // Act
            var result = reservacion.LlenarFechas(reservacion, form);

            // Assert
            Assert.AreEqual("2022-06-01", result.PrimerDia);
            Assert.AreEqual("2022-06-05", result.UltimoDia);
        }


        /* 
         * En el método LlenarCantidadPersonas se prueba que las cantidades de
         * personas obtenidas de un formulario se asignen correctamente
         * a un objeto ReservacionModelo en la lista cantTipoPersona.
         */

        [TestMethod]
        public void LlenarCantidadPersonas_DebeAsignarCantidadPersonasCorrectas()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
       {
        { "cantidad_Adultos_Nacional", new StringValues("2") },
        { "cantidad_Ninnos_Nacional_mayor6", new StringValues("3") },
        { "cantidad_Ninnos_Nacional_menor6", new StringValues("1") },
        { "cantidad_adulto_mayor", new StringValues("1") },
        { "cantidad_Adultos_Extranjero", new StringValues("5") },
        { "cantidad_ninnos_extranjero", new StringValues("2") },
        { "cantidad_adultoMayor_extranjero", new StringValues("0") }
         });

            var reservacion = new ReservacionModelo();
            reservacion.tarifas = new List<TarifaModelo>
        {
        new TarifaModelo(),
        new TarifaModelo(),
        new TarifaModelo(),
        new TarifaModelo(),
        new TarifaModelo(),
        new TarifaModelo(),
        new TarifaModelo()
        };

            // Act
            var result = reservacion.LlenarCantidadPersonas(reservacion, form);

            // Assert
            Assert.AreEqual(2, result.tarifas[0].Cantidad);
            Assert.AreEqual(3, result.tarifas[1].Cantidad);
            Assert.AreEqual(1, result.tarifas[2].Cantidad);
            Assert.AreEqual(1, result.tarifas[3].Cantidad);
            Assert.AreEqual(5, result.tarifas[4].Cantidad);
            Assert.AreEqual(2, result.tarifas[5].Cantidad);
            Assert.AreEqual(0, result.tarifas[6].Cantidad);
        }



        /*
         * Este método de prueba verifica que las propiedades
         * de los objetos creados a través de los dos constructores se 
         * inicialicen correctamente y tengan los valores esperados.
         */

        [TestMethod]
         public void Constructores_DebeInicializarPropiedadesCorrectamente()
            {
              // Arrange
              string tipo = "Tipo de reserva";

              // Act
              ReservacionModelo reserva1 = new ReservacionModelo();
              ReservacionModelo reserva2 = new ReservacionModelo(tipo);

              // Assert
              Assert.IsNotNull(reserva1.tipoPersona);
              Assert.IsNotNull(reserva1.cantTipoPersona);
              Assert.IsNotNull(reserva1.placasVehiculos);
              Assert.IsNotNull(reserva1.hospedero);

              Assert.AreEqual(0, reserva1.cantTipoPersona.Count);
              Assert.AreEqual(0, reserva1.placasVehiculos.Count);

             Assert.IsNotNull(reserva2.tipoPersona);
             Assert.IsNotNull(reserva2.cantTipoPersona);
             Assert.IsNotNull(reserva2.placasVehiculos);
             Assert.IsNotNull(reserva2.hospedero);

             Assert.AreEqual(tipo, reserva2.TipoActividad);
             Assert.AreEqual(0, reserva2.cantTipoPersona.Count);
             Assert.AreEqual(0, reserva2.placasVehiculos.Count);
            }
        }
    }

