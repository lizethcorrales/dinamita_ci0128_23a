using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using JunquillalAutomatedTesting.PageObjectModels;

namespace JunquillalAutomatedTesting.Tests
{
    [TestFixture]
    public class CamposDisponiblesTests
    {
        IWebDriver driver = null;
        FormCamposDisponiblesPage paginaCamposDisponibles;
        public CamposDisponiblesTests()
        {
            driver = new ChromeDriver();
            paginaCamposDisponibles = new(driver);
        }

        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/CamposDisponibles/CamposDisponibles");
            driver.Manage().Window.Maximize();
        }
        public void tearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void CamposDisponibles_BuscarCamposSinFechaEntrada()
        {
            // Arrange
            Setup();
            // Act
            paginaCamposDisponibles.PresionarBotonBuscar();
            IWebElement mensajeErrorCampos = paginaCamposDisponibles.obtenerMensajeDeError();
            // Assert
            Assert.AreEqual("Debe de ingresar una fecha válida", mensajeErrorCampos.Text);

        }
        [Test, Order(2)]
        public void CamposDisponibles_BusquedaDeFormaCorrecta()
        {
            // Arrange
            Setup();
            // Act
            paginaCamposDisponibles.elegirNumeroDeDia("28");
            paginaCamposDisponibles.PresionarBotonBuscar();
            IWebElement mensajeErrorCampos = paginaCamposDisponibles.obtenerMensajeDeError();
            // Assert
            Assert.IsEmpty(mensajeErrorCampos.Text);
            tearDown();
        }



    }
}
