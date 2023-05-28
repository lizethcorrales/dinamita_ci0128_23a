using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{
    public class VisitaHandler : HandlerBase
    {
        public VisitaHandler()
        {
            
        }

        public void insertar_Visita(HospederoModelo hospedero, ReservacionModelo reservacion)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertarVisita";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion", hospedero.Identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@fechaEntrada", reservacion.PrimerDia);

            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }

        public void insertarNacionalidadVisita(HospederoModelo hospedero, ReservacionModelo reservacion)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertarNacionalidadVisita";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion", hospedero.Identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@fechaEntrada", reservacion.PrimerDia);
            comandoParaConsulta.Parameters.AddWithValue("@NombrePais", hospedero.Nacionalidad);
            int cantidadPersonas = 0;
            for (int i = 0; i < reservacion.cantTipoPersona.Count(); i++)
            {
                cantidadPersonas += reservacion.cantTipoPersona[i];
            }
            comandoParaConsulta.Parameters.AddWithValue("@cantidad",cantidadPersonas);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
        public void insertarPrecioVisita(HospederoModelo hospedero, ReservacionModelo reservacion)
        {
            //se indica el procedimiento almacenado a ejecutarse
            string consulta = "insertar_PrecioVisita";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            //se agregan los parámetros que recibe el procedimiento
            comandoParaConsulta.Parameters.AddWithValue("@identificacion", hospedero.Identificacion);
            comandoParaConsulta.Parameters.AddWithValue("@fechaEntrada", reservacion.PrimerDia);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_nacional", reservacion.cantTipoPersona[0]);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional_mayor6", reservacion.cantTipoPersona[1]);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_nacional_menor6", reservacion.cantTipoPersona[2]);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_mayor_nacional", reservacion.cantTipoPersona[3]);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_extranjero", reservacion.cantTipoPersona[4]);
            comandoParaConsulta.Parameters.AddWithValue("@ninno_extranjero", reservacion.cantTipoPersona[5]);
            comandoParaConsulta.Parameters.AddWithValue("@adulto_mayor_extranjero", reservacion.cantTipoPersona[6]);
            //ejecución del query
            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
    }
}


