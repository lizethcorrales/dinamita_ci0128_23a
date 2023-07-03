using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Areas.Admin.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace JunquillalUserSystemTest.Controllers
{
    // Pruebas Unitarias de Gabriel Chacón
    [TestClass]
    public class LoginControllerTest
    {
        //En esta clase se verifica que el metodo de este Controlador
        //funcione correctamente
        [TestMethod]
        public void LoginValido()
        {
            // Arrange
            LoginController controller = new LoginController();
            TrabajadorModelo empleadoExistente = new TrabajadorModelo();
            empleadoExistente.ID = "211118888";
            empleadoExistente.Contrasena = "1";
            empleadoExistente.Puesto = "Administrador";
            string actionEsperada = "Index";
            string controladorEsperado = "Home";

            //Act
            var resultado = controller.Login(empleadoExistente,0) as RedirectToActionResult;


            // Assert
            Assert.AreEqual(actionEsperada, resultado.ActionName);
            Assert.AreEqual(controladorEsperado, resultado.ControllerName);
        }


        [TestMethod]
        public void LoginContrasenaIncorrecta()
        {
            // Arrange
            TrabajadorModelo empleadoCI = new();
            empleadoCI.ID = "211118888";
            empleadoCI.Contrasena = "sssss";
            LoginController controller = new LoginController();
            string mensajeEsperado = "La contraseña es incorrecta";

            // Act
            var resultado = controller.Login(empleadoCI,0) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void LoginUsuarioNoExistente()
        {
            // Arrange
            LoginController controller = new LoginController();
            TrabajadorModelo empleadoUV = new();
            empleadoUV.ID = "211118887";
            empleadoUV.Contrasena = "sssss";
            string mensajeEsperado = "Usuario no registrado";

            // Act
            var resultado = controller.Login(empleadoUV,0) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void LoginUsuarioNoValido()
        {
            // Arrange
            LoginController controller = new LoginController();
            TrabajadorModelo empleadoNV = new();
            empleadoNV.ID = "2111";
            string mensajeEsperado = "Usuario no registrado";

            // Act
            var resultado = controller.Login(empleadoNV, 0) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void LoginTrabajadorNulo()
        {
            // Arrange
            TrabajadorModelo nulo = null;
            LoginController controller = new LoginController();
            string mensajeEsperado = "Hubo un problema en el sistema";

            // Act
            var resultado = controller.Login(nulo,0) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void LoginTrabajadorVacio()
        {
            // Arrange
            TrabajadorModelo empleadoVacio = new TrabajadorModelo();
            LoginController controller = new LoginController();
            string mensajeEsperado = "Usuario no registrado";

            // Act
            var resultado = controller.Login(empleadoVacio,0) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }


    }


}
