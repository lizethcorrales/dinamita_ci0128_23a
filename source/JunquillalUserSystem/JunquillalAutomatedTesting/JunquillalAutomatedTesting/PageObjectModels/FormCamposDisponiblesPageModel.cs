using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

// Gabriel Chacon Garro
namespace JunquillalAutomatedTesting.PageObjectModels
{
    public class FormCamposDisponiblesPage : BasePage
    {
        private By inputFechaBy;
        private By inputResultado1By;
        private By botonBuscarBy;
        private By mensajeErrorCamposBy;

        public FormCamposDisponiblesPage(IWebDriver driver) : base(driver)
        {
            inputFechaBy = By.Id("fecha-entrada");
            inputResultado1By = By.Id("campos");
            botonBuscarBy = By.Id("buscar");
            mensajeErrorCamposBy = By.Id("mensajeErrorCampos");
        }

        public void PresionarBotonBuscar()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement botonBuscar = wait.Until(e => e.FindElement(botonBuscarBy));
            botonBuscar.Click();
        }

        public IWebElement obtenerMensajeDeError()
        {
            return driver.FindElement(mensajeErrorCamposBy);
        }

        public void elegirNumeroDeDia(string fecha)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement inputFechaEntrada = wait.Until(e => e.FindElement(inputFechaBy));
            inputFechaEntrada.Click();
            IWebElement fechaEntradaIngresada = driver.FindElement(By.LinkText(fecha));
            fechaEntradaIngresada.Click();
        }
    }
}
