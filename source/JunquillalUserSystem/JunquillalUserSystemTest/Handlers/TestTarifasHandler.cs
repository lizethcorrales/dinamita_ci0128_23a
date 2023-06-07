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
        public void TestTarifasHandlerComprobarResultadosDeTablaDevuelta()
        {
            // Arrange
            TarifasHandler handler = new();
            int cantidadTarifas = 14;
            List<TarifaModelo> tarifas = llenarListaDePrueba(cantidadTarifas);
            int indiceMitadTabla = cantidadTarifas / 2 - 1;           

            // Act
            var resultado = handler.obtenerTarifasActuales();

            // Assert
            for (int indice = 0; indice < cantidadTarifas; ++indice)
            {
                Assert.AreEqual(tarifas[indice].Nacionalidad, resultado[indice].Nacionalidad);
                Assert.AreEqual(tarifas[indice].Poblacion, resultado[indice].Poblacion);
                Assert.AreEqual(tarifas[indice].Actividad, resultado[indice].Actividad);
                Assert.AreEqual(tarifas[indice].Precio, resultado[indice].Precio);
            }
        }

        public List<TarifaModelo> llenarListaDePrueba(int cantidadTarifas) {
            List<TarifaModelo> listaADevolver = new();
            List<string> TipoNacionalidad = new List<string> { "Extranjero", "Nacional"};
            List<string> TipoPoblacion = new List<string> { "Adulto", "Adulto Mayor", "Niño", "Niño menor 6 años" };
            List<string> TipoActividad = new List<string> { "Camping", "Picnic" };
            List<double> Precios = new List<double> { 18.08, 13.56, 18.08, 13.56, 10.17, 5.65, 4520, 2260, 2260, 0, 3390, 1130, 0, 0 };
            for (int indice = 0; indice < cantidadTarifas; ++indice) {
                listaADevolver.Add(
                    new TarifaModelo
                    {
                        Nacionalidad = indice < 6 ? TipoNacionalidad[0] : TipoNacionalidad[1],                       
                        Poblacion = indice < 6 ? TipoPoblacion[indice/2] : TipoPoblacion[(indice-6)/2],
                        Actividad = indice % 2 == 0 ? TipoActividad[0] : TipoActividad[1],
                        Precio = Precios[indice]
                    });
            }
            return listaADevolver;
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