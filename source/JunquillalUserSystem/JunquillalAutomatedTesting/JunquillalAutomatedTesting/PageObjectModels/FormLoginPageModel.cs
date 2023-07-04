using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Linq;

namespace JunquillalAutomatedTesting.PageObjectModels
{
    public class FormLoginPage : BasePage
    {
        private By inputIDBy;
        private By inputContraseniaBy;
        private By usuarioErrorBy;
        private By contraseniaErrorBy;
        private By botonIniciarSesionBy;

        public FormLoginPage(IWebDriver driver) : base(driver)
        {

            inputIDBy = By.Id("usuario");
            inputContraseniaBy = By.Id("contrasena");
            usuarioErrorBy = By.Id("usuarioError");
            contraseniaErrorBy = By.Id("contraError");
            botonIniciarSesionBy = By.Id("botonContinuar");
        }

        public void PresionarBotonIniciarSesion()
        {
            driver.FindElement(botonIniciarSesionBy).Click();
        }

        public List<IWebElement> ObtenerMensajesDeError()
        {
            List<IWebElement> listaMensajesError = new();
            IWebElement ErrorDeUsuario = driver.FindElement(usuarioErrorBy);
            IWebElement ErrorDeContrasenia = driver.FindElement(contraseniaErrorBy);
            if (ErrorDeUsuario.Displayed)
                listaMensajesError.Add(ErrorDeUsuario);
            if (ErrorDeContrasenia.Displayed)
                listaMensajesError.Add(ErrorDeContrasenia);
            return listaMensajesError;
        }

        public void AgregarCredencialesDePrueba(string usuarioDigitado, string contraseniaDigitada)
        {
            IWebElement usuario = driver.FindElement(inputIDBy);
            usuario.Click();
            usuario.SendKeys(usuarioDigitado);
            IWebElement contrasenia = driver.FindElement(inputContraseniaBy);
            contrasenia.Click();
            contrasenia.SendKeys(contraseniaDigitada);
        }

        public void LimpiarDatos()
        {
            driver.FindElement(inputIDBy).SendKeys("");
            driver.FindElement(inputContraseniaBy).SendKeys("");
        }
    }
}
