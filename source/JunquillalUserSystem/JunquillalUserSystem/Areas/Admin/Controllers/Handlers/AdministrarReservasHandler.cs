using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using JunquillalUserSystem.Models;
using Microsoft.VisualBasic;
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

		public List<ReservacionModelo> ObtenerReservas(string datoBusqueda, string tipoBusqueda)
		{
			List<ReservacionModelo> reservas = new List<ReservacionModelo>();
			string query = "";

			if (tipoBusqueda == "fecha")
			{
				query = "SELECT * FROM dbo.ObtenerReservacionesPorFecha(@Fecha)";
			}
			else
			{
				query = "SELECT * FROM dbo.ObtenerReservacionesPorIdentificador(@Identificador)";
			}


			using (SqlCommand command = new SqlCommand(query, conexion))
			{
				if (tipoBusqueda == "fecha")
				{
					command.Parameters.AddWithValue("@Fecha", datoBusqueda); // Fecha que deseas utilizar para buscar
				}
				else
				{
					command.Parameters.AddWithValue("@Identificador", datoBusqueda); //Codigo que deseas utilizar para buscar

				}

				conexion.Open();

				// Llamar al método para obtener los datos del reader
				using (SqlDataReader reader = command.ExecuteReader())
				{
					reservas = ObtenerReservasDesdeReader(reader);
				}

				conexion.Close();
			}

			foreach (ReservacionModelo reserva in reservas)
			{
				reserva.placasVehiculos = ObtenerPlacasReservacion(reserva.Identificador);
				reserva.tipoPersona = ObtenerPersonasReservacion(reserva.Identificador,reserva);
				
			}


			return reservas;
		}

		private List<ReservacionModelo> ObtenerReservasDesdeReader(SqlDataReader reader)
		{
			List<ReservacionModelo> reservas = new List<ReservacionModelo>();

			while (reader.Read())
			{
				ReservacionModelo reservacion = new ReservacionModelo();
				reservacion.PrimerDia = reader.GetDateTime(reader.GetOrdinal("PrimerDia")).ToString("yyyy-MM-dd");
				reservacion.Identificador = reader.GetString(reader.GetOrdinal("IdentificadorReserva"));
				reservacion.UltimoDia = reader.GetDateTime(reader.GetOrdinal("UltimoDia")).ToString("yyyy-MM-dd");
				reservacion.hospedero.Nombre = reader.GetString(reader.GetOrdinal("Nombre"));
				reservacion.hospedero.Apellido1 = reader.GetString(reader.GetOrdinal("Apellido1"));
				reservacion.hospedero.Apellido2 = reader.GetString(reader.GetOrdinal("Apellido2"));
				reservacion.hospedero.Email = reader.GetString(reader.GetOrdinal("Email"));
				reservacion.hospedero.Identificacion = reader.GetString(reader.GetOrdinal("Identificacion"));
				reservacion.hospedero.Nacionalidad = reader.GetString(reader.GetOrdinal("NombrePais"));
                reservacion.hospedero.Telefono= reader.GetString(reader.GetOrdinal("Telefono"));
                reservacion.TipoActividad = reader.GetString(reader.GetOrdinal("TipoActividad"));
                reservacion.actividad= reader.GetString(reader.GetOrdinal("Motivo"));
                reservacion.Estado = reader.GetInt32(reader.GetOrdinal("Estado"));




                reservas.Add(reservacion);
			}

			return reservas;
		}


		public List<String> ObtenerPlacasReservacion(string IdentificadorReserva)
		{
			List<string> placas = new List<string>();

			string query = "SELECT * FROM dbo.ObtenerPlacasReservacion(@Identificador)";



			using (SqlCommand command = new SqlCommand(query, conexion))
			{

				command.Parameters.AddWithValue("@Identificador", IdentificadorReserva); // Fecha que deseas utilizar
				conexion.Open();
				// Llamar al método para obtener los datos del reader
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						placas.Add(reader.GetString(reader.GetOrdinal("Placa")));
					}
				}

				conexion.Close();


			}

			return placas;
		}

		public Dictionary<string, Tuple<int, string>> ObtenerPersonasReservacion(string IdentificadorReserva , ReservacionModelo reservacion)
		{
			Dictionary<string, Tuple<int, string>> cantidadTipoPersona = new Dictionary<string, Tuple<int, string>>();

			string query = "SELECT * FROM dbo.ObtenerCantidaTipoPersona(@Identificador)";



			using (SqlCommand command = new SqlCommand(query, conexion))
			{
				int totalNoches = 0;
                command.Parameters.AddWithValue("@Identificador", IdentificadorReserva); // Fecha que deseas utilizar
				conexion.Open();
				// Llamar al método para obtener los datos del reader
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						int cantidadPersonas = (int)reader.GetInt16(reader.GetOrdinal("Cantidad"));
						int PrecioAlHacerReserva = (int)(reader.GetDouble("PrecioAlHacerReserva")) * cantidadPersonas;

                        if (reservacion.TipoActividad == "Camping")
                        {
                            DateTime primerDia = DateTime.ParseExact(reservacion.PrimerDia, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                            DateTime ultimoDia = DateTime.ParseExact(reservacion.UltimoDia, "yyyy-MM-dd", CultureInfo.InvariantCulture);

                            TimeSpan duracionReservacion = ultimoDia - primerDia;
                            totalNoches = duracionReservacion.Days;
							PrecioAlHacerReserva *= totalNoches;


                        }


                        cantidadTipoPersona.Add(reader.GetString(reader.GetOrdinal("Poblacion")) + " " + reader.GetString(reader.GetOrdinal("Nacionalidad")),
							Tuple.Create(cantidadPersonas, PrecioAlHacerReserva.ToString()));



                    }
				}

				conexion.Close();


			}

			return cantidadTipoPersona;
		}

		public void EliminarReservacion(string identificadorReserva)
		{
			string query = "Delete from Reservacion where IdentificadorReserva = @IdentificadorReserva";



			using (SqlCommand command = new SqlCommand(query, conexion))
			{

				command.Parameters.AddWithValue("@IdentificadorReserva", identificadorReserva); // Identificador de reserva a eliminar

				conexion.Open();
				command.ExecuteNonQuery();
				conexion.Close();
			}
		}
	}
}
		

