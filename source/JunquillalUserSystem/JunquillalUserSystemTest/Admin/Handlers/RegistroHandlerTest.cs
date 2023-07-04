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
    public class RegistroHandlerTest
    {
        //En esta clase se verifica que el metodo de este handler
        //funcione correctamente
        [TestMethod]
        public void buscarEmpleadoExistente()
        {
            // Arrange
            RegistroHandler handler = new RegistroHandler();

            //Act
            var resultado = handler.existeEmpleado("211118888");

            //Assert
            Assert.AreEqual(1, resultado);
        }


        [TestMethod]
        public void buscarEmpleadoNOExistente()
        {
            // Arrange
            RegistroHandler handler = new RegistroHandler();
            string idAuxiliar = DateTime.Now.ToString("h:mm:ss");
            string idNueva = idAuxiliar.Replace(':', '1');
            idNueva = "65" + idNueva;

            //Act
            var resultado = handler.existeEmpleado(idNueva);

            //Assert
            Assert.AreEqual(0, resultado);
        }

        [TestMethod]
        public void buscarEmpleadoNull()
        {
            // Arrange
            RegistroHandler handler = new RegistroHandler();
            string empleadoNull = null;

            //Act
            var resultado = handler.existeEmpleado(empleadoNull);

            //Assert
            Assert.AreEqual(2, resultado);
        }

        [TestMethod]
        public void registrarEmpleadoNuevoNull()
        {
            // Arrange
            RegistroHandler handler = new RegistroHandler();
            TrabajadorModelo empleadoNull = null;

            //Act
            var resultado = handler.registrarEmpleadoNuevo(empleadoNull);

            //Assert
            Assert.AreEqual(2, resultado);
        }

        [TestMethod]
        public void registrarEmpleadoNuevoConValoresInvalidos()
        {
            // Arrange
            RegistroHandler handler = new RegistroHandler();
            TrabajadorModelo empleadoNull = new TrabajadorModelo();
            empleadoNull.Nombre = null;

            //Act
            var resultado = handler.registrarEmpleadoNuevo(empleadoNull);

            //Assert
            Assert.AreEqual(0, resultado);
        }


    }
}
