﻿using System;
using System.Collections.Generic;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Builder;




namespace JunquillalUserSystem.Handlers
{
    public class ReservacionHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
        private static readonly Random _random = new Random();
        public ReservacionHandler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("ContextoJunquillal");
            conexion = new SqlConnection(rutaConexion);
        }
        private DataTable CrearTablaConsulta(string consulta)
        {
            SqlCommand comandoParaConsulta = new SqlCommand(consulta,
            conexion);
            SqlDataAdapter adaptadorParaTabla = new
            SqlDataAdapter(comandoParaConsulta);
            DataTable consultaFormatoTabla = new DataTable();
            conexion.Open();
            adaptadorParaTabla.Fill(consultaFormatoTabla);
            conexion.Close();
            return consultaFormatoTabla;
        }
        public double CostoTotal(string identificadorReserva)
        {
            //PROCEDIMIENTO

            string consulta = "calcularCostoTotalReserva";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", identificadorReserva);

            //SqlParameter reserva = new SqlParameter("@identificador_Reserva", SqlDbType.VarChar);
            //reserva.Value = identificadorReserva;
            SqlParameter costo = new SqlParameter("@costo_total", SqlDbType.Float);
            costo.Direction = ParameterDirection.Output;
            //comandoParaConsulta.Parameters.Add(reserva);
            comandoParaConsulta.Parameters.Add(costo);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();

            //CONSULTA NORMAL

            //System.Diagnostics.Debug.Write(costo.Value);
            //string consulta = "SELECT * FROM Hospedero";
            // DataTable tablaResultado = CrearTablaConsulta(consulta);
            //foreach (DataRow columna in tablaResultado.Rows)
            //{
            //System.Diagnostics.Debug.Write(columna["Nombre"]);
            //
            return (double)costo.Value;
        }

        public void insertar_PrecioReservacion(string indentificador, string adulto_nacional,
            string ninno_nacional, string adulto_extranjero, string ninno_extranjero)
        {
            string consulta = "insertar_PrecioReservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificador_Reserva", indentificador);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_nacional", adulto_nacional);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional", ninno_nacional);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_extranjero", adulto_extranjero);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_extranjero", ninno_extranjero);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
        public void insertarReserva(string identificador, string primerDia, string ultimoDia, string estado)
        {
            string consulta = "insertar_Reservacion";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", identificador);
            comandoParaConsulta.Parameters.AddWithValue("@primerDia_entrante", primerDia);
            comandoParaConsulta.Parameters.AddWithValue("@ultimoDia_entrante", ultimoDia);
            comandoParaConsulta.Parameters.AddWithValue("@estado_entrante", estado);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
        public void insertarHospedero(string identificacion, string email, string nombre, string apellido1,
            string apellido2, bool estado)
        {
            string consulta = "insertar_hospedero";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificacion_entrante", identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@email_entrante", email);
            comandoParaConsulta.Parameters.AddWithValue("@nombre_entrante", nombre);
            comandoParaConsulta.Parameters.AddWithValue("@apellido1_entrante", apellido1);
            comandoParaConsulta.Parameters.AddWithValue("@apellido2_entrante", apellido2);
            comandoParaConsulta.Parameters.AddWithValue("@estado_entrante", estado);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public void insertarPlacas(string identificador, string placa1, string placa2, string placa3, string placa4)
        {
            string consulta = "insertar_Placas";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificador_reserva", identificador);
            comandoParaConsulta.Parameters.AddWithValue("@placa1", placa1);
            comandoParaConsulta.Parameters.AddWithValue("@placa2", placa2);
            comandoParaConsulta.Parameters.AddWithValue("@placa3", placa3);
            comandoParaConsulta.Parameters.AddWithValue("@placa4", placa4);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public void insertarPago(string comprobante, string fechaPago)
        {
            string consulta = "insertar_Pago";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@comprobante", comprobante);
            comandoParaConsulta.Parameters.AddWithValue("@fecha_pago", fechaPago);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public void InsertarEnBaseDatos(HospederoModelo hospedero , ReservacionModelo reservacion)
        {

            insertarHospedero(hospedero.Identificacion, hospedero.Email , hospedero.Nombre , hospedero.Apellido1
                ,hospedero.Apellido2,false);
            insertarReserva(reservacion.Identificador,reservacion.PrimerDia,reservacion.UltimoDia,"0");

            switch (reservacion.placasVehiculos.Count)
            {
                case 0:
                    insertarPlacas(reservacion.Identificador, "", "", "", "");
                    break;
                case 1:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], "", "", "");
                    break;
                case 2:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], reservacion.placasVehiculos[1], "", "");
                    break;
                case 3:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], reservacion.placasVehiculos[1],
                        reservacion.placasVehiculos[2], "");
                    break;
                case 4:
                    insertarPlacas(reservacion.Identificador, reservacion.placasVehiculos[0], reservacion.placasVehiculos[1],
                       reservacion.placasVehiculos[2], reservacion.placasVehiculos[3]);
                    break;

            }

            insertar_PrecioReservacion(reservacion.Identificador , reservacion.cantTipoPersona[0].ToString(),
            reservacion.cantTipoPersona[1].ToString(), reservacion.cantTipoPersona[2].ToString(), 
            reservacion.cantTipoPersona[3].ToString());

            string identificadorPago = crearIdentificador(6);
            DateOnly date = new DateOnly();
            insertarPago(identificadorPago, date.ToString());

            insertarHospederoRealiza(hospedero.Identificacion, reservacion.Identificador,
            identificadorPago);


        }

        public void insertarHospederoRealiza(string identificador_hospedero, string identificador_Reserva, 
            string identificador_pago)
        {
            string consulta = "insertar_HospederoRealiza";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificador_hospedero", identificador_hospedero);
            comandoParaConsulta.Parameters.AddWithValue("@identificador_reserva", identificador_Reserva);
            comandoParaConsulta.Parameters.AddWithValue("@identificador_pago", identificador_pago);
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public ReservacionModelo LlenarCantidadPersonas(ReservacionModelo reservacion, IFormCollection form)
        {

             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Nacional"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Ninnos_Nacional"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Extranjero"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_ninnos_extranjero"]));
          


            return reservacion;
        }

        public ReservacionModelo LlenarFechas(ReservacionModelo reservacion, IFormCollection form)
        {

            reservacion.PrimerDia = form["fecha-entrada"];
            reservacion.UltimoDia = form["fecha-salida"];

            return reservacion;
        }

        public ReservacionModelo LlenarInformacionResarva(ReservacionModelo reservacion, IFormCollection form)
        {

            if (form["placa1"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            if (form["placa2"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa2"]);

            }

            if (form["placa3"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            if (form["placa4"] != "")
            {
                reservacion.placasVehiculos.Add(form["placa1"]);

            }

            string identificador = crearIdentificador(10);
            reservacion.Identificador = identificador;

            return reservacion;
        }

        public string crearIdentificador(int length)
        {
            const string allowedChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var result = new char[length];

            for (int i = 0; i < length; i++)
            {
                result[i] = allowedChars[_random.Next(0, allowedChars.Length)];
            }

            return new string(result);
        }
        public HospederoModelo LlenarHospedero(IFormCollection form)
        {
            HospederoModelo hospedero = new HospederoModelo();

            hospedero.Nombre = form["nombre"];
            hospedero.Apellido1 = form["primerApellido"];
            hospedero.Apellido2 = form["segundoApellido"];
            hospedero.Identificacion = form["identificacion"];
            hospedero.Telefono = form["segundoApellido"];
            hospedero.Email = form["email"];
            hospedero.TipoIdentificacion = form["nacionalidad"];
            hospedero.Nacionalidad = form["pais"];
            hospedero.Motivo = form["motivo"];
            hospedero.Estado = 0;

            return hospedero;
        }

        public string CrearConfirmacionMensaje(ReservacionModelo reservacion, HospederoModelo hospedero)
        {
            StringBuilder sb = new StringBuilder();

            // Encabezado
            sb.Append("<h2 style='text-align:center;'>Confirmación de Reserva</h2><br><br>");

            sb.Append("<h3>Datos del hospedero: </h3><br>");
            sb.Append("<h6>Identificación: " + hospedero.Identificacion + "</h6>");
            sb.Append("<h6>Nacionalidad: " + hospedero.Nacionalidad + "</h6>");
            sb.Append("<h6>Tipo de identificación: " + hospedero.TipoIdentificacion + "</h6>");
            sb.Append("<br><h6>" + hospedero.Nombre + " " +
                      hospedero.Apellido1 + " " + hospedero.Apellido2 + ", es un placer darles la bienvenida " +
                      "a Junquillal. Le deseamos un disfrute de su estadia,\n a continuación adjuntamos informacion de " +
                      "su reserva: </h6><br>");
            sb.Append("<h6>" + "</h6><br>");
            sb.Append("<h3> Detalles de la reserva: </h3><br>");
            sb.Append("<h6>Tu código de reservación es: " + reservacion.Identificador + "</h6>");
            sb.Append("<h6>Primer día: " + reservacion.PrimerDia + "</h6>");
            sb.Append("<h6>Último día: " + reservacion.UltimoDia + "</h6>");
            sb.Append("<h6>Cantidad de personas: " + reservacion.cantTipoPersona.Sum() + "</h6>");
            sb.Append("<ul>");

            if (reservacion.cantTipoPersona[0] != 0)
            {
                sb.Append("<li>Adultos nacionales: " + reservacion.cantTipoPersona[0] + "</li>");
            }
            if (reservacion.cantTipoPersona[1] != 0)
            {
                sb.Append("<li>Niños nacionales: " + reservacion.cantTipoPersona[1] + "</li>");
            }
            if (reservacion.cantTipoPersona[2] != 0)
            {
                sb.Append("<li>Adultos extranjeros: " + reservacion.cantTipoPersona[2] + "</li>");
            }
            if (reservacion.cantTipoPersona[3] != 0)
            {
                sb.Append("<li>Niños extranjeros: " + reservacion.cantTipoPersona[3] + "</li>");
            }
            sb.Append("</ul><br>");
            sb.Append("<h6>Placas de vehículos:</h6>");
            sb.Append("<ul>");
            foreach (string placa in reservacion.placasVehiculos)
            {
                sb.Append("<li>Placa: " + placa + "</li>");
            }
            sb.Append("</ul><br>");
            sb.Append("<h6>Motivo de la visita: " + hospedero.Motivo + "</h6>");

            return sb.ToString();

        }

        public void EnviarEmail(string mensaje , string correo)
        {
            SmtpClient smtp = new SmtpClient("smtp.office365.com", 587);
            smtp.EnableSsl = true;
            smtp.Credentials = new NetworkCredential("dinamita_PI@outlook.com", "PI_JUNQUILLAL");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.UseDefaultCredentials = false;

            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("dinamita_PI@outlook.com", "Confirmacion de reserva");
            mail.To.Add(new MailAddress("dinamita_PI@outlook.com"));
            mail.Subject = "Un gusto saludarle por parte de Junquillal";
            mail.IsBodyHtml = true;
            mail.Body = mensaje;

            smtp.Send(mail);


        }
    }

}
