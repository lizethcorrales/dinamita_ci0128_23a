using System.Data.SqlClient;
using JunquillalUserSystem.Models;
using NuGet.Protocol.Plugins;

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    public class AdministrarReservasHandler
    {
        private SqlConnection conexion;
        private string rutaConexion;

        public AdministrarReservasHandler()
        {
            var builder = WebApplication.CreateBuilder();
            rutaConexion =
            builder.Configuration.GetConnectionString("ContextoJunquillal");
            conexion = new SqlConnection(rutaConexion);
        }

       public  List<ReservacionModelo> ObtenerReservasPorFecha(string fecha)
        {
            List<ReservacionModelo> reservas = new List<ReservacionModelo>();

           
               

            string query = "SELECT * FROM dbo.ObtenerReservacionesPorFecha(@Fecha)";

            using (SqlCommand command = new SqlCommand(query, conexion))
            {
                command.Parameters.AddWithValue("@Fecha", fecha); // Fecha que deseas utilizar
                conexion.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
            
                        ReservacionModelo reservacion = new ReservacionModelo();
                        reservacion.PrimerDia = reader.GetDateTime(reader.GetOrdinal("PrimerDia")).ToString("yyyy-MM-dd");
                        reservacion.Identificador = reader.GetString(reader.GetOrdinal("IdentificadorReserva"));
                        reservacion.UltimoDia = reader.GetDateTime(reader.GetOrdinal("UltimoDia")).ToString("yyyy-MM-dd");
                        reservacion.hospedero.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));
                        reservacion.hospedero.Apellido1 = reader.GetString(reader.GetOrdinal("Apellido1"));
                        reservacion.hospedero.Apellido2 = reader.GetString(reader.GetOrdinal("Apellido2"));
                        reservacion.hospedero.Email= reader.GetString(reader.GetOrdinal("Email"));



                        reservas.Add(reservacion);
                    }
                }
            }
            

            return reservas;
        }
    }
}
