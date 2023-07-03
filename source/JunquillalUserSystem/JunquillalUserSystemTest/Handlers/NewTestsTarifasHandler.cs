using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;


namespace JunquillalUserSystemTest.Handlers
{
    [TestClass]
    public class NewTestsTarifasHandler
    {
        private TarifasHandler tarifasHandler = new TarifasHandler();

        [TestMethod]
        public void insertarTarifaNuevaNoRepetida()
        {
            TarifaModelo tarifa = new TarifaModelo();
            tarifa.Nacionalidad = "Extranjero";
            tarifa.Actividad = "Picnic";
            tarifa.Poblacion = "Niño menor de 3 años";
            tarifa.Precio = 0;

            tarifasHandler.insertarNuevaTarifa(tarifa);
            List<TarifaModelo> tarifas = new List<TarifaModelo>();
            tarifas = tarifasHandler.obtenerTarifasActuales();
            TarifaModelo tarifaIntroducida = new TarifaModelo();

            for (int i = 0; i < tarifas.Count; i++)
            {
                if (tarifas[i].Poblacion.Equals(tarifa.Poblacion))
                {
                    tarifaIntroducida = tarifas[i];
                }
            }

            Assert.AreEqual(tarifa.Nacionalidad, tarifaIntroducida.Nacionalidad);
            Assert.AreEqual(tarifa.Poblacion, tarifaIntroducida.Poblacion);
            Assert.AreEqual(tarifa.Actividad, tarifaIntroducida.Actividad);
            Assert.AreEqual(tarifa.Precio, tarifaIntroducida.Precio);

        }

        [TestMethod]
        public void insertarTarifaNuevaRepetida()
        {
            TarifaModelo tarifa = new TarifaModelo();
            tarifa.Nacionalidad = "Extranjero";
            tarifa.Actividad = "Picnic";
            tarifa.Poblacion = "Niño menor de 3 años";
            tarifa.Precio = 0;

            tarifasHandler.insertarNuevaTarifa(tarifa);
            List<TarifaModelo> tarifas = new List<TarifaModelo>();
            tarifas = tarifasHandler.obtenerTarifasActuales();
            int cuentaTarifasIguales = 0;

            for (int i = 0; i < tarifas.Count; i++)
            {
                if (tarifas[i].Poblacion.Equals(tarifa.Poblacion) &&
                    tarifas[i].Nacionalidad.Equals(tarifa.Nacionalidad) &&
                    tarifas[i].Actividad.Equals(tarifa.Actividad) &&
                    tarifas[i].Precio.Equals(tarifa.Precio))
                {
                    cuentaTarifasIguales++;
                }
            }

            Assert.AreEqual(1, cuentaTarifasIguales);

        }

        //Despues de correr las pruebas anteriores y esta
        /*
         * drop trigger eliminarTarifa;
         * delete Tarifa
           where Tarifa.Poblacion = 'Niño menor de 3 años' AND Tarifa.Actividad = 'Picnic' AND Tarifa.Nacionalidad = 'Extranjero';
            Volver a activar el trigger del archivo de procedimientos
         * */

        [TestMethod]
        public void borrarTarifaExistente()
        {
            TarifaModelo tarifa = new TarifaModelo();
            tarifa.Nacionalidad = "Extranjero";
            tarifa.Actividad = "Picnic";
            tarifa.Poblacion = "Niño menor de 3 años";
            tarifa.Precio = 0;
            tarifa.Esta_Vigente = "0";

            tarifasHandler.borrarTarifa(tarifa);
            List<TarifaModelo> tarifas = new List<TarifaModelo>();
            tarifas = tarifasHandler.obtenerTarifasActuales();
            TarifaModelo tarifaIntroducida = new TarifaModelo();

            for (int i = 0; i < tarifas.Count; i++)
            {
                if (tarifas[i].Poblacion.Equals(tarifa.Poblacion) &&
                    tarifas[i].Nacionalidad.Equals(tarifa.Nacionalidad) &&
                    tarifas[i].Actividad.Equals(tarifa.Actividad))
                {
                    tarifaIntroducida = tarifas[i];
                }
            }

            Assert.AreEqual(tarifa.Nacionalidad, tarifaIntroducida.Nacionalidad);
            Assert.AreEqual(tarifa.Poblacion, tarifaIntroducida.Poblacion);
            Assert.AreEqual(tarifa.Actividad, tarifaIntroducida.Actividad);
            Assert.AreEqual(tarifa.Precio, tarifaIntroducida.Precio);
            Assert.AreEqual(Convert.ToInt32(tarifa.Esta_Vigente), 0);
        }

        [TestMethod]
        public void insertarUnaTarifaConAlgunDatoIncompleto()
        {
            TarifaModelo tarifa = new TarifaModelo();
            tarifa.Nacionalidad = "Extranjero";
            tarifa.Actividad = "Picnic";
            tarifa.Poblacion = "";
            tarifa.Precio = 0;

            tarifasHandler.insertarNuevaTarifa(tarifa);
            List<TarifaModelo> tarifas = new List<TarifaModelo>();
            tarifas = tarifasHandler.obtenerTarifasActuales();
            int contadorTarifaConPoblacionNula = 0;
            for (int i = 0; i < tarifas.Count; i++)
            {
                if (tarifas[i].Poblacion == null)
                {
                    contadorTarifaConPoblacionNula++;
                }
            }

            Assert.AreEqual(contadorTarifaConPoblacionNula, 0);
        }
        /*
         * Update Tarifa
           SET Tarifa.Precio = 3500
           WHERE Tarifa.Poblacion = 'Adulto mayor de 50';
         */
        [TestMethod]
        public void actualizarTarifaConDatosValidos()
        {
            TarifaModelo tarifa = new TarifaModelo();
            tarifa.Nacionalidad = "Nacional";
            tarifa.Actividad = "Camping";
            tarifa.Poblacion = "Adulto mayor de 50";
            tarifa.Precio = 3000;

            tarifasHandler.actualizarPrecioTarifas(tarifa);
            List<TarifaModelo> tarifas = new List<TarifaModelo>();
            tarifas = tarifasHandler.obtenerTarifasActuales();
            TarifaModelo tarifaIntroducida = new TarifaModelo();

            for (int i = 0; i < tarifas.Count; i++)
            {
                if (tarifas[i].Poblacion.Equals(tarifa.Poblacion) &&
                    tarifas[i].Nacionalidad.Equals(tarifa.Nacionalidad) &&
                    tarifas[i].Actividad.Equals(tarifa.Actividad))
                {
                    tarifaIntroducida = tarifas[i];
                }
            }

            Assert.AreEqual(tarifa.Nacionalidad, tarifaIntroducida.Nacionalidad);
            Assert.AreEqual(tarifa.Poblacion, tarifaIntroducida.Poblacion);
            Assert.AreEqual(tarifa.Actividad, tarifaIntroducida.Actividad);
            Assert.AreEqual(tarifa.Precio, tarifaIntroducida.Precio);
        }
    }
}
