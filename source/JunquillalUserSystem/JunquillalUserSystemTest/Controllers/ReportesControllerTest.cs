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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Controllers
{
    [TestClass]
    public class ReportesControllerTest
    {
        [TestMethod]
        public void Download_DebeDevolverLaVistaReportes_SiElNombreDelArchivoEstaVacio()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);

            //Act
            var resultado = controlador.Download(String.Empty) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Reportes", resultado.ActionName);
        }

        [TestMethod]
        public void Download_DebeDevolverLaVistaReportes_CuandoLaCarpetaNoExiste()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);
            string nombreArchivo = "reporte.xls";
            //Act
            var resultado = controlador.Download(nombreArchivo) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual("Reportes", resultado.ActionName);
        }

        [TestMethod]
        public void GetFiles_DebeDevolverListaVacia_CuandoLaCarpetaNoExiste()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);

            //Act
            var resultado = controlador.GetFiles();

            // Assert
            Assert.IsNotNull(resultado);
            Assert.IsTrue(resultado.Count == 0);
        }

        [TestMethod]
        public void esReporteLiquidacion_DebeDevolverVerdadero_SiElTipoDeReporteEsLiquidacion()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);

            //Act
            List<StringValues> valoresForm = new();
            valoresForm.Add("2023-07-01");
            valoresForm.Add(String.Empty);
            valoresForm.Add("visitas");
            valoresForm.Add("liquidacion");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            var resultado = controlador.esReporteLiquidacion(formEjemplo);

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void esReporteLiquidacion_DebeDevolverFalso_SiElTipoDeReporteEsLiquidacion()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);

            //Act
            List<StringValues> valoresForm = new();
            valoresForm.Add("2023-07-01");
            valoresForm.Add(String.Empty);
            valoresForm.Add("visitas");
            valoresForm.Add("ventas");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            var resultado = controlador.esReporteLiquidacion(formEjemplo);

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void CamposFaltantes_DebeDevolverVerdadero_CuandoFechaInicialEstaVacia()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);
            string nombreArchivo = "reporte.xls";
            List<StringValues> valoresForm = new();
            valoresForm.Add(String.Empty);
            valoresForm.Add("2023-07-01");
            valoresForm.Add("visitas");
            valoresForm.Add("liquidacion");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            //Act
            var resultado = controlador.CamposFaltantes(formEjemplo);

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CamposFaltantes_DebeDevolverVerdadero_CuandoFechaFinalEstaVacia()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);
            string nombreArchivo = "reporte.xls";
            List<StringValues> valoresForm = new();
            valoresForm.Add("2023-07-01");
            valoresForm.Add(String.Empty);
            valoresForm.Add("visitas");
            valoresForm.Add("liquidacion");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            //Act
            var resultado = controlador.CamposFaltantes(formEjemplo);

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CamposFaltantes_DebeDevolverVerdadero_CuandoFechaFinalEstaVaciaPeroReportesEsVisitas()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);
            string nombreArchivo = "reporte.xls";
            List<StringValues> valoresForm = new();
            valoresForm.Add("2023-07-01");
            valoresForm.Add(String.Empty);
            valoresForm.Add("visitas");
            valoresForm.Add("liquidacion");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            //Act
            var resultado = controlador.CamposFaltantes(formEjemplo);

            // Assert
            Assert.IsTrue(resultado);
        }

        [TestMethod]
        public void CamposFaltantes_DebeDevolverFalso_CuandoFechaFinalEstaVaciaPeroReportesEsDiario()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);
            string nombreArchivo = "reporte.xls";
            List<StringValues> valoresForm = new();
            valoresForm.Add("2023-07-01");
            valoresForm.Add(String.Empty);
            valoresForm.Add("diario");
            valoresForm.Add("liquidacion");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            //Act
            var resultado = controlador.CamposFaltantes(formEjemplo);

            // Assert
            Assert.IsFalse(resultado);
        }

        [TestMethod]
        public void CamposFaltantes_DebeDevolverFalso_CuandoTodoslosCamposEstanLlenos()
        {
            // Arrange
            ReportesHandler reportesHandler = new ReportesHandler();
            ReportesController controlador = new ReportesController(reportesHandler);
            string nombreArchivo = "reporte.xls";
            List<StringValues> valoresForm = new();
            valoresForm.Add("2023-07-01");
            valoresForm.Add("2023-07-10");
            valoresForm.Add("diario");
            valoresForm.Add("liquidacion");
            var formEjemplo = new FormCollection(InicializarValoresForm(valoresForm));
            //Act
            var resultado = controlador.CamposFaltantes(formEjemplo);

            // Assert
            Assert.IsFalse(resultado);
        }

        public Dictionary<string, StringValues> InicializarValoresForm(List<StringValues> valoresForm)
        {
            return new Dictionary<string, StringValues>
            {
                { "fecha-entrada", valoresForm[0] },
                { "fecha-salida", valoresForm[1] },
                { "reportes", valoresForm[2] },
                { "tipoReporte", valoresForm[3] }
            };
        }

    }
}
