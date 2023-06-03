using Microsoft.VisualStudio.TestTools.UnitTesting;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;

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