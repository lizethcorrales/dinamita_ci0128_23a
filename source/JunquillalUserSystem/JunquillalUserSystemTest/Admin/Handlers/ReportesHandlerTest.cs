/*
 * Archivo creado por Lizeth Corrales C02428
 */

using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Admin.Handlers
{
    [TestClass]
    public class ReportesHandlerTest
    {
        [TestMethod]    
        public void ReportesHandlerMetodoObtenerReporteSinActividadValida()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            string fechaInicialDePrueba = "2023-06-02";
            string fechaFinalDePrueba = "2023-06-03";
            string tipoReporte = "visitas";
            List<PrecioReservacionDesglose> reporteEsperado = new();
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "fecha-salida", new StringValues(fechaFinalDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act
            //List<PrecioReservacionDesglose> reporteObtenido = reportesHandler.obtenerReporte(formEjemplo, "");

            //Assert
            //Assert.IsNotNull(reporteObtenido);
            //CollectionAssert.AreEqual(reporteEsperado, reporteObtenido);
        }

        [TestMethod]
        public void ReportesHandlerMetodoObtenerReporteCuyoFormTieneElementosInesperados()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            string fechaInicialDePrueba = "2023-06-02";
            string actividad = "Picnic";
            List<PrecioReservacionDesglose> reporteEsperado = new();
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha", new StringValues(fechaInicialDePrueba) }
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act
            //List<PrecioReservacionDesglose> reporteObtenido = reportesHandler.obtenerReporte(formEjemplo, actividad);

            //Assert
            //Assert.IsNotNull(reporteObtenido);
            //CollectionAssert.AreEqual(reporteEsperado, reporteObtenido);
        }

        [TestMethod]
        public void ReportesHandlerMetodoObtenerReporteDeUnDia()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            string fechaInicialDePrueba = "2023-06-02";
            string tipoReporte = "diario";
            string actividad = "Picnic";
            List<PrecioReservacionDesglose> reporteEsperado = llenarListaDeReporteDePruebaUnDia();
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act 
           // List<PrecioReservacionDesglose> reporteObtenido = reportesHandler.obtenerReporte(formEjemplo, actividad);


            //Arrange
            //Assert.IsNotNull(reporteObtenido);
            //for (int indice = 0; indice < reporteEsperado.Count; ++indice)
            //{
            //    Assert.AreEqual(reporteEsperado[indice].Nacionalidad, reporteEsperado[indice].Nacionalidad);
            //    Assert.AreEqual(reporteEsperado[indice].Poblacion, reporteEsperado[indice].Poblacion);
            //    Assert.AreEqual(reporteEsperado[indice].Actividad, reporteEsperado[indice].Actividad);
            //    Assert.AreEqual(reporteEsperado[indice].Cantidad, reporteEsperado[indice].Cantidad);
            //    Assert.AreEqual(reporteEsperado[indice].PrecioAlHacerReserva, reporteEsperado[indice].PrecioAlHacerReserva);
            //}
        }

        public List<PrecioReservacionDesglose> llenarListaDeReporteDePruebaUnDia()
        {
            List<PrecioReservacionDesglose> reporte = new List<PrecioReservacionDesglose>
            {
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Extranjero", Poblacion = "Adulto",
                    Actividad = "Picnic", Cantidad = 1, PrecioAlHacerReserva = 13.56
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Nacional", Poblacion = "Adulto",
                    Actividad = "Picnic", Cantidad = 3, PrecioAlHacerReserva = 6780
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Extranjero", Poblacion = "Adulto Mayor",
                    Actividad = "Picnic", Cantidad = 1, PrecioAlHacerReserva = 0
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Nacional", Poblacion = "Adulto Mayor",
                    Actividad = "Picnic", Cantidad = 1, PrecioAlHacerReserva = 0
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Extranjero", Poblacion = "Niño",
                    Actividad = "Picnic", Cantidad = 1, PrecioAlHacerReserva = 5.65
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Nacional", Poblacion = "Niño",
                    Actividad = "Picnic", Cantidad = 3, PrecioAlHacerReserva = 3390
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Nacional", Poblacion = "Niño menor 6 años",
                    Actividad = "Picnic", Cantidad = 2, PrecioAlHacerReserva = 0
                } 
            };
            return reporte;
        }

        [TestMethod]
        public void ReportesHandlerMetodoObtenerReporteRangoDeDias()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            string fechaInicialDePrueba = "2023-07-02";
            string fechaFinalDePrueba = "2023-07-04";
            string tipoReporte = "visitas";
            string actividad = "Camping";
            List<PrecioReservacionDesglose> reporteEsperado = llenarListaDeReporteDePruebaRangoDias();
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "fecha-salida", new StringValues(fechaFinalDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act 
            //List<PrecioReservacionDesglose> reporteObtenido = reportesHandler.obtenerReporte(formEjemplo, actividad);

            
            //Arrange
        //    Assert.IsNotNull(reporteObtenido);
        //    for (int indice = 0; indice < reporteEsperado.Count; ++indice)
        //    {
        //        Assert.AreEqual(reporteEsperado[indice].Nacionalidad, reporteObtenido[indice].Nacionalidad);
        //        Assert.AreEqual(reporteEsperado[indice].Poblacion, reporteObtenido[indice].Poblacion);
        //        Assert.AreEqual(reporteEsperado[indice].Actividad, reporteObtenido[indice].Actividad);
        //        Assert.AreEqual(reporteEsperado[indice].Cantidad, reporteObtenido[indice].Cantidad);
        //        Assert.AreEqual(reporteEsperado[indice].PrecioAlHacerReserva, reporteObtenido[indice].PrecioAlHacerReserva);
        //    }
        }

        public List<PrecioReservacionDesglose> llenarListaDeReporteDePruebaRangoDias()
        {
            List<PrecioReservacionDesglose> reporte = new List<PrecioReservacionDesglose>
            {
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Nacional", Poblacion = "Adulto",
                    Actividad = "Camping", Cantidad = 2, PrecioAlHacerReserva = 9040
                },
                new PrecioReservacionDesglose
                {
                    Nacionalidad = "Nacional", Poblacion = "Niño",
                    Actividad = "Camping", Cantidad = 1, PrecioAlHacerReserva = 3390
                }
            };
            return reporte;
        }

        [TestMethod]
        public void AgregarDatoConPrecioReservacionDesgloseNulo()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            string separador = "\t";

            //Act & Assert
            try
            {
                //reportesHandler.agregarDato(null, separador);
            } catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void AgregarDatoConSeparadorNulo()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            PrecioReservacionDesglose desgloseIndividual = new();

            //Act & Assert
            try
            {
                //reportesHandler.agregarDato(desgloseIndividual, null);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [TestMethod]
        public void EscribirCSVConPrecioReservacionDesgloseNulo()
        {
            //Arrange
            ReportesHandler reportesHandler = new();
            List<PrecioReservacionDesglose> reporteEsperado = llenarListaDeReporteDePruebaRangoDias();
            string fechaInicialDePrueba = "2023-05-09";
            string fechaFinalDePrueba = "2023-05-12";
            string tipoReporte = "visitas";
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaInicialDePrueba) },
                { "fecha-salida", new StringValues(fechaFinalDePrueba) },
                { "reportes", new StringValues(tipoReporte) },
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act & Assert
            try
            {
               // reportesHandler.escribirCSV(null, formEjemplo);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }

}
