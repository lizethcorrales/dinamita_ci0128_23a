using Microsoft.VisualStudio.TestTools.UnitTesting;
using JunquillalUserSystem.Controllers;
using JunquillalUserSystem.Models;

namespace JunquillalUserSystemTest
{
    [TestClass]
    public class TestModelos
    {
        [TestMethod]
        public void MetodosGeneralesModelcrearIdentificadorNegativo()
        {
            //Arrange
            MetodosGeneralesModel metodosGeneralesModel = new MetodosGeneralesModel();
            int valorNegativo = -1;

            //Act & Assert
            Assert.ThrowsException<OverflowException>(() => metodosGeneralesModel.crearIdentificador(valorNegativo));
        }
    }
}