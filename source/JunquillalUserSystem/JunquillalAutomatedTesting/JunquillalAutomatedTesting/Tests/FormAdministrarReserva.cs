// Creado por Andres Matarrita Miranda

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;
using NUnit.Framework;
using JunquillalAutomatedTesting.PageObjectModels;


namespace JunquillalAutomatedTesting.Tests

{

    [TestFixture]
    public class FormaIncorrectoBuscarReservaPorFechaTest
    {
        private IWebDriver driver = null;
        private FormAdministrarReserva paginaAdministrarReserva;

        public FormaIncorrectoBuscarReservaPorFechaTest()
        {
            driver = new ChromeDriver();
        }

        public void SetUp()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Admin");
            driver.Manage().Window.Maximize();
            paginaAdministrarReserva = new(driver);
        }

        public void TearDown()
        {
            driver.Quit();
        }

        [Test]
        public void formaIncorrectoBuscarReservaPorFecha()
        {
            // Arrange
            SetUp();
            // Act
            paginaAdministrarReserva.BuscarReservacionPorFechaQueNoTengaFormatoYYYYMMDD("prueba");
            IWebElement mensajeErrorIdentificacion = paginaAdministrarReserva.ObtenerMensajeDeError();
            // Assert
            Assert.That(mensajeErrorIdentificacion.Text, Is.EqualTo("Formato de fecha inválido. Utiliza el formato yyyy-mm-dd"));
            TearDown();
        }
    }

}
