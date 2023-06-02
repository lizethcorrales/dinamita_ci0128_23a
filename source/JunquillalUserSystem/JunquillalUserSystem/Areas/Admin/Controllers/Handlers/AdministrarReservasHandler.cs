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
				query = "SELECT * FROM dbo.ObtenerReservacionesPorIdentificador(@identificador)";
			}


			using (SqlCommand command = new SqlCommand(query, conexion))
			{
				if (tipoBusqueda == "fecha")
				{
					command.Parameters.AddWithValue("@Fecha", datoBusqueda); // Fecha que deseas utilizar para buscar
				}
				else
				{
					command.Parameters.AddWithValue("@identificador", datoBusqueda); //Codigo que deseas utilizar para buscar

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
				reserva.tipoPersona = ObtenerPersonasReservacion(reserva.Identificador);
			}


			return reservas;
		}

		private List<ReservacionModelo> ObtenerReservasDesdeReader(SqlDataReader reader)
		{
			List<ReservacionModelo> reservas = new List<ReservacionModelo>();

			while (reader.Read())
			{
				ReservacionModelo reservacion = new ReservacionModelo();
				reservacion.PrimerDia = reader.GetDateTime(reader.GetOrdinal("primerDia")).ToString("yyyy-MM-dd");
				reservacion.Identificador = reader.GetString(reader.GetOrdinal("IdentificadorReserva"));
				reservacion.UltimoDia = reader.GetDateTime(reader.GetOrdinal("ultimoDia")).ToString("yyyy-MM-dd");
				reservacion.Hospedero.Nombre = reader.GetString(reader.GetOrdinal("nombre"));
				reservacion.Hospedero.Apellido1 = reader.GetString(reader.GetOrdinal("apellido1"));
				reservacion.Hospedero.Apellido2 = reader.GetString(reader.GetOrdinal("apellido2"));
				reservacion.Hospedero.Email = reader.GetString(reader.GetOrdinal("email"));
				reservacion.Hospedero.Identificacion = reader.GetString(reader.GetOrdinal("identificacion"));
				//reservacion.hospedero.nacionalidad = reader.GetString(reader.GetOrdinal("NombrePais"));




				reservas.Add(reservacion);
			}

			return reservas;
		}


		private List<String> ObtenerPlacasReservacion(string IdentificadorReserva)
		{
			List<string> placas = new List<string>();

			string query = "SELECT * FROM dbo.ObtenerPlacasReservacion(@identificador)";



			using (SqlCommand command = new SqlCommand(query, conexion))
			{

				command.Parameters.AddWithValue("@identificador", IdentificadorReserva); // Fecha que deseas utilizar
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

		private Dictionary<string, Tuple<int, string>> ObtenerPersonasReservacion(string IdentificadorReserva)
		{
			Dictionary<string, Tuple<int, string>> cantidadTipoPersona = new Dictionary<string, Tuple<int, string>>();

			string query = "SELECT * FROM dbo.ObtenerCantidaTipoPersona(@identificador)";



			using (SqlCommand command = new SqlCommand(query, conexion))
			{

				command.Parameters.AddWithValue("@identificador", IdentificadorReserva); // Fecha que deseas utilizar
				conexion.Open();
				// Llamar al método para obtener los datos del reader
				using (SqlDataReader reader = command.ExecuteReader())
				{
					while (reader.Read())
					{
						cantidadTipoPersona.Add(reader.GetString(reader.GetOrdinal("Poblacion")) + " " + reader.GetString(reader.GetOrdinal("nacionalidad")),
							Tuple.Create((int)reader.GetInt16(reader.GetOrdinal("Cantidad")),
							reader.GetDouble(reader.GetOrdinal("PrecioAlHacerReserva")).ToString()));


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

				command.Parameters.AddWithValue("@IdentificadorReserva", identificadorReserva); // identificador de reserva a eliminar

				conexion.Open();
				command.ExecuteNonQuery();
				conexion.Close();
			}
		}
	}
}
		

