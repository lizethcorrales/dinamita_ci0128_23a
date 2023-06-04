using System.Data;
using System.Data.SqlClient;

namespace JunquillalUserSystem.Handlers
{    
    public abstract class HandlerBase
    {
        protected SqlConnection conexion;
        protected string rutaConexion;
        public HandlerBase()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("ContextoJunquillal");
            conexion = new SqlConnection(rutaConexion);
        }

        //método para llenar una tabla a partir de la información obtenida de una consulta a la base de datos
        protected DataTable CrearTablaConsulta(string consulta)
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
    }
}
