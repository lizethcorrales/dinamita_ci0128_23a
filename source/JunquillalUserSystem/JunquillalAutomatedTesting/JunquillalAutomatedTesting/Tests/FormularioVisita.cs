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
    public class HacerUnaVisitaTest
    {

        IWebDriver driver = null;
        FormVisitaCantidadPersonas paginaVisitaCantidadPersonas;

        public HacerUnaVisitaTest()
        {
            driver = new ChromeDriver();
            paginaVisitaCantidadPersonas = new(driver);
        }

        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Visita/FormularioCantidadPersonas");
            driver.Manage().Window.Maximize();
        }

        public void TearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void HacerUnaVisita_FormaIncorrectaNoSeAgregaNingunDato()
        {
            // Arrange
            Setup();
            // Act
            paginaVisitaCantidadPersonas.CompletarPaginaConDatosDePruebaDondeTodosLosCamposSon0();
            var mensajeError = paginaVisitaCantidadPersonas.ObtenerMensajeDeError();
            // Assert
            Assert.That(mensajeError.Text, Is.EqualTo("Revise que haya al menos una persona"));
            TearDown();
        }

    }

}
