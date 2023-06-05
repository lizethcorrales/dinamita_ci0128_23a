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

/*Hechas por Sabrina Brenes Hernandez C01309*/

namespace JunquillalUserSystemTest.Handlers
{
    [TestClass]
    public class TestReservacionesHandlerBase
    {
        private ReservacionesHandlerBase handlerSiendoProbado = new ReservacionesHandlerBase();
        private CampingHandler campingHandlerEnPrueba = new CampingHandler();
        private HandlerPruebas handlerPruebas = new HandlerPruebas();
        public string identificadorReservacion = "";

        //No necesita borrar nada 
        [TestMethod]
        public void costoTotalDaCorrectamente_AlIntroducirUnIdentificadorDeReservaExistente()
        {
            string identificadorReserva = "SeUmptLFLY";
            double expectedResult = 12430;

            double actualResult = handlerSiendoProbado.CostoTotal(identificadorReserva);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //No necesita borrar nada 
        [TestMethod]
        public void costoTotalDevuelve0_AlMeterUnaReservaQueFueCancelada()
        {
            string identificadorReserva = "ktl0Curkfi";
            double expectedResult = 0;

            double actualResult = handlerSiendoProbado.CostoTotal(identificadorReserva);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //No necesita borrar nada 
        [TestMethod]
        public void costoTotalDevuelve0_AlMeterUnaReservaQueNoExiste()
        {
            string identificadorReserva = "khlkyop67h";
            double expectedResult = 0;

            double actualResult = handlerSiendoProbado.CostoTotal(identificadorReserva);

            Assert.AreEqual(expectedResult, actualResult);
        }

        //Ver sql de procedimientos al fondo
        /*
         delete Hospedero
          where Hospedero.Identificacion = '308970567';
         */
        [TestMethod]
        public void insertarNuevoHospederoABaseDeDatos()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Nombre = "Gustavo";
            hospedero.Apellido1 = "Lopez";
            hospedero.Apellido2 = "Herrera";
            hospedero.Identificacion = "308970567";
            hospedero.Telefono = "76569000";
            hospedero.Email = "LopezH@gmail.com";

            handlerSiendoProbado.insertarHospedero(hospedero);
            List<HospederoModelo> hospederoDeLaBaseDeDatos = handlerPruebas.obtenerHospedero(hospedero.Identificacion);
            

            Assert.AreEqual(hospedero.Identificacion, hospederoDeLaBaseDeDatos[0].Identificacion.Trim());
            Assert.AreEqual(hospedero.Nombre, hospederoDeLaBaseDeDatos[0].Nombre.Trim());
            Assert.AreEqual(hospedero.Apellido1, hospederoDeLaBaseDeDatos[0].Apellido1.Trim());
            Assert.AreEqual(hospedero.Apellido2, hospederoDeLaBaseDeDatos[0].Apellido2.Trim());
            Assert.AreEqual(hospedero.Telefono, hospederoDeLaBaseDeDatos[0].Telefono.Trim());
            Assert.AreEqual(hospedero.Email, hospederoDeLaBaseDeDatos[0].Email.Trim());
        }

        //Ver sql de procedimientos al fondo
        /*
         delete Hospedero
          where Hospedero.Identificacion = '308970567';
         */
        [TestMethod]
        public void insertarHospederoRepetidoEnBaseDeDatos_DeberiaConservarDatosOriginalesNoRepiteTupla()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Nombre = "Gustavo";
            hospedero.Apellido1 = "Lopez";
            hospedero.Apellido2 = "Herrera";
            hospedero.Identificacion = "308970567";
            hospedero.Telefono = "76569000";
            hospedero.Email = "LopezH@gmail.com";
            int expectedResult = 1;

            handlerSiendoProbado.insertarHospedero(hospedero);
            List<HospederoModelo> hospederoDeLaBaseDeDatos = handlerPruebas.obtenerHospedero(hospedero.Identificacion);

            Assert.AreEqual(expectedResult, hospederoDeLaBaseDeDatos.Count());
            
        }

