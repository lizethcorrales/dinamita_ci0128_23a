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
        private By inputCantidadAdultosNacionalBy;
        private By inputCantidadNinosNacionalMayores6By;
        private By inputCantidadNinosNacionalMenores6By;
        private By inputCantidadAdultosMayoresNacionalBy;
        private By inputCantidadAdultosExtranjeroBy;
        private By inputCantidadNinosExtranjeroBy;
        private By inputCantidadAdultosMayorExtranjeroBy;
        private By botonSiguienteBy;

        public FormCantidadPersonas(IWebDriver driver) : base(driver) 
        {         
            inputCantidadAdultosNacionalBy = By.Name("cantidad_Adultos_Nacional");
            inputCantidadNinosNacionalMayores6By = By.Name("cantidad_Ninnos_Nacional_mayor6");
            inputCantidadNinosNacionalMenores6By = By.Name("cantidad_Ninnos_Nacional_menor6");
            inputCantidadAdultosMayoresNacionalBy = By.Name("cantidad_adulto_mayor");
            inputCantidadAdultosExtranjeroBy = By.Name("cantidad_Adultos_Extranjero");
            inputCantidadNinosExtranjeroBy = By.Name("cantidad_ninnos_extranjero");
            inputCantidadAdultosMayorExtranjeroBy = By.Name("cantidad_adultoMayor_extranjero");
            botonSiguienteBy = By.Id("siguiente_calendario");
        }

        public override void LlenarDatosPagina()
        {
            IWebElement botonSiguiente = driver.FindElement(botonSiguienteBy);
            rellenarCampoCantidadPoblacion("1", inputCantidadAdultosNacionalBy);
            botonSiguiente.Click();
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
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosNacionalBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosNacionalMayores6By);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosNacionalMenores6By);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosMayoresNacionalBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosExtranjeroBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadNinosExtranjeroBy);
            rellenarCampoCantidadPoblacion(valor, inputCantidadAdultosMayorExtranjeroBy);
        }
    }
}