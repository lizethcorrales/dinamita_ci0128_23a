using System;
using System.Collections.Generic;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;

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

        public ReservacionModelo llenarCantidadPersonas(ReservacionModelo reservacion, IFormCollection form)
        {

             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Nacional"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Ninnos_Nacional"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_Adultos_Extranjero"]));
             reservacion.cantTipoPersona.Add(int.Parse(form["cantidad_ninnos_extranjero"]));
          


            return reservacion;
        }
    }
}
