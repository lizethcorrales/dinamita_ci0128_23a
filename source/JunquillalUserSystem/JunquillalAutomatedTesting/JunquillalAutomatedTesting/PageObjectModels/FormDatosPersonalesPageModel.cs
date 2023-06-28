using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace JunquillalAutomatedTesting.PageObjectModels
{
    public class FormDatosPersonales : BasePage
    {
        private FormCalendarioPage formCalendarioPage;
        private By inputNombreBy;
        private By inputPrimerApellidoBy;
        private By inputSegundaApellidoBy;
        private By inputIdentificacionBy;
        private By inputTelefonoBy;
        private By inputEmailBy;
        private By radioTipoNacionalBy;
        private By radioTipoExtranjerolBy;
        private By inputPaisBy;
        private By inputProvinciaBy;
        private By radioOcioBy;
        private By radioAcademicoBy;
        private By inputCantidadVehiculosBy;
        private By botonContinuarBy;
        private By mensajeErrorNombreBy;
        private By mensajeErrorPrimerApellidoBy;
        private By mensajeErrorIDBy;

        public FormDatosPersonales(IWebDriver driver) : base(driver) 
        {
            formCalendarioPage = new(driver);
            formCalendarioPage.ElegirFechaEntradaYSalidaYDarleSiguiente();
            inputNombreBy = By.Id("nombre");
            inputPrimerApellidoBy = By.Id("primerApellido");
            inputSegundaApellidoBy = By.Id("segundoApellido");
            inputIdentificacionBy = By.Id("identificacion");
            inputTelefonoBy = By.Id("telefono");
            inputEmailBy = By.Id("email");
            radioTipoNacionalBy = By.Id("Nacional");
            radioTipoExtranjerolBy = By.Id("Extranjero");
            inputPaisBy = By.Id("pais");
            inputProvinciaBy = By.Id("provincia");
            radioOcioBy = By.Id("Ocio");
            radioAcademicoBy = By.Id("Academico");
            inputCantidadVehiculosBy = By.Id("vehiculo");
            botonContinuarBy = By.Id("continuar");
            mensajeErrorNombreBy = By.Id("mensajeErrorNombre");
            mensajeErrorPrimerApellidoBy = By.Id("mensajeErrorApellido1");
            mensajeErrorIDBy = By.Id("mensajeErrorIdent");
        }

        public void CompletarPaginaDatosPersonalesConDatosDePrueba()
        {
            llenarInput("Ana", inputNombreBy);
            llenarInput("Robles", inputPrimerApellidoBy);
            llenarInput("Vargas", inputSegundaApellidoBy);
            llenarInput("88884444", inputTelefonoBy);
            llenarInput("111110011", inputIdentificacionBy);
            llenarInput("ARV@outlook.com", inputEmailBy);
            IWebElement botonContinuar = driver.FindElement(botonContinuarBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public void CompletarPaginaDatosPersonalesConDatosDePruebaPeroSinNombre()
        {
            llenarInput("Robles", inputPrimerApellidoBy);
            llenarInput("Vargas", inputSegundaApellidoBy);
            llenarInput("88884444", inputTelefonoBy);
            llenarInput("111110011", inputIdentificacionBy);
            llenarInput("ARV@outlook.com", inputEmailBy);
            IWebElement botonContinuar = driver.FindElement(botonContinuarBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public void CompletarPaginaDatosPersonalesConDatosDePruebaPeroSinPrimerApellido()
        {
            llenarInput("Ana", inputNombreBy);
            llenarInput("Vargas", inputSegundaApellidoBy);
            llenarInput("88884444", inputTelefonoBy);
            llenarInput("111110011", inputIdentificacionBy);
            llenarInput("ARV@outlook.com", inputEmailBy);
            IWebElement botonContinuar = driver.FindElement(botonContinuarBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public void CompletarPaginaDatosPersonalesConDatosDePruebaPeroSinID()
        {
            llenarInput("Ana", inputNombreBy);
            llenarInput("Robles", inputPrimerApellidoBy);
            llenarInput("Vargas", inputSegundaApellidoBy);
            llenarInput("88884444", inputTelefonoBy);
            llenarInput("ARV@outlook.com", inputEmailBy);
            IWebElement botonContinuar = driver.FindElement(botonContinuarBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        // Rellena cualquier campo que sea de tipo input
        public void llenarInput(string valor, By inputBy)
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement input = wait.Until(e => e.FindElement(inputBy));
            input.Clear();
            input.SendKeys(valor); // Cambia valor del input
        }

        public void darleClickAlBotonContinuarConFormularioEnBlanco()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement botonContinuar = wait.Until(e => e.FindElement(botonContinuarBy));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public List<IWebElement> ObtenerMensajesDeError()
        {
            List<IWebElement> listaMensajesError = new();
            IWebElement ErrorDeNombre = driver.FindElement(mensajeErrorNombreBy);
            IWebElement ErrorDePrimerApellido = driver.FindElement(mensajeErrorPrimerApellidoBy);
            IWebElement ErrorDeID = driver.FindElement(mensajeErrorIDBy);
            if (ErrorDeNombre.Displayed) 
                listaMensajesError.Add(ErrorDeNombre);
            if (ErrorDePrimerApellido.Displayed)
                    listaMensajesError.Add(ErrorDePrimerApellido);
            if (ErrorDeID.Displayed)
                listaMensajesError.Add(ErrorDeID);
            return listaMensajesError;
        }

        public ReadOnlyCollection<IWebElement> SeleccionarTipoIDNacional()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement radioTipoNacional = wait.Until(e => e.FindElement(radioTipoNacionalBy));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", radioTipoNacional);
            var dropDownProvincia = driver.FindElements(inputProvinciaBy);
            return dropDownProvincia;
        }

        public IWebElement SeleccionarTipoIDExtranjero()
        {
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement radioTipoExtranjero = wait.Until(e => e.FindElement(radioTipoExtranjerolBy));
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", radioTipoExtranjero);
            IWebElement dropDownProvincia = driver.FindElement(inputProvinciaBy);
            return dropDownProvincia;
        }
    }
}