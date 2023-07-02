using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Xml.Linq;
using JunquillalAutomatedTesting.PageObjectModels;

// Gabriel Chacon Garro
namespace JunquillalAutomatedTesting.Tests
{
    public class TestFormularioRegistro
    {
        IWebDriver driver = null;
        FormRegistroPage paginaRegistro = null;

        public TestFormularioRegistro()
        {
            driver = new ChromeDriver();
        }

        public void Setup()
        {
            driver.Navigate().GoToUrl("https://localhost:7042/Admin/Registro/Registro");
            paginaRegistro = new(driver);
            driver.Manage().Window.Maximize();
        }

        public void TearDown()
        {
            driver.Quit();
        }


        [Test, Order(1)]
        public void ComprobarRegistroConContrasenaDistinta()
        {
            //Arrange
            Setup();

            //Act
            paginaRegistro.registroConContrasenaDistinta();
            IWebElement mensajeError = paginaRegistro.ObtenerMensajeError();

            //Assert
            Assert.AreEqual(mensajeError.Text, "Las contraseñas no coinciden");
        }

        [Test, Order(2)]
        public void ComprobarRegistroConCorreoInvalido()
        {
            //Arrange
            Setup();

            //Act
            paginaRegistro.registroConCorreoInvalido();
            IWebElement mensajeError = paginaRegistro.ObtenerMensajeErrorCorreo();

            //Assert
            Assert.AreEqual(mensajeError.Text, "Escriba un correo valido");
        }


        [Test, Order(3)]
        public void ComprobarRegistroValido()
        {
            //Arrange
            Setup();

            //Act
            paginaRegistro.registroValido();
            IWebElement mensajesRegistro = paginaRegistro.ObtenerMensajeRegistro();

            //Assert
            Assert.AreEqual(mensajesRegistro.Text, "Usuario Registrado Exitosamente");
        }

    }
}
