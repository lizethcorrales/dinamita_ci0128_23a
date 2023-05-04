using System;
using System.Collections.Generic;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;

namespace JunquillalUserSystem.Handlers
{
    public class ReservacionHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;
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
        }
    }
}
