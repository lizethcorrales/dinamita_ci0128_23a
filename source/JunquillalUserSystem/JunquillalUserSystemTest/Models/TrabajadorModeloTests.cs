using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Policy;

namespace JunquillalUserSystemTest.Models
{
    // Pruebas Unitarias de Gabriel Chacón
    [TestClass]
    public class TrabajadorModeloTests
    {
        //En esta clase se verifica que los metodos de este modelo
        //funcionen correctamente
        [TestMethod]
        public void crearSalCorrectamente()
        {
            // Arrange
            TrabajadorModelo empleado = new TrabajadorModelo();
            string diaSal = DateTime.Now.ToString("MM-dd-yyyy");
            string salEsperada = diaSal.Replace('-', '1');

            // Act
            var resultado = empleado.crearSal();

            // Assert
            Assert.AreEqual(salEsperada, resultado);
        }

        [TestMethod]
        public void HashearContrasenaVacia()
        {
            // Arrange
            TrabajadorModelo empleado = new TrabajadorModelo();
            string contrasenaHashear = "";

            // Act and assert
            Assert.ThrowsException<System.ArgumentOutOfRangeException>(() => empleado.HashearContrasena(contrasenaHashear));
        }

        [TestMethod]
        public void HashearContrasenaNula()
        {
            // Arrange
            TrabajadorModelo empleado = new TrabajadorModelo();
            string contrasenaHashear = null;
            string hashEsperado = "0";

            //Act
            var hash = empleado.HashearContrasena(contrasenaHashear);

            // Assert
            Assert.AreEqual(hashEsperado, hash);
        }

        [TestMethod]
        public void HashearContrasenaCorrectamente()
        {
            // Arrange
            TrabajadorModelo empleado = new TrabajadorModelo();
            string contrasena = "230610512023";
            string hashEsperado = "4B43C2036CD31E8D5ABC97625513278A3F90EADEF12D5277261C7B5110457E52";

            // Act
            string hash = empleado.HashearContrasena(contrasena);
            // Assert
            Assert.AreEqual(hashEsperado, hash);
        }
  
    }
}
