/*
 * Pruebas Unitarias Esteban Mora
 * 
 */

using JunquillalUserSystem.Areas.Admin.Controllers;
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

namespace JunquillalUserSystemTest.Controllers
{
    [TestClass]
    public class AdministrarReservasControllerTest
    {
        /*
         * Este método de prueba verifica que el modelo
         * que retorna el controlador no esa nulo
         */
        [TestMethod]
        public void Reservas_VerificarModelo()
        {
            AdministrarReservasController adminControler = new AdministrarReservasController();
            var resultado = adminControler.Reservas();
            Assert.IsNotNull(resultado);
        }

        /*
         * Este método de prueba verifica que el viewresult
         * pertenezca al AspNetCore con el patrón MCV
         */
        [TestMethod]
        public void Reservas_VerificarViewResult()
        {
            AdministrarReservasController adminControler = new AdministrarReservasController();
            var resultado = adminControler.Reservas();
            Assert.IsInstanceOfType(resultado,typeof(Microsoft.AspNetCore.Mvc.ViewResult));
        }

        /*
         * Este método de prueba verifica que el modelo
         * que retorna el controlador con identificador específico
         * no esa nulo
         */
        [TestMethod]
        public void ReservasPorIdentificador_VerificarModeloPorIdentificador()
        {
            AdministrarReservasController adminControler = new AdministrarReservasController();
            var resultado = adminControler.ReservasPorIdentificador("6pw5R8YhzY");
            Assert.IsNotNull(resultado);
        }

        /*
         * Este método de prueba verifica que el modelo
         * que retorna el controlador con fecha específica
         * no esa nulo
         */
        [TestMethod]
        public void ReservasPorFecha_VerificarModeloPorFecha()
        {
            AdministrarReservasController adminControler = new AdministrarReservasController();
            var resultado = adminControler.ReservasPorIdentificador("2022-06-05");
            Assert.IsNotNull(resultado);
        }

        /*
         * Este método de prueba verifica que el modelo
         * que retorna el controlador con fecha específica
         * no esa nulo
         */
        [TestMethod]
        public void EliminarReserva_VerificarEliminarReservaExistente()
        {
            AdministrarReservasController adminControler = new AdministrarReservasController();
            RedirectToActionResult resultado = (RedirectToActionResult)adminControler.EliminarReserva("6pw5R8YhzY");
            Assert.IsNotNull(resultado);
        }

    }
}
