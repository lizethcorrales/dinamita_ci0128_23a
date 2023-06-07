using System.Data.SqlClient;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Handlers;
using NuGet.Protocol.Plugins;

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    public class LoginHandler : HandlerBase
    {
        public LoginHandler() { }
        public SqlConnection Conexion { get { return conexion; } }
        public TrabajadorModelo ObtenerCredencialesTrabajador(string id)
        {
            TrabajadorModelo empleado = new TrabajadorModelo();

            string query = "SELECT * FROM dbo.ObtenerCredencialesTrabajador(@Identificacion)";

            if(id  != null)
            {
                using (SqlCommand command = new SqlCommand(query, conexion))
                {
                    command.Parameters.AddWithValue("@Identificacion", id);
                    conexion.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            empleado.ID = reader.GetString(reader.GetOrdinal("Cedula"));
                            empleado.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                            empleado.Apellido1 = reader.GetString(reader.GetOrdinal("Apellido1"));
                            empleado.Contrasena = reader.GetString(reader.GetOrdinal("Contrasena"));
                            empleado.Sal = reader.GetString(reader.GetOrdinal("Salt"));
                            empleado.Puesto = reader.GetString(reader.GetOrdinal("Puesto"));
                        }
                    }
                }

            }
            else
            {
                return null;

            }
            return empleado;
        }
    }
}

