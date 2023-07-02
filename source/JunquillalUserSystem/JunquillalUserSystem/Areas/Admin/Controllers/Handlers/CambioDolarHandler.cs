using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    public class CambioDolarHandler : HandlerBase
    {
        public CambioDolarHandler() { }

        public CambioDolarModel obtenerValorDeDolarActual()
        {
            string consultaBaseDatos = "SELECT ValorDolar FROM CambioDolar";
            CambioDolarModel cambioDolar = new();
            DataTable tablaDeReporte = CrearTablaConsulta(consultaBaseDatos);
            try { 
                foreach (DataRow columna in tablaDeReporte.Rows)
                {
                    var resultado = columna["ValorDolar"];
                    if (resultado != null)
                    {
                        cambioDolar.ValorDolar = Convert.ToDouble(resultado);
                    }
                    else
                    {
                        cambioDolar.ValorDolar = 0.0;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al leer los datos de la consulta: " + ex.Message);
            }
            return cambioDolar;
        }

        public bool modificarValorDeDolarActual(CambioDolarModel cambioDolar)
        {
            string consultaBaseDatos = "actualizarValorDolar";
            SqlCommand comandoParaConsulta = new SqlCommand(consultaBaseDatos, conexion);
            bool resultado = false;
            try
            {
                comandoParaConsulta.CommandType = CommandType.StoredProcedure;
                comandoParaConsulta.Parameters.AddWithValue("@nuevo_valor", cambioDolar.ValorDolar);
                conexion.Open();
                comandoParaConsulta.ExecuteNonQuery();
                conexion.Close();
                resultado = true;
            }
            catch (Exception ex)
            {
                return resultado;
            }

            return resultado;
        }
    }
}
