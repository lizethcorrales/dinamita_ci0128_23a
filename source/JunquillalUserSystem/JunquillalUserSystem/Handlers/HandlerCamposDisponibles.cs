using System;
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
    public class HandlerCamposDisponibles
    {
        private SqlConnection conexion;
        private string rutaConexion;
        public HandlerCamposDisponibles()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("ContextoJunquillal");
            conexion = new SqlConnection(rutaConexion);
        }
        
        public int ReservasTotal(string fecha) { 
     
            //PROCEDIMIENTO

            string consulta = "ReservasTotales";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@fecha", fecha);
            SqlParameter campos = new SqlParameter("@espaciosOcupados", SqlDbType.Int);
            campos.Direction = ParameterDirection.Output;
            comandoParaConsulta.Parameters.Add(campos);

            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            System.Diagnostics.Debug.Write(campos.Value);
            conexion.Close();
            return (int)campos.Value;
        }
        public CamposDisponiblesModel LlenarFecha(IFormCollection form)
        {
            CamposDisponiblesModel campos = new CamposDisponiblesModel();
            campos.fecha = form["fecha-entrada"];
            return campos;
        }

    }

}