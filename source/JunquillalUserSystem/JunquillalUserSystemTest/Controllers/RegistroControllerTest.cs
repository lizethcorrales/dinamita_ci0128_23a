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
    public class RegistroControllerTest
    {
        //En esta clase se verifica que el metodo de este Controlador
        //funcione correctamente
        [TestMethod]
        public void RegistroValido()
        {
            // Arrange
            RegistroController controller = new RegistroController();
            TrabajadorModelo empleadoNuevo = new TrabajadorModelo();
            string idAuxiliar = DateTime.Now.ToString("h:mm:ss");
            string idNueva = idAuxiliar.Replace(':', '1');
            idNueva = "70" + idNueva;
            empleadoNuevo.ID = idNueva;
            empleadoNuevo.Puesto = "Administrador";
            empleadoNuevo.Nombre = "Jane";
            empleadoNuevo.Apellido1 = "Doe";
            empleadoNuevo.Apellido2 = "NA";
            empleadoNuevo.Contrasena = "dd";
            empleadoNuevo.Correo = "Jane@gmail.com";
            string mensajeEsperado = "Usuario Registrado Exitosamente";

            //Act
            var resultado = controller.Registro(empleadoNuevo) as ViewResult;


            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }


        [TestMethod]
        public void RegistroConUsuarioYaExistente()
        {
            // Arrange
            RegistroController controller = new RegistroController();
            TrabajadorModelo empleadoExistente= new TrabajadorModelo();
            empleadoExistente.ID = "211118888";
            empleadoExistente.Puesto = "Administrador";
            empleadoExistente.Nombre = "Jane";
            empleadoExistente.Apellido1 = "Doe";
            empleadoExistente.Apellido2 = "NA";
            empleadoExistente.Contrasena = "dd";
            empleadoExistente.Correo = "Jane@gmail.com";
            string mensajeEsperado = "El usuario ya estaba registrado en el sistema";

            //Act
            var resultado = controller.Registro(empleadoExistente) as ViewResult;


            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void RegistroConEmpleadoNull()
        {
            // Arrange
            RegistroController controller = new RegistroController();
            TrabajadorModelo empleadoNuevo = null;
            string mensajeEsperado = "Hubo un problema en el sistema";

            //Act
            var resultado = controller.Registro(empleadoNuevo) as ViewResult;


            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void RegistroConDatosInvalidos()
        {
            // Arrange
            RegistroController controller = new RegistroController();
            TrabajadorModelo empleadoExistente = new TrabajadorModelo();
            empleadoExistente.ID = "211118887";
            empleadoExistente.Puesto = "Administrador";
            empleadoExistente.Nombre = null;
            empleadoExistente.Apellido1 = "Guido";
            empleadoExistente.Apellido2 = "NA";
            empleadoExistente.Contrasena = "dd";
            empleadoExistente.Correo = "Jane@gmail.com";
            string mensajeEsperado = "No fue posible registrar el Usuario";

            //Act
            var resultado = controller.Registro(empleadoExistente) as ViewResult;


            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

        [TestMethod]
        public void RegistroConEmpleadoVacio()
        {
            // Arrange
            RegistroController controller = new RegistroController();
            TrabajadorModelo empleadoExistente = new TrabajadorModelo();
            string mensajeEsperado = "No fue posible registrar el Usuario";

            //Act
            var resultado = controller.Registro(empleadoExistente) as ViewResult;


            // Assert
            Assert.AreEqual(mensajeEsperado, resultado.ViewData["Mensaje"]);
        }

    }
}
