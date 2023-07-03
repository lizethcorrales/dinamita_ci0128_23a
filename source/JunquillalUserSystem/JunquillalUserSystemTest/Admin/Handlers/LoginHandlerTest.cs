using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;

namespace JunquillalUserSystemTest.Admin.Handlers
{
    // Pruebas Unitarias de Gabriel Chacón
    [TestClass]
    public class LoginHandlerTest
    {
        //En esta clase se verifica que el metodo de este handler
        //funcione correctamente
        [TestMethod]
        public void crearCorrectamenteHandler()
        {
            // Arrange
            LoginHandler handler = new LoginHandler();

            //Act && Assert
            Assert.IsNotNull(handler.Conexion);
        }


        [TestMethod]
        public void CredencialesObtenidasConNull()
        {
            // Arrange
            LoginHandler handler = new LoginHandler();
            string idNull = null;

            // Act
            TrabajadorModelo resultado = handler.ObtenerCredencialesTrabajador(idNull);

            // Assert
            Assert.IsNull(resultado);
        }

        [TestMethod]
        public void CredencialesObtenidasCorrectamente()
        {
            // Arrange
            LoginHandler handler = new LoginHandler();
            string empleado = "211118888";

            // Act
            TrabajadorModelo resultado = handler.ObtenerCredencialesTrabajador(empleado);

            // Assert
            Assert.IsNotNull(resultado);

            TrabajadorModelo empleadoEsperado = new TrabajadorModelo();
            empleadoEsperado.Nombre = "Dom"; empleadoEsperado.Apellido1 = "Dima"; empleadoEsperado.Puesto = "Administrador";
            empleadoEsperado.Contrasena = "590887434DD1A16ECCD60E67C076D85A07F13C91849655278A8282BBA74DBD8E";
            empleadoEsperado.Sal = "0513112023";

            Assert.AreEqual(empleadoEsperado.Nombre, resultado.Nombre);
            Assert.AreEqual(empleadoEsperado.Apellido1, resultado.Apellido1);
            Assert.AreEqual(empleadoEsperado.Puesto, resultado.Puesto);
            Assert.AreEqual(empleadoEsperado.Contrasena, resultado.Contrasena);
            Assert.AreEqual(empleadoEsperado.Sal, resultado.Sal);
        }

        [TestMethod]
        public void CredencialesObtenidasIDVacia()
        {
            // Arrange
            LoginHandler handler = new LoginHandler();
            string empleado = "";

            // Act
            TrabajadorModelo resultado = handler.ObtenerCredencialesTrabajador(empleado);

            // Assert
            Assert.IsNotNull(resultado);

            TrabajadorModelo empleadoEsperado = new TrabajadorModelo();

            Assert.AreEqual(empleadoEsperado.Nombre, resultado.Nombre);
            Assert.AreEqual(empleadoEsperado.Apellido1, resultado.Apellido1);
            Assert.AreEqual(empleadoEsperado.Puesto, resultado.Puesto);
            Assert.AreEqual(empleadoEsperado.Contrasena, resultado.Contrasena);
            Assert.AreEqual(empleadoEsperado.Sal, resultado.Sal);

        }

        [TestMethod]
        public void CredencialesDeEmpleadoInexistente()
        {
            // Arrange
            LoginHandler handler = new LoginHandler();
            string empleado = "233331212";

            // Act
            TrabajadorModelo resultado = handler.ObtenerCredencialesTrabajador(empleado);

            // Assert
            Assert.IsNotNull(resultado);

            TrabajadorModelo empleadoEsperado = new TrabajadorModelo();

            Assert.AreEqual(empleadoEsperado.Nombre, resultado.Nombre);
            Assert.AreEqual(empleadoEsperado.Apellido1, resultado.Apellido1);
            Assert.AreEqual(empleadoEsperado.Puesto, resultado.Puesto);
            Assert.AreEqual(empleadoEsperado.Contrasena, resultado.Contrasena);
            Assert.AreEqual(empleadoEsperado.Sal, resultado.Sal);

        }


    }
}
