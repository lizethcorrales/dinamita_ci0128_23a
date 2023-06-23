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


namespace JunquillalAutomatedTesting

{

    [TestFixture]
    public class FormaIncorrectoBuscarReservaPorFechaTest
    {
        private IWebDriver driver = null;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;


        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver(); 
            driver.Manage().Window.Maximize();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }
        [Test]
        public void formaIncorrectoBuscarReservaPorFecha()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Admin");
            driver.Manage().Window.Maximize();
            driver.FindElement(By.Id("usuario")).SendKeys("211118888");
            driver.FindElement(By.Id("contrasena")).SendKeys("1");
            driver.FindElement(By.Id("botonContinuar")).Click();
            driver.Navigate().GoToUrl("https://localhost:7042/Admin/AdministrarReservas/Reservas");
            driver.FindElement(By.Id("fecha-input")).SendKeys("jhgdfsebtr");
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement botonContinuar = wait.Until(e => e.FindElement(By.Id("purple-buttonFecha")));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
     
            {
             
                IWebElement mensajeErrorIdentificacion = driver.FindElement(By.Id("errorMessage"));
                Assert.That(mensajeErrorIdentificacion.Text, Is.EqualTo("Formato de fecha inválido. Utiliza el formato yyyy-mm-dd"));
            }
        }
    }

}
