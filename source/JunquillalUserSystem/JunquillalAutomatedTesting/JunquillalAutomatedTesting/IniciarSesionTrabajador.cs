// Generated by Selenium IDE
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
using static System.Net.WebRequestMethods;


namespace JunquillalAutomatedTesting
{
    [TestFixture]

    public class IniciarSesionTrabajador
    {

        private IWebDriver driver;
        public IDictionary<string, object> vars { get; private set; }
        private IJavaScriptExecutor js;



        [SetUp]
        public void SetUp()
        {
            driver = new ChromeDriver();
            js = (IJavaScriptExecutor)driver;
            vars = new Dictionary<string, object>();
            driver.Navigate().GoToUrl("https://localhost:7042/Admin");
            driver.Manage().Window.Maximize();
        }
        [TearDown]
        protected void TearDown()
        {
            driver.Quit();
        }


        [Test, Order(2)]
        public void inicioSesionCredencialesErroneos()
        {
            
            driver.FindElement(By.Id("usuario")).Click();
            driver.FindElement(By.Id("usuario")).SendKeys("rgfrrsgfe");
            driver.FindElement(By.Id("contrasena")).Click();
            driver.FindElement(By.Id("contrasena")).SendKeys("gsfd");
            driver.FindElement(By.Id("botonContinuar")).Click();
            driver.FindElement(By.Id("usuarioError")).Click();
            

            IWebElement mensajeErrorID= driver.FindElement(By.Id("usuarioError"));
            Assert.That(mensajeErrorID.Text, Is.EqualTo("Escriba un ID valido"));
           

        }

        [Test, Order(1)]
        public void inicioSecionManejoCredencialesVacios()
        {
    
            driver.FindElement(By.Id("botonContinuar")).Click();
            driver.FindElement(By.Id("usuarioError")).Click();
            driver.FindElement(By.Id("contraError")).Click();

            IWebElement mensajeErrorID = driver.FindElement(By.Id("usuarioError"));
            Assert.That(mensajeErrorID.Text, Is.EqualTo("Por favor escriba su identificaci�n"));

            IWebElement mensajeErrorContrasenna = driver.FindElement(By.Id("contraError"));
            Assert.That(mensajeErrorContrasenna.Text, Is.EqualTo("Por favor escriba su contrase�a"));
        }

        [Test, Order(3)]
        public void inicioSesionCredencialesCorrectos()
        {
            LimpiarDatos();
            driver.Manage().Window.Size = new System.Drawing.Size(1552, 832);
            driver.FindElement(By.Id("usuario")).Click();
            driver.FindElement(By.Id("usuario")).SendKeys("211118888");
            driver.FindElement(By.Id("contrasena")).Click();
            driver.FindElement(By.Id("contrasena")).SendKeys("1");
            driver.FindElement(By.Id("botonContinuar")).Click();

            string urlActual = driver.Url;  // Obtener el URL actual de la p�gina
            string urlEsperado = "https://localhost:7042/Admin/Home/Index";  // Obtener el URL actual de la p�gina
            Assert.AreEqual(urlEsperado, urlActual, "El URL de la p�gina no coincide con el esperado.");
            TearDown();

        }

        public void LimpiarDatos()
        {
            driver.FindElement(By.Id("usuario")).SendKeys("");
            driver.FindElement(By.Id("contrasena")).SendKeys("");
        }
    }
}