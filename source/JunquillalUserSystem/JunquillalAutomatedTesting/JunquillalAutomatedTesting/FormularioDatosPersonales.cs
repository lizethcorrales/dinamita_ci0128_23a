using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;

namespace JunquillalAutomatedTesting
{
    [TestFixture]
    public class TestsDatos
    {
        IWebDriver driver = null;
        public TestsDatos()
        {
            driver = new ChromeDriver();
        }

        [SetUp]
        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Reservacion/FormularioCantidadPersonas");
            driver.Manage().Window.Maximize();
        }

        public void tearDown()
        {
            driver.Quit();
        }

        [Test, Order(1)]
        public void ComprobarValidacionCampos()
        {    
            irAPaginaFormularioDatosPersonales();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement botonContinuar = wait.Until(e => e.FindElement(By.Id("continuar")));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
            IWebElement mensajeErrorNombre = driver.FindElement(By.Id("mensajeErrorNombre"));
            IWebElement mensajeErrorApellido = driver.FindElement(By.Id("mensajeErrorApellido1"));
            IWebElement mensajeErrorIdentificacion = driver.FindElement(By.Id("mensajeErrorIdent"));
            Assert.AreEqual("Nombre NO válido", mensajeErrorNombre.Text);
            Assert.AreEqual("Apellido NO válido", mensajeErrorApellido.Text);
            Assert.AreEqual("Identificación NO válido", mensajeErrorIdentificacion.Text);
        }

        [Test, Order(2)]
        public void ComprobarQueExisteProvinciaConIdentificacionNacional()
        {
            Setup();
            irAPaginaFormularioDatosPersonales();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement radioNacional = wait.Until(e => e.FindElement(By.Id("Nacional")));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", radioNacional);
            var dropDownProvincia = driver.FindElements(By.Id("provincia"));
            Assert.True(dropDownProvincia.Count == 1);
        }

        [Test, Order(3)]
        public void ComprobarQueDesapareceProvinciaConIdentificacionExtranjera()
        {
            Setup();
            irAPaginaFormularioDatosPersonales();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement radioExtranjero = wait.Until(e => e.FindElement(By.Id("Extranjero")));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", radioExtranjero);
            IWebElement dropDownProvincia = driver.FindElement(By.Id("provincia"));
            Boolean visibility = dropDownProvincia.Displayed;
            Assert.IsFalse(visibility);
            tearDown();
        }

        public void CompletarPaginaCantidadPersonas()
        {
            IWebElement inputCantidadAdultosNacional = driver.FindElement(By.Name("cantidad_Adultos_Nacional"));
            IWebElement botonSiguiente = driver.FindElement(By.Id("siguiente_calendario"));
            inputCantidadAdultosNacional.Clear();
            inputCantidadAdultosNacional.SendKeys("1"); // Cambia valor del input
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);
        }

        public void CompletarPaginaCalendario()
        {
            IWebElement inputFechaEntrada = driver.FindElement(By.Name("fecha-entrada"));
            IWebElement inputFechaSalida = driver.FindElement(By.Name("fecha-salida"));
            IWebElement botonSiguiente = driver.FindElement(By.Id("btn-siguiente"));
            inputFechaEntrada.Click();
            IWebElement flechaSiguienteMes = driver.FindElement(By.CssSelector(".ui-icon-circle-triangle-e"));
            flechaSiguienteMes.Click();
            IWebElement fechaEntradaElegida = driver.FindElement(By.LinkText("19"));
            fechaEntradaElegida.Click();
            inputFechaEntrada.Click();
            inputFechaSalida.Click();
            IWebElement fechaSalidaElegida = driver.FindElement(By.LinkText("24"));
            fechaSalidaElegida.Click();
            botonSiguiente.Click();
        }

        public void irAPaginaFormularioDatosPersonales()
        {
            CompletarPaginaCantidadPersonas();
            CompletarPaginaCalendario();
        }
    }
}