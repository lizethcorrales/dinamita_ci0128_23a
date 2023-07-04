
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using JunquillalUserSystem.Areas.Admin.Views.CambioDolar;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JunquillalUserSystem.Tests.Areas.Admin.Controllers
{
    [TestClass]
    public class CambioDolarControllerTests
    {
        [TestMethod]
        public void CambioDolar_ReturnsViewResult()
        {
            // Arrange
            var controller = new CambioDolarController();

            // Act
            var result = controller.CambioDolar();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditarCambioDolar_Get_ReturnsViewResult()
        {
            // Arrange
            var controller = new CambioDolarController();

            // Act
            var result = controller.EditarCambioDolar(valorDolar: null);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void EditarCambioDolar_Post_WithValidModel_ReturnsRedirectToActionResult()
        {
            // Arrange
            var controller = new CambioDolarController();
            var cambioDolar = new CambioDolarModel
            {
                // Configurar propiedades del modelo según lo necesario para la prueba
            };

            // Act
            var result = controller.EditarCambioDolar(cambioDolar);

            // Assert
            Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            var redirectResult = (RedirectToActionResult)result;
            Assert.AreEqual("CambioDolar", redirectResult.ActionName);
            Assert.AreEqual("CambioDolar", redirectResult.ControllerName);
        }
    }
}