        //Busquen la reservacion con el identificador que se genera y lo borran 
        /*
         delete Reservacion
         where Reservacion.IdentificadorReserva = 'identificador';
         */
        [TestMethod]
        public void insertarNuevaReservacionCampingEnBaseDeDatos()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Motivo = "Ocio";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = handlerSiendoProbado.crearIdentificador(10);
            this.identificadorReservacion = reservacion.Identificador;
            reservacion.PrimerDia = "2023-07-12";
            reservacion.UltimoDia = "2023-07-15";
            reservacion.cantTipoPersona.Add(2);
            reservacion.cantTipoPersona.Add(2);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.TipoActividad = "Camping";
            string PrimerDiaExpected = "7/12/2023";
            string UltimoDiaExpected = "7/15/2023";

            campingHandlerEnPrueba.insertarReserva(reservacion, hospedero, "0");
            List<ReservacionModeloPruebas> reservacionDelaBaseDeDatos = handlerPruebas.obtenerReservacion(reservacion.Identificador);

            Assert.AreEqual(reservacion.Identificador, reservacionDelaBaseDeDatos[0].Identificador.Trim());
            Assert.AreEqual(PrimerDiaExpected, reservacionDelaBaseDeDatos[0].PrimerDia.Trim().Substring(0, 9));
            Assert.AreEqual(UltimoDiaExpected, reservacionDelaBaseDeDatos[0].UltimoDia.Trim().Substring(0, 9));
            Assert.AreEqual(reservacion.TipoActividad, reservacionDelaBaseDeDatos[0].TipoActividad.Trim());
            Assert.AreEqual(hospedero.Motivo, reservacionDelaBaseDeDatos[0].Motivo.Trim());
            Assert.AreEqual(4, reservacionDelaBaseDeDatos[0].CantidadTotal);
        }

