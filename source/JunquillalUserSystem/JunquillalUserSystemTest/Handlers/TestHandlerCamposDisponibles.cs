using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using static JunquillalUserSystemTest.CalculadorFechas;
using System.Diagnostics;

namespace JunquillalUserSystemTest.Handlers
{
    [TestClass]
    public class TestHandlerCamposDisponibles
    {
        [TestMethod]
        public void FechaConCeroReservas()
        {
            // Arrange
            HandlerCamposDisponibles handlerCamposDisponibles = new HandlerCamposDisponibles();
            string fecha = "2002-06-25";
            int resultadoEsperado = 0;

            // Act
            var resultado = handlerCamposDisponibles.ReservasTotal(fecha);

            // Assert
            Assert.IsNotNull(resultado);
            Assert.AreEqual(resultado, resultadoEsperado);
        }

        [TestMethod]
        public void FechaRegistradaEnBDPeroNoDisponibleParaReservar()
        {
            // Arrange
            HandlerCamposDisponibles handlerCamposDisponibles = new HandlerCamposDisponibles();
            string fecha = "2023-05-12";
            int resultadoEsperado = 19;

            // Act
            int resultado = handlerCamposDisponibles.ReservasTotal(fecha);

            // Assert
            Assert.AreEqual(resultadoEsperado, resultado);
        }

        [TestMethod]
        public void BuscarReservasEnRangoFechas()
        {
            // Arrange
            HandlerCamposDisponibles handlerCamposDisponibles = new HandlerCamposDisponibles();
            bool bisiesto = false;
            int maxDias = 0;

            // Act
            for (int anio = 2020; anio < 2023; ++anio)
            {
                bisiesto = anioBisiesto(anio);
                for (int mes = 1; mes <= 12; ++mes)
                {
                    maxDias = obtenerDiasDeMes(mes, anio);
                    for (int dia = 1; dia < maxDias + 1; ++dia)
                    {
                        DateOnly fecha = new(anio, mes, dia);
                        int resultado = handlerCamposDisponibles.ReservasTotal(fecha.ToString("yyyy-MM-dd"));
                        // Assert
                        Assert.IsNotNull(resultado);
                    }
                }
            }
        }

        [TestMethod]
        public void MetodoLlenarFechaHandlerCamposDisponibles()
        {
            //Arrange
            HandlerCamposDisponibles handlerCamposDisponibles = new();
            CamposDisponiblesModel modeloEsperado = new();
            CamposDisponiblesModel modeloAPrueba = null;
            string fechaDePrueba = "2023-06-02";
            modeloEsperado.fecha = fechaDePrueba;
            var formEjemploDatos = new Dictionary<string, StringValues>
            {
                { "fecha-entrada", new StringValues(fechaDePrueba) }
            };
            var formEjemplo = new FormCollection(formEjemploDatos);

            //Act
            modeloAPrueba = handlerCamposDisponibles.LlenarFecha(formEjemplo);

            // Assert
            Assert.IsNotNull(modeloAPrueba);
            Assert.AreEqual(modeloEsperado.fecha, modeloAPrueba.fecha);
        }
    }
}