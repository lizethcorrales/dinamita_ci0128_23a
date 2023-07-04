using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace JunquillalAutomatedTesting.PageObjectModels
{
    public class FormAdministrarReserva : BasePage
    {
        private By botonFechaBy;
        private By inputFechaBy;
        private By mensajeErrorBy;
        private FormLoginPage loginPage;  

        public FormAdministrarReserva(IWebDriver driver) : base(driver)
        {
            loginPage = new(driver);
            loginPage.AgregarCredencialesDePrueba("211118888","1");
            loginPage.PresionarBotonIniciarSesion();
            driver.Navigate().GoToUrl("https://localhost:7042/Admin/AdministrarReservas/Reservas");
            botonFechaBy = By.Id("purple-buttonFecha");
            inputFechaBy = By.Id("fecha-input");
            mensajeErrorBy = By.Id("errorMessage"); 
        }

        public void BuscarReservacionPorFechaQueNoTengaFormatoYYYYMMDD(string entrada)
        {
            driver.FindElement(inputFechaBy).SendKeys(entrada);
            IWebElement botonSiguiente = driver.FindElement(botonFechaBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);
        }

        public IWebElement ObtenerMensajeDeError()
        {
            return driver.FindElement(mensajeErrorBy);
        }
    }
}