using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace JunquillalAutomatedTesting.PageObjectModels
{
    public class FormCantidadPersonas : BasePage
    {
        private By inputCantidadAdultosExtranjeroBy;
        private By inputCantidadAdultosMayorExtranjeroBy;
        private By inputCantidadNinosExtranjeroBy;
        private By inputCantidadNinosExtranjeroMenores6By;
        private By inputCantidadAdultosNacionalBy;
        private By inputCantidadAdultosMayoresNacionalBy;
        private By inputCantidadAdultosMayores50NacionalBy;
        private By inputCantidadNinosNacionalMayores6By;
        private By inputCantidadNinosNacionalMenores6By;
        private By botonSiguienteBy;
        private By errorBy;

        public FormCantidadPersonas(IWebDriver driver) : base(driver) 
        {
            inputCantidadAdultosExtranjeroBy = By.Name("AdultoExtranjero");
            inputCantidadAdultosMayorExtranjeroBy = By.Name("AdultoMayorExtranjero");
            inputCantidadNinosExtranjeroBy = By.Name("NiñoExtranjero");
            inputCantidadNinosExtranjeroMenores6By = By.Name("Niñomenor6añosExtranjero");
            inputCantidadAdultosNacionalBy = By.Name("AdultoNacional");
            inputCantidadAdultosMayoresNacionalBy = By.Name("AdultoMayorNacional");
            inputCantidadAdultosMayores50NacionalBy = By.Name("Adultomayorde50Nacional");
            inputCantidadNinosNacionalMayores6By = By.Name("NiñoNacional");
            inputCantidadNinosNacionalMenores6By = By.Name("Niñomenor6añosNacional");
            errorBy = By.Id("error");
            botonSiguienteBy = By.Id("siguiente_calendario");
        }

        public void CompletarPaginaCantidadPersonasYDarleSiguiente()
        {
            IWebElement botonSiguiente = driver.FindElement(botonSiguienteBy);
            rellenarCampoCantidadPoblacion("1", inputCantidadAdultosExtranjeroBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);
        }

        // Rellena cualquier campo según el tipo de población que se le pase por parámetro
        public void rellenarCampoCantidadPoblacion(string valor, By poblacionBy) 
        {
            IWebElement inputCantidadPoblacion = driver.FindElement(poblacionBy);
            inputCantidadPoblacion.Clear();
            inputCantidadPoblacion.SendKeys(valor); // Cambia valor del input
        }

        public void rellenarTodosLosInputs(string valor)
        {
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosExtranjeroBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosMayorExtranjeroBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosExtranjeroBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosExtranjeroMenores6By);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosNacionalBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosMayoresNacionalBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosMayores50NacionalBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosNacionalMayores6By);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosNacionalMenores6By);
        }

        public IWebElement ObtenerMensajeDeError()
        {
            IWebElement errorEnElFormularioCantidadPersonas = driver.FindElement(errorBy);
   
            return errorEnElFormularioCantidadPersonas;
        }

        public void CompletarPaginaConDatosDePruebaDondeTodosLosCamposSon0()
        {
            IWebElement botonSiguiente = driver.FindElement(botonSiguienteBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);
        }

        public void CompletarPaginaConDatosDePruebaDondeLaCantidadTotalExcedeLaMaxima()
        {
            IWebElement botonSiguiente = driver.FindElement(botonSiguienteBy);
            rellenarCampoCantidadPoblacion("35", inputCantidadAdultosExtranjeroBy);
            rellenarCampoCantidadPoblacion("30", inputCantidadAdultosMayorExtranjeroBy);
            rellenarCampoCantidadPoblacion("15", inputCantidadNinosExtranjeroBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);
        }

        public void CompletarPaginaConDatosDePruebaDondeSoloHayNinnos()
        {
            IWebElement botonSiguiente = driver.FindElement(botonSiguienteBy);
            rellenarCampoCantidadPoblacion("1", inputCantidadNinosExtranjeroBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonSiguiente);
        }
    }
}