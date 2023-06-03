using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using JunquillalUserSystem.Handlers;
using JunquillalUserSystem.Models;
using NuGet.Protocol.Plugins;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    public class ReportesHandler : HandlerBase
    {
        public ReportesHandler() { }

        public List<PrecioReservacionDesglose> obtenerReporte(IFormCollection form, string actividad)
        {
            string primerDia = form["fecha-entrada"];
            string ultimoDia = "";
            
            var tipoReporte = form["reportes"];

            if (tipoReporte == "diario")
            {
                ultimoDia = primerDia;
            } else
            {
                ultimoDia = form["fecha-salida"];
            }

            System.Diagnostics.Debug.WriteLine(primerDia);
            System.Diagnostics.Debug.WriteLine(ultimoDia);


            List<PrecioReservacionDesglose> precioReservacion = new List<PrecioReservacionDesglose>();
            string consulta = "select * from dbo.ReporteVisitacionVentasActividad(@primerDia, @ultimoDia, @actividad)";
            SqlCommand consultaCommand = new SqlCommand(consulta, conexion);
            consultaCommand.Parameters.AddWithValue("@primerDia", primerDia);
            consultaCommand.Parameters.AddWithValue("@ultimoDia", ultimoDia);
            consultaCommand.Parameters.AddWithValue("@actividad", actividad);

            string test = consultaCommand.ToString();

            System.Diagnostics.Debug.WriteLine(test);

            conexion.Open();
            using(SqlDataReader reader = consultaCommand.ExecuteReader())
            {
                precioReservacion = obtenerReporteDesdeReader(reader);
            }
            conexion.Close();
            return precioReservacion;
        }

        private List<PrecioReservacionDesglose> obtenerReporteDesdeReader(SqlDataReader reader)
        {
            List<PrecioReservacionDesglose> precioReservacion = new List<PrecioReservacionDesglose>();
            while(reader.Read())
            {
                PrecioReservacionDesglose precioReporte = new PrecioReservacionDesglose();
                precioReporte.Nacionalidad = reader.GetString(reader.GetOrdinal("Nacionalidad"));
                precioReporte.Poblacion = reader.GetString(reader.GetOrdinal("Poblacion"));
                precioReporte.Actividad = reader.GetString(reader.GetOrdinal("Actividad"));
                precioReporte.Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad_Total"));
                precioReporte.PrecioAlHacerReserva = reader.GetDouble(reader.GetOrdinal("Ventas_Totales"));
                
                precioReservacion.Add(precioReporte);

            }


            return precioReservacion;
        }

        public void escribirCSV(List<PrecioReservacionDesglose> precioReservacion)
        {
            var dateNow = DateOnly.FromDateTime(DateTime.Now);
            string archivo = "reporte_" + dateNow;
            string ruta = @"~/Areas/Admin/Views/Shared/ReportesCSV/" + archivo;
            string separador = ",";
            StringBuilder salida = new StringBuilder();
            List<string> lista = new List<string>();

            string cadena = "";
            foreach(PrecioReservacionDesglose item in precioReservacion)
            {
                cadena = agregarDato(item);
                lista.Add(cadena);
            } 


            for (int i = 0; i < lista.Count; ++i)
            {
                salida.AppendLine(string.Join(separador, lista[i]));
                File.AppendAllText(ruta, salida.ToString());
            }
        }

        public string agregarDato(PrecioReservacionDesglose precio)
        {
            return (precio.Nacionalidad + "," + precio.Poblacion + "," + precio.Actividad
                    + "," + precio.Cantidad + "," + precio.PrecioAlHacerReserva);
        }

    }
}
