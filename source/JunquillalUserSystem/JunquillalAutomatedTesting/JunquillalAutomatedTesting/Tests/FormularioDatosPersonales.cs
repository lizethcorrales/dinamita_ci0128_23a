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
        public void ComprobarValidacionCampos()
        {
            Setup();
            List<IWebElement> mensajesDeError = formDatosPersonales.ObtenerMensajesDeError();
            Assert.True(mensajesDeError.Count == 3);
            Assert.AreEqual("Nombre NO válido", mensajesDeError[0].Text);
            Assert.AreEqual("Apellido NO válido", mensajesDeError[1].Text);
            Assert.AreEqual("Identificación NO válido", mensajesDeError[2].Text);
        }

        [Test, Order(2)]
        public void ComprobarQueExisteProvinciaConIdentificacionNacional()
        {
            Setup();
            var dropDownProvincia = formDatosPersonales.SeleccionarTipoIDNacional();
            Assert.True(dropDownProvincia.Count == 1);
        }

        [Test, Order(3)]
        public void ComprobarQueDesapareceProvinciaConIdentificacionExtranjera()
        {
            Setup();
            IWebElement dropDownProvincia = formDatosPersonales.SeleccionarTipoIDExtranjero();
            bool visibility = dropDownProvincia.Displayed;
            Assert.IsFalse(visibility);
            TearDown();
        }
    }
}