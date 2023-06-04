using JunquillalUserSystem.Models;
using System.Data;
using System.Data.SqlClient;

namespace JunquillalUserSystem.Handlers
{
    public class TarifasHandler : HandlerBase
    {
        public TarifasHandler() { }

        public List<TarifaModelo> obtenerTarifasActuales()
        {
            List<TarifaModelo> tarifas = new List<TarifaModelo>();
            string consultaBaseDatos = "SELECT * FROM Tarifa";
            DataTable tablaDeTarifas = CrearTablaConsulta(consultaBaseDatos);
            foreach(DataRow columna in tablaDeTarifas.Rows)
            {
                tarifas.Add(
                new TarifaModelo
                {
                    Nacionalidad = Convert.ToString(columna["Nacionalidad"]),
                    Poblacion = Convert.ToString(columna["Poblacion"]),
                    Actividad = Convert.ToString(columna["Actividad"]),
                    Precio = Convert.ToDouble(columna["Precio"])
                });
            }
            return tarifas;
        }

        public void actualizarPrecioTarifas(TarifaModelo tarifa)
        {
            if (TarifaNoNula(tarifa))
            {
                string consulta = "actualizarPrecioTarifa";
                SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
                try
                {
                    comandoParaConsulta.CommandType = CommandType.StoredProcedure;
                    comandoParaConsulta.Parameters.AddWithValue("@Nacionalidad", tarifa.Nacionalidad);
                    comandoParaConsulta.Parameters.AddWithValue("@Poblacion", tarifa.Poblacion);
                    comandoParaConsulta.Parameters.AddWithValue("@Actividad", tarifa.Actividad);
                    comandoParaConsulta.Parameters.AddWithValue("@Precio", tarifa.Precio);
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

        public bool TarifaNoNula(TarifaModelo tarifa)
        {
            return tarifa == null ? false : true;
        }
    }
}
