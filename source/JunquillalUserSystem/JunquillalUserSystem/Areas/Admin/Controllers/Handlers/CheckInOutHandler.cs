﻿using System;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

//esta clase se encarga de manejar las interacciones con la base de datos cuando se hace una nueva reserva 
// y de finalizar los detalles de las nuevas reservas 


namespace JunquillalUserSystem.Handlers
{

    public class CheckInOutHandler : HandlerBase
    {
        private static readonly Random _random = new Random();


        public CheckInOutHandler()
        {
        }

        public void insertarHospedaje(string identificador, int numeroParcela)
        {
            string consulta = "insertarHospedajeReservacion"; // Nombre del procedimiento almacenado
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@identificadorReserva", identificador);
            comandoParaConsulta.Parameters.AddWithValue("@numeroParcela", numeroParcela);

            try
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
            }
   
        }


        public List<Parcela> obtenerParcelas()
        {
            List<Parcela> parcelas = new List<Parcela>();
            string consultaBaseDatos = "SELECT * FROM Parcela;";
            DataTable tablaDeParcelas = CrearTablaConsulta(consultaBaseDatos);
            foreach (DataRow columna in tablaDeParcelas.Rows)
            {
                parcelas.Add(
                new Parcela
                {
                    NumeroParcela = Convert.ToInt32(columna["NumeroParcela"]),
                    CapacidadParcela = Convert.ToInt32(columna["Capacidad"]),
                });
            }
            return parcelas;
        }

        public void CheckInOutReserva(string identificador , string opcion)
        {
            string  consulta = $"Update  Reservacion Set Estado = 1 Where  IdentificadorReserva  = '{identificador}'";
            if (opcion == "CheckOut")
            {
                consulta = $"Update  Reservacion Set Estado = 3 Where  IdentificadorReserva  = '{identificador}'";
            } 
          
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);

            try
            {
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
  
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al ejecutar la consulta: " + ex.Message);
            }
        }


    }

}