        //Ver sql de procedimientos al fondo
        /*
         delete TieneNacionalidad 
          where TieneNacionalidad.IdentificadorReserva = '3463933048' AND TieneNacionalidad.NombrePais= 'Francia';

        Delete Pais 
         where Pais.Nombre = 'Francia';
         */
        [TestMethod]
        public void insertarNacionalidadNueva()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Nacionalidad = "Francia";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = "3463933048";
            reservacion.cantTipoPersona.Add(20);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);

            handlerSiendoProbado.insertarTieneNacionalidad(hospedero,reservacion);
            List<NacionalidadPruebas> nacionalidadDeLaReserva = handlerPruebas.obtenerNacionalidad("3463933048");

            Assert.AreEqual(reservacion.Identificador, nacionalidadDeLaReserva[0].Identificador.Trim());
            Assert.AreEqual(hospedero.Nacionalidad, nacionalidadDeLaReserva[0].NombrePais.Trim());
            Assert.AreEqual(20, nacionalidadDeLaReserva[0].CantidadTotal);
        }

        //Ver sql de procedimientos al fondo
        /*
         TieneNacionalidad 
          where TieneNacionalidad.IdentificadorReserva = '2595141556' AND TieneNacionalidad.NombrePais= 'Estados Unidos'
         */
        [TestMeth        public void insertarNacionalidadExistente()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Nacionalidad = "Estados Unidos";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = "2595141556";
            reservacion.cantTipoPersona.Add(2);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);

            handlerSiendoProbado.insertarTieneNacionalidad(hospedero, reservacion);
            List<NacionalidadPruebas> nacionalidadDeLaReserva = handlerPruebas.obtenerNacionalidad("2595141556");

            int estados = 0;
            for (int i = 0; i < nacionalidadDeLaReserva.Count; i++)
            {
                if (nacionalidadDeLaReserva[i].NombrePais.Trim() == "Estados Unidos")
                {
                    estados++;
                }
            }

            Assert.AreEqual(reservacion.Identificador, nacionalidadDeLaReserva[1].Identificador.Trim());
            Assert.AreEqual(hospedero.Nacionalidad, nacionalidadDeLaReserva[1].NombrePais.Trim());
            Assert.AreEqual(2, nacionalidadDeLaReserva[1].CantidadTotal);
        }

        //Ver sql de procedimientos al fondo
        /*
         delete TieneNacionalidad 
        where TieneNacionalidad.IdentificadorReserva = '2595141556' AND TieneNacionalidad.NombrePais= 'Alemania'
         */
        [TestMethod]
        public void insertarDosVecesLaMismaTuplaDeNacionalidad_DeberiaMeterSoloUna()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Nacionalidad = "Alemania";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = "2595141556";
            reservacion.cantTipoPersona.Add(3);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);

            handlerSiendoProbado.insertarTieneNacionalidad(hospedero, reservacion);
            handlerSiendoProbado.insertarTieneNacionalidad(hospedero, reservacion);
            List<NacionalidadPruebas> nacionalidadDeLaReserva = handlerPruebas.obtenerNacionalidad("2595141556");

            int tuplasConEsePais = 0;
            for (int i =0; i < nacionalidadDeLaReserva.Count; i++)
            {
                if (nacionalidadDeLaReserva[i].NombrePais.Trim() == "Alemania")
                {
                    tuplasConEsePais++;
                }
            }

            Assert.AreEqual(1, tuplasConEsePais);

        }

        //Ver sql de procedimientos al fondo
        /*
            delete ProvinciaReserva
            where ProvinciaReserva.IdentificadorReserva = '8865933684';
         */
        [TestMethod]
        public void insertarProvinciaAUnaReservaPrueba()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Provincia = "Alajuela";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = "8865933684";
            reservacion.cantTipoPersona.Add(3);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);

            handlerSiendoProbado.insertarProvincia(reservacion, hospedero);
            List<ProvinciaPruebas>provinciaDeLaReserva = handlerPruebas.obtenerProvincia("8865933684");

            Assert.AreEqual(reservacion.Identificador, provinciaDeLaReserva[0].Identificador.Trim());
            Assert.AreEqual(hospedero.Provincia, provinciaDeLaReserva[0].NombreProvincia.Trim());
            Assert.AreEqual(3, provinciaDeLaReserva[0].CantidadTotal);
        }

        //Ver sql de procedimientos al fondo
        /*
            delete ProvinciaReserva
            where ProvinciaReserva.IdentificadorReserva = '8865933684';
         */
        [TestMethod]
        public void insertarDosVecesLaMismaProvinciaAUnaReservaPrueba()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Provincia = "Guanacaste";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = "8865933684";
            reservacion.cantTipoPersona.Add(3);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);

            handlerSiendoProbado.insertarProvincia(reservacion, hospedero);
            handlerSiendoProbado.insertarProvincia(reservacion, hospedero);
            List<ProvinciaPruebas> provinciaDeLaReserva = handlerPruebas.obtenerProvincia("8865933684");


            int tuplasConEsaProvincia = 0;
            for (int i = 0; i < provinciaDeLaReserva.Count; i++)
            {
                if (provinciaDeLaReserva[i].NombreProvincia.Trim() == "Guanacaste")
                {
                    tuplasConEsaProvincia++;
                }
            }

            Assert.AreEqual(1, tuplasConEsaProvincia);
        }

        //No necesita borrar nada 
        [TestMethod]
        public void insertarProvinciaQueNoExiste()
        {
            HospederoModelo hospedero = new HospederoModelo();
            hospedero.Provincia = "Coronado";
            ReservacionModelo reservacion = new ReservacionModelo();
            reservacion.Identificador = "8865933684";
            reservacion.cantTipoPersona.Add(3);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);
            reservacion.cantTipoPersona.Add(0);

            handlerSiendoProbado.insertarProvincia(reservacion, hospedero);
            List<ProvinciaPruebas> provinciaDeLaReserva = handlerPruebas.obtenerProvincia("8865933684");

            int tuplasConEsaProvincia = 0;
            for (int i = 0; i < provinciaDeLaReserva.Count; i++)
            {
                if (provinciaDeLaReserva[i].NombreProvincia.Trim() == "Coronado")
                {
                    tuplasConEsaProvincia++;
                }
            }
            
            Assert.AreEqual(0, tuplasConEsaProvincia);
        }


        //Ver sql de procedimientos al fondo
        /*
            delete Pago
            where Pago.Comprobante = 'comprobante';
         */
        [TestMethod]
        public void insertaPagoNuevo()
        {
            PagoPruebas pagoNuevo = new PagoPruebas();
            pagoNuevo.Comprobante = handlerSiendoProbado.crearIdentificador(6);
            pagoNuevo.FechaPago = "2023-07-30";
            string expectedFecha = "7/30/2023";

            handlerSiendoProbado.insertarPago(pagoNuevo.Comprobante, pagoNuevo.FechaPago);
            List<PagoPruebas> pagosDeLaBaseDeDatos = handlerPruebas.obtenerPago(pagoNuevo.Comprobante);

            Assert.AreEqual(pagoNuevo.Comprobante, pagosDeLaBaseDeDatos[0].Comprobante.Trim());
            Assert.AreEqual(expectedFecha, pagosDeLaBaseDeDatos[0].FechaPago.Trim().Substring(0, 9));
        }

        //Ver sql de procedimientos al fondo
        /*
            delete Pago
            where Pago.Comprobante = 'comprobante';
         */
        [TestMethod]
        public void insertaPagoRepetido_NoDeberiaInsertarDosTuplasIguales()
        {
            PagoPruebas pagoNuevo = new PagoPruebas();
            pagoNuevo.Comprobante = handlerSiendoProbado.crearIdentificador(6);
            pagoNuevo.FechaPago = "2023-07-29";
            string expectedFecha = "7/29/2023";

            handlerSiendoProbado.insertarPago(pagoNuevo.Comprobante, pagoNuevo.FechaPago);
            handlerSiendoProbado.insertarPago(pagoNuevo.Comprobante, pagoNuevo.FechaPago);
            List<PagoPruebas> pagosDeLaBaseDeDatos = handlerPruebas.obtenerPago(pagoNuevo.Comprobante);

            int tuplasConEseComprobante = 0;
            for (int i = 0; i < pagosDeLaBaseDeDatos.Count; i++)
            {
                if (pagosDeLaBaseDeDatos[i].Comprobante.Trim() == pagoNuevo.Comprobante)
                {
                    tuplasConEseComprobante++;
                }
            }
            Assert.AreEqual(1, tuplasConEseComprobante);

        }

        //No necesita borrar nada
        [TestMethod]
        public void obtenerDesgloseDeUnaReserva()
        {
            PrecioReservacionDesglose primeraPoblacion = new PrecioReservacionDesglose();
            PrecioReservacionDesglose segundaPoblacion = new PrecioReservacionDesglose();
            primeraPoblacion.IdentificadorReserva = "SeUmptLFLY";
            primeraPoblacion.Nacionalidad = "Nacional";
            primeraPoblacion.Poblacion = "Adulto";
            primeraPoblacion.Actividad = "Camping";
            primeraPoblacion.Cantidad = 2;
            primeraPoblacion.PrecioAlHacerReserva = 4520;

            segundaPoblacion.IdentificadorReserva = "SeUmptLFLY";
            segundaPoblacion.Nacionalidad = "Nacional";
            segundaPoblacion.Poblacion = "Niño";
            segundaPoblacion.Actividad = "Camping";
            segundaPoblacion.Cantidad = 1;
            segundaPoblacion.PrecioAlHacerReserva = 3390;

            List<PrecioReservacionDesglose> desgloseDeBaseDeDatos = handlerSiendoProbado.obtenerDesgloseReservaciones(primeraPoblacion.IdentificadorReserva);

            Assert.AreEqual(primeraPoblacion.IdentificadorReserva, desgloseDeBaseDeDatos[0].IdentificadorReserva.Trim());
            Assert.AreEqual(primeraPoblacion.Nacionalidad, desgloseDeBaseDeDatos[0].Nacionalidad.Trim());
            Assert.AreEqual(primeraPoblacion.Poblacion, desgloseDeBaseDeDatos[0].Poblacion.Trim());
            Assert.AreEqual(primeraPoblacion.Actividad, desgloseDeBaseDeDatos[0].Actividad.Trim());
            Assert.AreEqual(primeraPoblacion.Cantidad, desgloseDeBaseDeDatos[0].Cantidad);
            Assert.AreEqual(primeraPoblacion.PrecioAlHacerReserva, desgloseDeBaseDeDatos[0].PrecioAlHacerReserva);

            Assert.AreEqual(segundaPoblacion.IdentificadorReserva, desgloseDeBaseDeDatos[1].IdentificadorReserva.Trim());
            Assert.AreEqual(segundaPoblacion.Nacionalidad, desgloseDeBaseDeDatos[1].Nacionalidad.Trim());
            Assert.AreEqual(segundaPoblacion.Poblacion, desgloseDeBaseDeDatos[1].Poblacion.Trim());
            Assert.AreEqual(segundaPoblacion.Actividad, desgloseDeBaseDeDatos[1].Actividad.Trim());
            Assert.AreEqual(segundaPoblacion.Cantidad, desgloseDeBaseDeDatos[1].Cantidad);
            Assert.AreEqual(segundaPoblacion.PrecioAlHacerReserva, desgloseDeBaseDeDatos[1].PrecioAlHacerReserva);
        }

    }
}
