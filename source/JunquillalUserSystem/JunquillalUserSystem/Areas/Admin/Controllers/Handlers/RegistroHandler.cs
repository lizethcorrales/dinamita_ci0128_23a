using System.Data.SqlClient;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Handlers;
using NuGet.Protocol.Plugins;
using System.Data;

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    public class RegistroHandler : HandlerBase
    {
        public RegistroHandler() { }
        public SqlConnection Conexion { get { return conexion; } }

        public int existeEmpleado(string id)
        {
            TrabajadorModelo empleado = new TrabajadorModelo();

            string query = "SELECT * FROM dbo.ObtenerCredencialesTrabajador(@Identificacion)";

            int existe = 0;

            if (id != null)
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
                        }
                    }
                    conexion.Close();
                }

                if (empleado.ID != "") {
                    existe = 1;
                }

            }
            else
            {
                existe = 2;

            }
            return existe;
        }

        public int registrarEmpleadoNuevo(TrabajadorModelo empleadoNuevo)
        {
            string consulta = "insertar_Trabajador";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            int resultado = 1;
            int existeEmpl = 2;
            if(empleadoNuevo != null)
            {
                existeEmpl = existeEmpleado(empleadoNuevo.ID);
            }
            if (existeEmpl != 1 & existeEmpl != 2) { 
                if(empleadoNuevo.ID == "")
                {
                    empleadoNuevo.Nombre = null;
                }
                try
                {
                    comandoParaConsulta.CommandType = CommandType.StoredProcedure;
                    comandoParaConsulta.Parameters.AddWithValue("@Cedula_entrante", empleadoNuevo.ID);
                    comandoParaConsulta.Parameters.AddWithValue("@Nombre_entrante", empleadoNuevo.Nombre);
                    comandoParaConsulta.Parameters.AddWithValue("@Apellido1_entrante", empleadoNuevo.Apellido1);
                    comandoParaConsulta.Parameters.AddWithValue("@Apellido2_entrante", empleadoNuevo.Apellido2);
                    comandoParaConsulta.Parameters.AddWithValue("@Correo_entrante", empleadoNuevo.Correo);
                    comandoParaConsulta.Parameters.AddWithValue("@Puesto_entrante", empleadoNuevo.Puesto);
                    comandoParaConsulta.Parameters.AddWithValue("@Contrasena_entrante", empleadoNuevo.Contrasena);
                    comandoParaConsulta.Parameters.AddWithValue("@Salt_entrante", empleadoNuevo.Sal);
                    conexion.Open();
                    comandoParaConsulta.ExecuteNonQuery();
                    conexion.Close();
                }
                catch (Exception ex)
                {
                    resultado = 0;
                }
            } else
            {
                resultado = 2;
            }

            return resultado;
        }
    }
}
