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
using System.Data;
using System.Text.RegularExpressions;

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

            string consultaBaseDatos = @"SELECT P.Nacionalidad, P.Poblacion, P.Actividad, SUM(P.Cantidad) AS Cantidad_Total, SUM(P.Cantidad*P.PrecioAlHacerReserva) AS Ventas_Totales
	                                    FROM PrecioReservacion AS P JOIN Reservacion AS R ON P.IdentificadorReserva = R.IdentificadorReserva
	                                    WHERE R.Estado != '2' AND R.PrimerDia >= '" +primerDia+ "' AND R.UltimoDia <= '"+ultimoDia+ "' AND P.Actividad = '"+actividad+"' GROUP BY  P.Nacionalidad, P.Poblacion, P.Actividad";
            System.Diagnostics.Debug.WriteLine(consultaBaseDatos);
            DataTable tablaDeReporte = CrearTablaConsulta(consultaBaseDatos);
                foreach (DataRow columna in tablaDeReporte.Rows)
                {
                precioReservacion.Add(
                new PrecioReservacionDesglose
                {
                    Nacionalidad = Convert.ToString(columna["Nacionalidad"]),
                    Poblacion = Convert.ToString(columna["Poblacion"]),
                    Actividad = Convert.ToString(columna["Actividad"]),
                    Cantidad = Convert.ToInt32(columna["Cantidad_Total"]),
                    PrecioAlHacerReserva = Convert.ToDouble(columna["Ventas_Totales"])
                });
                }

            return precioReservacion;
        }


        public void escribirCSV(List<PrecioReservacionDesglose> precioReservacion, IFormCollection form)
        {
            string primerDia = form["fecha-entrada"];
            string ultimoDia = "";

            var tipoReporte = form["reportes"];

            if (tipoReporte == "diario")
            {
                ultimoDia = primerDia;
            }
            else
            {
                ultimoDia = form["fecha-salida"];
            }

            var dateNow = "del_" + primerDia + "_a_" + ultimoDia;
            string archivo = "reporte_" + dateNow+".csv";
            string ruta = @"wwwroot/ReportesCSV" + archivo;
            string separador = ",";
            StringBuilder salida = new StringBuilder();
            List<string> lista = new List<string>();

            string cadena = "Nacionalidad,Poblacion,Actividad,Cantidad,Ventas" + "\n";

            foreach(PrecioReservacionDesglose item in precioReservacion)
            {
                cadena += agregarDato(item) ;
                cadena += "\n";
            }
            lista.Add(cadena);

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
