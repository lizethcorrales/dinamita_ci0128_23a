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

        public int registrarEmpleadoNuevo(TrabajadorModelo empleadoNuevo)
        {
            string consulta = "insertar_Trabajador";
            SqlCommand comandoParaConsulta = new SqlCommand(consulta, conexion);
            int resultado = 1;
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

            return resultado;
        }
    }
}
