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
                    grupoPoblacion = Convert.ToString(columna["Poblacion"]),
                    actividad = Convert.ToString(columna["Actividad"]),
                    precio = Convert.ToDouble(columna["Precio"])
                });
            }
            return tarifas;
        }

        public void actualizarPrecioTarifas(TarifaModelo tarifa)
        {
            string consulta = "actualizarPrecioTarifa";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            comandoParaConsulta.CommandType = CommandType.StoredProcedure;
            comandoParaConsulta.Parameters.AddWithValue("@Nacionalidad", tarifa.Nacionalidad);
            comandoParaConsulta.Parameters.AddWithValue("@Poblacion", tarifa.grupoPoblacion);
            comandoParaConsulta.Parameters.AddWithValue("@Actividad", tarifa.actividad);
            comandoParaConsulta.Parameters.AddWithValue("@Precio", tarifa.precio);

            conexion.Open();
            comandoParaConsulta.ExecuteNonQuery();
            conexion.Close();
        }
    }
}
