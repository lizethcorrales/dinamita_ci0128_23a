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
    public class FormRegistroPage : BasePage
    {
        private By inputNombreBy;
        private By inputApellido1By;
        private By inputApellido2By;
        private By inputCedulaBy;
        private By inputContrasenaBy;
        private By inputContrasenaDNBy;
        private By inputCorreoBy;
        private By mensajeErrorBy;
        private By botonRegistrarBy;
        private By mensajeRegistroBy;
        private By correoErrorBY;

        public FormRegistroPage(IWebDriver driver) : base(driver)
        {

            inputNombreBy = By.Id("nombre");
            inputApellido1By = By.Id("papellido");
            inputApellido2By = By.Id("sapellido");
            inputCedulaBy = By.Id("cedula");
            inputContrasenaBy = By.Id("contrasena");
            inputContrasenaDNBy = By.Id("contrasenaDN");
            inputCorreoBy = By.Id("correo");
            mensajeErrorBy = By.Id("Error");
            correoErrorBY = By.Id("correoError");
            mensajeRegistroBy = By.Id("mensaje");
            botonRegistrarBy = By.Id("botonRegistrar");
        }


        public void llenarCampoRegistro(string valor, By campoBy)
        {
            IWebElement inputRegistro = driver.FindElement(campoBy);
            inputRegistro.Clear();
            inputRegistro.SendKeys(valor);
        }

        public void registroValido()
        {
            IWebElement botonContinuar = driver.FindElement(botonRegistrarBy);
            string idAuxiliar = DateTime.Now.ToString("h:mm:ss");
            string idNueva = idAuxiliar.Replace(':', '1');
            idNueva = "12" + idNueva;
            llenarCampoRegistro("Michael", inputNombreBy);
            llenarCampoRegistro("Jackson", inputApellido1By);
            llenarCampoRegistro("Rockwell", inputApellido2By);
            llenarCampoRegistro("mjr89@gmail.com", inputCorreoBy);
            llenarCampoRegistro(idNueva, inputCedulaBy);
            llenarCampoRegistro("1234", inputContrasenaBy);
            llenarCampoRegistro("1234", inputContrasenaDNBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public void registroConCorreoInvalido()
        {
            IWebElement botonContinuar = driver.FindElement(botonRegistrarBy);
            string idAuxiliar = DateTime.Now.ToString("MM-dd-yyyy");
            string idNueva = idAuxiliar.Replace('-', '1');
            idNueva = "1" + idNueva;
            llenarCampoRegistro("Michael", inputNombreBy);
            llenarCampoRegistro("Jackson", inputApellido1By);
            llenarCampoRegistro("Rockwell", inputApellido2By);
            llenarCampoRegistro("noSoyUngmail.com", inputCorreoBy);
            llenarCampoRegistro(idNueva, inputCedulaBy);
            llenarCampoRegistro("1234", inputContrasenaBy);
            llenarCampoRegistro("1234", inputContrasenaDNBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public void registroConContrasenaDistinta()
        {
            IWebElement botonContinuar = driver.FindElement(botonRegistrarBy);
            string idAuxiliar = DateTime.Now.ToString("MM-dd-yyyy");
            string idNueva = idAuxiliar.Replace('-', '1');
            idNueva = "1" + idNueva;
            llenarCampoRegistro("Michael", inputNombreBy);
            llenarCampoRegistro("Jackson", inputApellido1By);
            llenarCampoRegistro("Rockwell", inputApellido2By);
            llenarCampoRegistro("mjr89@gmail.com", inputCorreoBy);
            llenarCampoRegistro(idNueva, inputCedulaBy);
            llenarCampoRegistro("2345", inputContrasenaBy);
            llenarCampoRegistro("1234", inputContrasenaDNBy);
            IJavaScriptExecutor jsExecutor = (IJavaScriptExecutor)driver;
            jsExecutor.ExecuteScript("arguments[0].click();", botonContinuar);
        }

        public IWebElement ObtenerMensajeError()
        {
            IWebElement error = driver.FindElement(mensajeErrorBy);

            return error;
        }
        public IWebElement ObtenerMensajeErrorCorreo()
        {
            IWebElement errorCorreo = driver.FindElement(correoErrorBY);

            return errorCorreo;
        }

        public IWebElement ObtenerMensajeRegistro()
        {
            IWebElement mensaje = driver.FindElement(mensajeRegistroBy);

            return mensaje;
        }
    }
}
