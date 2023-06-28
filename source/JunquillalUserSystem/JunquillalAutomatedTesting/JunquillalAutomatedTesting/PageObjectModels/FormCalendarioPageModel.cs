using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace JunquillalAutomatedTesting.PageObjectModels
{
    public class FormCalendarioPage : BasePage
    {
        private FormCantidadPersonas formCantidadPersonas;
        private By inputFechaEntradaBy;
        private By inputFechaSalidaBy;
        private By botonSiguienteBy;
        private By flechaSiguienteMesBy;
        private By fechaEntradaElegidaBy;
        private By fechaSalidaElegidaBy;

        public FormCalendarioPage(IWebDriver driver) : base(driver) 
        {
            formCantidadPersonas = new(driver);
            formCantidadPersonas.CompletarPaginaCantidadPersonasYDarleSiguiente();
            inputFechaEntradaBy = By.Name("fecha-entrada");
            inputFechaSalidaBy = By.Name("fecha-salida");
            botonSiguienteBy = By.Id("btn-siguiente");
            flechaSiguienteMesBy = By.CssSelector(".ui-icon-circle-triangle-e");
            fechaEntradaElegidaBy = By.LinkText("19");
            fechaSalidaElegidaBy = By.LinkText("24");
        }

        public void ElegirFechaEntradaYSalidaYDarleSiguiente()
        {         
            IWebElement inputFechaEntrada = driver.FindElement(inputFechaEntradaBy);
            IWebElement inputFechaSalida = driver.FindElement(inputFechaSalidaBy);
            IWebElement botonSiguiente = driver.FindElement(botonSiguienteBy);
            inputFechaEntrada.Click();
            IWebElement flechaSiguienteMes = driver.FindElement(flechaSiguienteMesBy);
            flechaSiguienteMes.Click();
            IWebElement fechaEntradaElegida = driver.FindElement(fechaEntradaElegidaBy);
            fechaEntradaElegida.Click();
            inputFechaEntrada.Click();
            inputFechaSalida.Click();
            IWebElement fechaSalidaElegida = driver.FindElement(fechaSalidaElegidaBy);
            fechaSalidaElegida.Click();
            botonSiguiente.Click();
        }
    }
}