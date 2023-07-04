using JunquillalUserSystem.Controllers;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace JunquillalUserSystem.Tests.Controllers
{
    [TestClass]
    public class TarifasControllerTests
    {
        [TestMethod]
        public void AgregarTarifa_ReturnsViewResult()
        {
            // Arrange
            var mockHandler = new Mock<TarifasHandler>();
            var controller = new TarifasController(mockHandler.Object);
            var tarifa = new TarifaModelo();

            // Act
            var result = controller.AgregarTarifa(tarifa);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditarTarifa_ReturnsRedirectToActionResult()
        {
            // Arrange
            var mockHandler = new Mock<TarifasHandler>();
            var controller = new TarifasController(mockHandler.Object);
            var tarifa = new TarifaModelo();

            // Act
            var result = controller.EditarTarifa(tarifa);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
        }
    }
}
