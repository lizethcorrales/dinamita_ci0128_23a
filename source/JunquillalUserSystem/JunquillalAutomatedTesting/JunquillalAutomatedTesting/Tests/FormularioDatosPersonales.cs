/*
 * Archivo creado por Lizeth Corrales C02428
 */
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;
using JunquillalAutomatedTesting.PageObjectModels;

namespace JunquillalAutomatedTesting.Tests
{
    [TestFixture]
    public class TestsFormularioDatosPersonales
    {
        IWebDriver driver = null;
        FormDatosPersonales formDatosPersonales = null;

        public TestsFormularioDatosPersonales()
        {
            driver = new ChromeDriver();                  
        }

        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Reservacion/FormularioCantidadPersonas");
            formDatosPersonales = new(driver);      
            driver.Manage().Window.Maximize();
        }

        public void TearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void ComprobarQueSeGeneranMensajesDeErrorAlEnviarFomularioVacio()
        {
            //Arrange
            Setup();
            //Act
            formDatosPersonales.darleClickAlBotonContinuarConFormularioEnBlanco();
            List<IWebElement> mensajesDeError = formDatosPersonales.ObtenerMensajesDeError();
            //Assert
            Assert.True(mensajesDeError.Count == 3);
        }

        [Test, Order(2)]
        public void ComprobarQueSeGeneraMensajeDeErrorEnCampoNombreCuandoEsteSeEnviaVacio()
        {
            //Arrange
            Setup();
            //Act
            formDatosPersonales.CompletarPaginaDatosPersonalesConDatosDePruebaPeroSinNombre();
            List<IWebElement> mensajesDeError = formDatosPersonales.ObtenerMensajesDeError();
            //Assert
            Assert.AreEqual("Nombre NO válido", mensajesDeError[0].Text);
        }

        [Test, Order(3)]
        public void ComprobarQueSeGeneraMensajeDeErrorEnCampoPrimerApellidoCuandoEsteSeEnviaVacio()
        {
            //Arrange
            Setup();
            //Act
            formDatosPersonales.CompletarPaginaDatosPersonalesConDatosDePruebaPeroSinPrimerApellido();
            List<IWebElement> mensajesDeError = formDatosPersonales.ObtenerMensajesDeError();
            //Assert
            Assert.AreEqual("Apellido NO válido", mensajesDeError[0].Text);
        }

        [Test, Order(4)]
        public void ComprobarQueSeGeneraMensajeDeErrorEnCampoIDCuandoEsteSeEnviaVacio()
        {
            //Arrange
            Setup();
            //Act
            formDatosPersonales.CompletarPaginaDatosPersonalesConDatosDePruebaPeroSinID();
            List<IWebElement> mensajesDeError = formDatosPersonales.ObtenerMensajesDeError();
            //Assert
            Assert.AreEqual("Identificación NO válido", mensajesDeError[0].Text);
        }

        [Test, Order(5)]
        public void ComprobarQueExisteProvinciaConIdentificacionNacional()
        {
            //Arrange
            Setup();
            //Act
            var dropDownProvincia = formDatosPersonales.SeleccionarTipoIDNacional();
            //Assert
            Assert.True(dropDownProvincia.Count == 1);
        }

        [Test, Order(6)]
        public void ComprobarQueDesapareceProvinciaConIdentificacionExtranjera()
        {
            //Arrange
            Setup();
            //Act
            IWebElement dropDownProvincia = formDatosPersonales.SeleccionarTipoIDExtranjero();
            bool visibility = dropDownProvincia.Displayed;
            //Assert
            Assert.IsFalse(visibility);
            TearDown();
        }
    }
}