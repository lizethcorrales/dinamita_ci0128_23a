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
    public class HacerUnaVisitaTest
    {

        IWebDriver driver = null;
        private IJavaScriptExecutor js;
        public HacerUnaVisitaTest()
        {
            driver = new ChromeDriver();
        }
    
        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Visita/FormularioCantidadPersonas");
            js = (IJavaScriptExecutor)driver;
            driver.Manage().Window.Maximize();
        }
        public void tearDown()
        {
            driver.Quit();
        }
        [Test, Order(1)]
        public void HacerUnaVisita_FormaIncorrectaNoSeAgregaNingunDato()
        {
            Setup();
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement botonSiguiente = wait.Until(e => e.FindElement(By.Id("siguiente_calendario")));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);

            {

                IWebElement mensajeError = driver.FindElement(By.Id("error"));
                Assert.That(mensajeError.Text, Is.EqualTo("Revise que haya al menos una persona"));
            }
            tearDown();
        }

    }

}
