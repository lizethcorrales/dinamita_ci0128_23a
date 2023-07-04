/*
 * Pruebas Unitarias Lizeth Corrales. C02428
 */

using JunquillalUserSystem.Areas.Admin.Controllers;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Controllers
{
    [TestClass]
    public class GeneradorDeReportesTest
    {
        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueEscribirXLSDevuelveUnStringNoVacio()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            string fechaInicialDePrueba = "2023-06-02";
            string fechaFinalDePrueba = "2023-06-03";
            string tipoReporte = "visitas";
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "fecha-salida", new StringValues(fechaFinalDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            List<ReportesModel> reporte = new List<ReportesModel>
            {
                new ReportesModel
                {
                    Nacionalidad = "Extranjero", Poblacion = "Adulto",
                    Actividad = "Picnic", Cantidad = 1, VentasTotales = 13.56
                }
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act
            string resultado = generador.escribirXLS(reporte, formEjemplo, false);

            // Assert
            Assert.AreNotEqual(resultado, String.Empty);
        }

        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueDevuelveNombreDeArchivoEsperadoSiEsLiquidacion()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            string fechaInicialDePrueba = "2023-07-04";
            string fechaFinalDePrueba = "2023-07-06";
            string tipoReporte = "visitas";
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "fecha-salida", new StringValues(fechaFinalDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            List<ReportesModel> reporte = new List<ReportesModel>
            {
                new ReportesModel
                {
                    Nacionalidad = "Extranjero", Poblacion = "Niño",
                    Actividad = "Camping", Cantidad = 3, VentasTotales = 10.17
                }
            };
            var formEjemplo = new FormCollection(formEjemploDatos);
            string resultadoEsperado = "LiquidacionReporte_del_" + fechaInicialDePrueba + "_a_" + fechaFinalDePrueba + ".xlsx";

            //Act
            string resultado = generador.escribirXLS(reporte, formEjemplo, true);

            // Assert
            Assert.AreEqual(resultadoEsperado, resultado);
        }

        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueDevuelveNombreDeArchivoEsperadoSiEsVisitacion()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            string fechaInicialDePrueba = "2023-06-20";
            string fechaFinalDePrueba = "2023-06-28";
            string tipoReporte = "visitas";
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "fecha-salida", new StringValues(fechaFinalDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            List<ReportesModel> reporte = new List<ReportesModel>
            {
                new ReportesModel
                {
                    Nacionalidad = "Extranjero", Poblacion = "Niño",
                    Actividad = "Picnic", Cantidad = 5, VentasTotales = 5.65
                }
            };
            var formEjemplo = new FormCollection(formEjemploDatos);
            string resultadoEsperado = "VisitacionReporte_del_" + fechaInicialDePrueba + "_a_" + fechaFinalDePrueba + ".xlsx";

            //Act
            string resultado = generador.escribirXLS(reporte, formEjemplo, false);

            // Assert
            Assert.AreEqual(resultadoEsperado, resultado);
        }

        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueDevuelveElContenidoDeColumnasCorrectoSiEsReporteVisitacion()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            List<ReportesModel> reporte = new();
            string[] nombreColumnasEsperado = new string[] { "FECHA", "TIPO DE VISITANTE", "LUGAR DE PROCEDENCIA", "ESTATUS", "TIPO DE VISITA", "CANTIDAD", "MOTIVO" };
            
            //Act
            var resultado = generador.agregarContenidoAExcelReporteVisitacion(reporte, String.Empty);

            // Assert
            for (int indice = 0; indice < nombreColumnasEsperado.Length; ++indice)
            {
                Assert.AreEqual(nombreColumnasEsperado[indice], resultado[1, indice]);
            }
        }

        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueDevuelveLaCantidadDeColumnasYFilasEsperadoSiEsVisitacion()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            List<ReportesModel> reporte = new List<ReportesModel>
            {
                new ReportesModel
                {
                    PrimerDia = "2023-07-05", Nacionalidad = "Extranjero", Poblacion = "Adulto",
                    NombrePais = "Alemania", Actividad = "Picnic", Cantidad = 1, Motivo = "Ocio"
                }
            };
            int cantidadDeFilasYColumnasEsperado = 21; // (filaheader + filaNombreColumnas + cantidadReportes) * 7

            //Act
            var resultado = generador.agregarContenidoAExcelReporteVisitacion(reporte, String.Empty);

            // Assert         
            Assert.IsTrue(resultado.Length == cantidadDeFilasYColumnasEsperado);
        }

        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueDevuelveElContenidoDeColumnasCorrectoSiEsReporteLiquidacion()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            List<ReportesModel> reporte = new();
            string[] nombreColumnasEsperado = new string[] { "FECHA", "TIPO DE VISITANTE", "ESTATUS", "TIPO DE VISITA", "CANTIDAD", "VENTAS TOTALES" };
            //Act
            var resultado = generador.agregarContenidoAExcelReporteLiquidacion(reporte, String.Empty);

            // Assert
            for (int indice = 0; indice < nombreColumnasEsperado.Length; ++indice)
            {
                Assert.AreEqual(nombreColumnasEsperado[indice], resultado[1, indice]);
            }
        }

        [TestMethod]
        public void GeneradorDeReportes_ComprobarQueDevuelveLaCantidadDeColumnasYFilasEsperadoSiLiquidacion()
        {
            // Arrange
            GeneradorDeReportes generador = new();
            List<ReportesModel> reporte = new List<ReportesModel>
            {
                new ReportesModel
                {
                    PrimerDia = "2023-07-05", Nacionalidad = "Extranjero", Poblacion = "Adulto",
                    Actividad = "Picnic", Cantidad = 1, VentasTotales = 13.56
                }
            };
            int cantidadDeFilasYColumnasEsperado = 18; // (filaheader + filaNombreColumnas + cantidadReportes) * 6
            //Act
            var resultado = generador.agregarContenidoAExcelReporteLiquidacion(reporte, String.Empty);

            // Assert          
            Assert.IsTrue(resultado.Length == cantidadDeFilasYColumnasEsperado);
        }
    }
}
