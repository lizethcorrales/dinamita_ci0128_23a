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
using Spire.Xls;
using System.ComponentModel;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Spire.Xls.Core;
using Spire.Pdf.Exporting.XPS.Schema;

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{

    public class ReportesHandler : HandlerBase
    {
        public ReportesHandler() { }

        public List<ReportesModel> obtenerReporte(IFormCollection form, string actividad)
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
            List<ReportesModel> reportes = new List<ReportesModel>();

            string consultaBaseDatos = @"SELECT R.PrimerDia, P.Nacionalidad,PR.NombreProvincia, TN.NombrePais, P.Poblacion, P.Actividad, SUM(P.Cantidad) AS Cantidad_Total, R.Motivo, SUM(P.Cantidad*P.PrecioAlHacerReserva) AS Ventas_Totales
                                        FROM PrecioReservacion AS P LEFT JOIN Reservacion AS R ON P.IdentificadorReserva = R.IdentificadorReserva 
                                        LEFT JOIN ProvinciaReserva AS PR ON PR.IdentificadorReserva = R.IdentificadorReserva LEFT JOIN TieneNacionalidad AS TN 
                                        ON TN.IdentificadorReserva = R.IdentificadorReserva
                                        WHERE R.Estado != '2' AND R.PrimerDia >= '" + primerDia + "' AND R.UltimoDia <= '"
                                        + ultimoDia + "' AND P.Actividad = '" + actividad + "' " +
                                        "GROUP BY  R.IdentificadorReserva, P.Nacionalidad, P.Poblacion, P.Actividad, R.PrimerDia, PR.NombreProvincia, TN.NombrePais, R.Motivo ORDER BY R.PrimerDia;";

            DataTable tablaDeReporte = CrearTablaConsulta(consultaBaseDatos);
            foreach (DataRow columna in tablaDeReporte.Rows)
            {
                reportes.Add(
                new ReportesModel
                {
                    PrimerDia = DateOnly.FromDateTime(Convert.ToDateTime(columna["PrimerDia"])).ToString(),
                    Nacionalidad = Convert.ToString(columna["Nacionalidad"]),
                    NombreProvincia = Convert.ToString(columna["NombreProvincia"]),
                    NombrePais = Convert.ToString(columna["NombrePais"]),
                    Poblacion = Convert.ToString(columna["Poblacion"]),
                    Actividad = Convert.ToString(columna["Actividad"]),
                    Cantidad = Convert.ToInt32(columna["Cantidad_Total"]),
                    Motivo= Convert.ToString(columna["Motivo"]),
                    VentasTotales = Convert.ToDouble(columna["Ventas_Totales"])
                });
            }

            return reportes;
        }
    }
}
