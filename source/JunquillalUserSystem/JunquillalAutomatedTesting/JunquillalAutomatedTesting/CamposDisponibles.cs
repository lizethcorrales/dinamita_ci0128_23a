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

namespace JunquillalAutomatedTesting
{
    [TestFixture]
    public class CamposDisponiblesTests
    {
        IWebDriver driver = null;
        public CamposDisponiblesTests()
        {
            driver = new ChromeDriver();
        }

        [SetUp]
        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/CamposDisponibles/CamposDisponibles");
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void tearDown()
        {
            driver.Quit();
        }

        [Test]
        public void CamposDisponibles_BuscarCamposSinFechaEntrada()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement botonBuscar = wait.Until(e => e.FindElement(By.Id("buscar")));
            botonBuscar.Click();
            IWebElement mensajeErrorCampos = driver.FindElement(By.Id("mensajeErrorCampos"));
            Assert.AreEqual("Debe de ingresar una fecha válida", mensajeErrorCampos.Text);
        }

    }
}
