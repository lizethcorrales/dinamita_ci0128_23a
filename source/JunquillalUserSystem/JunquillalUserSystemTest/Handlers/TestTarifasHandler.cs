/*
 * Archivo creado por Lizeth Corrales C02428
 */

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using static JunquillalUserSystemTest.CalculadorFechas;
using System.Diagnostics;
using System.Security.Principal;
using System.Data.SqlClient;

namespace JunquillalUserSystemTest.Handlers
{
    [TestClass]
    public class TestTarifasHandler
    {
        [TestMethod]
        public void TestTarifasHandlerListaTarifasNoNula()
        {
            // Arrange
            TarifasHandler handler = new();

            // Act
            var resultado = handler.obtenerTarifasActuales();

            // Assert
            Assert.IsNotNull(resultado);
        }

        [TestMethod]
        public void TestTarifasHandlerComprobarQueExistenDatosEnTabla()
        {
            // Arrange
            TarifasHandler handler = new();

            // Act
            var resultado = handler.obtenerTarifasActuales();

            // Assert
            Assert.IsTrue(resultado.Count > 0);
        }

        [TestMethod]
        public void TestTarifasHandlerActualizarPrecioTarifasSoporteDeValoresNull()
        {
            // Arrange
            TarifasHandler handler = new();


            // Act && Assert
            try
            {
                handler.actualizarPrecioTarifas(null);
            }
            catch (Exception ex)
            {
                throw;
            }      
        }

        [TestMethod]
        public void TestTarifasHandlerActualizarPrecioTarifasCamposSinDatos()
        {
            // Arrange
            TarifasHandler handler = new();

            // Act && Assert
            try
            {
                handler.actualizarPrecioTarifas(new TarifaModelo());
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}