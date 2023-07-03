/*
 * Pruebas Unitarias Andres Matarrita Miranda C04668
 */



using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using JunquillalUserSystem.Areas.Admin.Controllers;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using System.Collections.Generic;

namespace JunquillalUserSystem.Tests
{
    [TestClass]
    public class AdministrarReservasControllerTests
    {
        [TestMethod]
        public void Reservas_ReturnsViewWithEmptyList()
        {
            // Arrange
            var controller = new AdministrarReservasController();

            // Act
            var result = controller.Reservas() as ViewResult;
            var model = result.Model as List<ReservacionModelo>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(null, result.ViewName);
            Assert.IsNotNull(model);
            Assert.AreEqual(0, model.Count);
        }

        [TestMethod]
        public void ReservasPorFecha_ReturnsViewWithReservasForGivenFecha()
        {
            // Arrange
            var controller = new AdministrarReservasController();
            var fecha = "2023-07-03";

            // Act
            var result = controller.ReservasPorFecha(fecha) as ViewResult;
            var model = result.Model as List<ReservacionModelo>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Reservas", result.ViewName);
            Assert.IsNotNull(model);
      
        }

        [TestMethod]
        public void ReservasPorIdentificador_ReturnsViewWithReservasForGivenIdentificador()
        {
            // Arrange
            var controller = new AdministrarReservasController();
            var identificador = "12345";

            // Act
            var result = controller.ReservasPorIdentificador(identificador) as ViewResult;
            var model = result.Model as List<ReservacionModelo>;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Reservas", result.ViewName);
            Assert.IsNotNull(model);
  
        }

        [TestMethod]
        public void EliminarReserva_RedirectsToReservas()
        {
            // Arrange
            var controller = new AdministrarReservasController();
            var identificador = "12345";

            // Act
            var result = controller.EliminarReserva(identificador) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Reservas", result.ActionName);
        }

        [TestMethod]
        public void CheckOutReserva_RedirectsToReservas()
        {
            // Arrange
            var controller = new AdministrarReservasController();
            var identificador = "12345";

            // Act
            var result = controller.CheckOutReserva(identificador) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Reservas", result.ActionName);
        }
    }
}
