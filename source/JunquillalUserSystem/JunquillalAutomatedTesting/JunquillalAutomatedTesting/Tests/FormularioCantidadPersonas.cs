/*
 *Sabrina Brenes Hernandez C01309
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
    public class TestsFormularioCantidadPersonas
    {
        IWebDriver driver = null;
        FormCantidadPersonas paginaFormularioCantidadPersonas = null;

        public TestsFormularioCantidadPersonas()
        {
            driver = new ChromeDriver();
        }

        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Reservacion/FormularioCantidadPersonas");
            paginaFormularioCantidadPersonas = new(driver);
            driver.Manage().Window.Maximize();
        }

        public void TearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void ComprobarMensajeDeErrorCuandoSeAgreganMasPersonasQueLasDelLimite()
        {
            //Arrange
            Setup();
            //Act
            paginaFormularioCantidadPersonas.CompletarPaginaConDatosDePruebaDondeLaCantidadTotalExcedeLaMaxima();
            IWebElement mensajesDeError = paginaFormularioCantidadPersonas.ObtenerMensajeDeError();
            //Assert
            Assert.AreEqual(mensajesDeError.Text, "Revise que la cantidad de personas no exceda las 70 personas");
        }

        [Test, Order(2)]
        public void ComprobarMensajeDeErrorCuandoNoSeAgreganPersonas()
        {
            //Arrange
            Setup();
            //Act
            paginaFormularioCantidadPersonas.CompletarPaginaConDatosDePruebaDondeTodosLosCamposSon0();
            IWebElement mensajesDeError = paginaFormularioCantidadPersonas.ObtenerMensajeDeError();
            //Assert
            Assert.AreEqual(mensajesDeError.Text, "Revise que haya al menos una persona");
        }

        [Test, Order(3)]
        public void ComprobarMensajeDeErrorCuandoSoloSeAgreganNinnos()
        {
            //Arrange
            Setup();
            //Act
            paginaFormularioCantidadPersonas.CompletarPaginaConDatosDePruebaDondeSoloHayNinnos();
            IWebElement mensajesDeError = paginaFormularioCantidadPersonas.ObtenerMensajeDeError();
            //Assert
            Assert.AreEqual(mensajesDeError.Text, "Debe ir al menos un adulto");
            TearDown();
        }
    }
}