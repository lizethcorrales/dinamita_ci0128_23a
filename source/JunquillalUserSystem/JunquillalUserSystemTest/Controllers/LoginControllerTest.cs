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
            TrabajadorModelo empleadoExistente = new();
            empleadoExistente.ID = "211118888";
            empleadoExistente.Contrasena = "1";
            empleadoExistente.Puesto = "Administrador";
            string actionEsperada = "Index";
            string controladorEsperado = "Home";

            //Act
            var resultado = controller.Login(empleadoExistente) as RedirectToActionResult;


            // Assert
            Assert.AreEqual(actionEsperada, resultado.ActionName);
            Assert.AreEqual(controladorEsperado, resultado.ControllerName);
        }

        [TestMethod]
        public void LoginPuestoIncorrecto()
        {
            // Arrange
            LoginController controller = new LoginController();
            TrabajadorModelo empleadoPI = new();
            empleadoPI.ID = "211118888";
            empleadoPI.Contrasena = "1";
            empleadoPI.Puesto = "Operativo";
            string mensajeEsperado = "El puesto es incorrecto";
            controller.ViewData["Mensaje"] = mensajeEsperado;

            // Act
            var resultado = controller.Login(empleadoPI) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
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
            controller.ViewData["Mensaje"] = mensajeEsperado;

            // Act
            var resultado = controller.Login(empleadoCI) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void LoginUsuarioNoValido()
        {
            // Arrange
            LoginController controller = new LoginController();
            TrabajadorModelo empleadoUV = new();
            empleadoUV.ID = "211118887";
            empleadoUV.Contrasena = "sssss";
            string mensajeEsperado = "Usuario no registrado";
            controller.ViewData["Mensaje"] = mensajeEsperado;

            // Act
            var resultado = controller.Login(empleadoUV) as ViewResult;

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
            controller.ViewData["Mensaje"] = mensajeEsperado;

            // Act
            var resultado = controller.Login(nulo) as ViewResult;

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
            controller.ViewData["Mensaje"] = mensajeEsperado;

            // Act
            var resultado = controller.Login(empleadoVacio) as ViewResult;

            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }


    }


}
