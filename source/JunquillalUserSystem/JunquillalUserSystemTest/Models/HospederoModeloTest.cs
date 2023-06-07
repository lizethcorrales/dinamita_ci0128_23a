/*
 * Pruebas Unitarias Esteban Mora
 * 
 */

using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JunquillalUserSystemTest.Models
{
    [TestClass]
    public class HospederoModeloTest
    {
        /*
         * Este método de prueba verifica que el nombre
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElNombre()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "nombre", new StringValues("Carlos") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Carlos", result.Nombre);
        }

        /*
         * Este método de prueba verifica que el primer apellido
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElPrimerApellido()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "primerApellido", new StringValues("Castro") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Castro", result.Apellido1);
        }

        /*
         * Este método de prueba verifica que el segundo apellido
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElSegundoApellido()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "segundoApellido", new StringValues("Barquero") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Barquero", result.Apellido2);
        }

        /*
         * Este método de prueba verifica que la identificación
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteLaIdentificacion()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "identificacion", new StringValues("12345678") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("12345678", result.Identificacion);
        }

        /*
         * Este método de prueba verifica que el teléfono
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElTelefono() 
        { 
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "telefono", new StringValues("88828767") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("88828767", result.Telefono);
        }

        /*
         * Este método de prueba verifica que el email
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElEmail()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "email", new StringValues("test@gmail.com") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("test@gmail.com", result.Email);
        }

        /*
         * Este método de prueba verifica que el tipo de identificación
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteLaNacionalidad()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "nacionalidad", new StringValues("Extranjero") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Extranjero", result.TipoIdentificacion);
        }

        /*
         * Este método de prueba verifica que el país
         * del hospedero se guarde correctamente
         */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElPaís()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "pais", new StringValues("Costa Rica") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Costa Rica", result.Nacionalidad);
        }

        /*
        * Este método de prueba verifica que el motivo
        * del hospedero se guarde correctamente
        */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteElMotivo()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "motivo", new StringValues("Ocio") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Ocio", result.Motivo);
        }

        /*
        * Este método de prueba verifica que la provincia
        * del hospedero se guarde correctamente
        */
        [TestMethod]
        public void LlenarHospedero_DebeAsignarCorrectamenteLaProvincia()
        {
            // Arrange
            var form = new FormCollection(new Dictionary<string, StringValues>
            {
               { "provincia", new StringValues("Guanacaste") },
            });

            var hospedero = new HospederoModelo();

            // Act
            var result = hospedero.LlenarHospedero(form);

            // Assert
            Assert.AreEqual("Guanacaste", result.Provincia);
        }
    }
}
