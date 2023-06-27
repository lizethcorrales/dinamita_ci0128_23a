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
            } else
            {
                ultimoDia = form["fecha-salida"];
            }
            List<ReportesModel> reportes = new List<ReportesModel>();

            string consultaBaseDatos = @"SELECT R.PrimerDia, P.Nacionalidad,PR.NombreProvincia, TN.NombrePais, P.Poblacion, P.Actividad, SUM(P.Cantidad) AS Cantidad_Total, SUM(P.Cantidad*P.PrecioAlHacerReserva) AS Ventas_Totales
                                        FROM PrecioReservacion AS P LEFT JOIN Reservacion AS R ON P.IdentificadorReserva = R.IdentificadorReserva 
                                        LEFT JOIN ProvinciaReserva AS PR ON PR.IdentificadorReserva = R.IdentificadorReserva LEFT JOIN TieneNacionalidad AS TN 
                                        ON TN.IdentificadorReserva = R.IdentificadorReserva
                                        WHERE R.Estado != '2' AND R.PrimerDia >= '" + primerDia + "' AND R.UltimoDia <= '" 
                                        + ultimoDia + "' AND P.Actividad = '" + actividad + "' " +
                                        "GROUP BY  R.IdentificadorReserva, P.Nacionalidad, P.Poblacion, P.Actividad, R.PrimerDia, PR.NombreProvincia, TN.NombrePais ORDER BY R.PrimerDia;";

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
                    VentasTotales = Convert.ToDouble(columna["Ventas_Totales"])
                }) ;
                System.Diagnostics.Debug.WriteLine(columna["PrimerDia"]);
            }

            return reportes;
        }


        public bool escribirCSV(List<ReportesModel> reportes, IFormCollection form)
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
            string archivo = "reporte_" + dateNow + ".csv";
            string ruta = @"wwwroot/ReportesCSV" + archivo;
            string separador = "\t";
            StringBuilder salida = new StringBuilder();
            List<string> lista = new List<string>();

            string header = @"<div>Prueba</div>";
            string cadena = "Fecha" + separador + "Nacionalidad" + separador + "Nombre Provincia"+ separador + "Nombre Pais" + separador + "Poblacion" + separador + "Actividad" + separador + "Cantidad" + separador + "Ventas" + "\n";
            try
            {
                foreach (ReportesModel item in reportes)
                {
                    cadena += agregarDato(item, separador);
                    cadena += "\n";
                }
            } catch (NullReferenceException ex) 
            {
                return false;
            }
            lista.Add(header);
            lista.Add(cadena);
            try { 
                for (int i = 0; i < lista.Count; ++i)
                {
                    salida.AppendLine(string.Join(separador, lista[i]));
                    File.AppendAllText(ruta, salida.ToString(), Encoding.Unicode);
                }
                return true;
            } catch (Exception e) {
                return false;
            }
        }

        public string agregarDato(ReportesModel reporte, string separador)
        {
            try
            {
                return (reporte.PrimerDia + separador +reporte.Nacionalidad + separador +reporte.NombreProvincia + separador + 
                    reporte.NombrePais + separador+ reporte.Poblacion + separador + reporte.Actividad
                    + separador + reporte.Cantidad + separador + reporte.VentasTotales);
            } catch (NullReferenceException ex)
            {
                return null;
            }            
        }

        public bool escribirXLS(List<ReportesModel> reportes, IFormCollection form)
        {
            bool resultado = true;
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
            //Create a Workbook instance
            Workbook workbook = new Workbook();
            //Remove default worksheets
            workbook.Worksheets.Clear();
            //Add a worksheet and name it
            var dateNow = "del_" + primerDia + "_a_" + ultimoDia;
            string archivo = "reporte_" + dateNow + ".xlsx";
            Worksheet worksheet = workbook.Worksheets.Add(archivo);
            //Create a one-dimensional array
            string[] nombreColumnas = new string[] { "Fecha", "Tipo de Visitante", "Lugar de Procedencia", "Estatus", "Tipo de Visita", "Cantidad" };
            //Create a two-dimensional array
            string[,] contenidoDeExcel = new string[reportes.Count, nombreColumnas.Length];
            string nacionalidad = ""                ;
            string NACIONAL = "Nacional";


            for (int fila = 0; fila < reportes.Count; ++fila)
            {

                for (int columna = 0; columna < nombreColumnas.Length; ++columna)
                {
                    if (fila == 0) { 
                        contenidoDeExcel[fila, columna] = nombreColumnas[columna];
                    } else {
                        if (columna == 0)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila].PrimerDia;
                        } else if (columna == 1)
                        {
                            nacionalidad = reportes[fila].Nacionalidad;
                            contenidoDeExcel[fila, columna] = nacionalidad;
                        }
                        else if (columna == 2)
                        {
                            if (nacionalidad.Equals(NACIONAL))
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila].NombreProvincia;
                            } else
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila].NombrePais;
                            }
                            
                        }
                        else if (columna == 3)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila].Poblacion;
                        }
                        else if (columna == 4)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila].Actividad;
                        }
                        else if (columna == 5)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila].Cantidad.ToString();
                        }
                    }
                }
            }
            //Write the array to the worksheet starting from the cell A3
            worksheet.InsertArray(contenidoDeExcel, 1, 1);
            //Auto fit column width in the located range
            worksheet.AllocatedRange.AutoFitColumns();
            //Save to an Excel file
            workbook.SaveToFile(archivo, ExcelVersion.Version2016);
            return resultado;
        }

    }
}
