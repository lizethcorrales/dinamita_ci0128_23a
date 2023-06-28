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

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    enum Columna
    {
        fecha,
        tipoDeVisitante,
        lugarDeProcedencia,
        estatus,
        tipoDeVisita,
        cantidad,
        motivo
    }

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
                System.Diagnostics.Debug.WriteLine(columna["PrimerDia"]);
            }

            return reportes;
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
            string header = "AREA DE CONSERVACION GUANACASTE";
            string[,] contenidoDeExcel = agregarContenidoAExcel(reportes, header);
            worksheet.InsertArray(contenidoDeExcel, 1, 1);
            worksheet.AllocatedRange.AutoFitColumns();
            workbook.SaveToFile(archivo, ExcelVersion.Version2016);
            int cantidadColumnas = contenidoDeExcel.GetLength(1);
            char letra = (char)(64 + cantidadColumnas);
            fusionarCeldas(archivo, "A1:" + letra+"1");
            int cantidadFilas = contenidoDeExcel.GetLength(0);
            alinearCeldasHaciaIzquierda(archivo, "A2:" + letra + cantidadFilas);
            return resultado;
        }

        public string[,] agregarContenidoAExcel(List<ReportesModel> reportes, string header)
        {
            string[] nombreColumnas = new string[] { "Fecha", "Tipo de Visitante", "Lugar de Procedencia", "Estatus", "Tipo de Visita", "Cantidad", "Motivo" };
            string[,] contenidoDeExcel = new string[reportes.Count+2, nombreColumnas.Length];
            string nacionalidad = "";
            string NACIONAL = "Nacional";
            for (int fila = 0; fila < reportes.Count + 2; ++fila)
            {

                for (int columna = 0; columna < nombreColumnas.Length; ++columna)
                {
                    if (fila == 0)
                    {
                        contenidoDeExcel[fila, columna] = header;
                    }
                    else if (fila == 1)
                    {
                        contenidoDeExcel[fila, columna] = nombreColumnas[columna];
                    }
                    else
                    {
                        if (columna == (int)Columna.fecha)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].PrimerDia;
                        }
                        else if (columna == (int)Columna.tipoDeVisitante)
                        {
                            nacionalidad = reportes[fila-2].Nacionalidad;
                            contenidoDeExcel[fila, columna] = nacionalidad;
                        }
                        else if (columna == (int)Columna.lugarDeProcedencia)
                        {
                            if (nacionalidad.Equals(NACIONAL))
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila-2].NombreProvincia;
                            }
                            else
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila-2].NombrePais;
                            }
                        }
                        else if (columna == (int)Columna.estatus)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].Poblacion;
                        }
                        else if (columna == (int)Columna.tipoDeVisita)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].Actividad;

                        }
                        else if (columna == (int)Columna.cantidad)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].Cantidad.ToString();
                        }
                        else if (columna == (int)Columna.motivo)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].Motivo;
                        }
                    }
                }
            }
            return contenidoDeExcel;
        }

        public void alinearCeldasHaciaIzquierda(string nombreArchivo, string rangoCeldas)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(nombreArchivo);
            Worksheet sheet = workbook.Worksheets[0];
            CellRange range = sheet.Range[rangoCeldas];
            range.Style.HorizontalAlignment = HorizontalAlignType.Left;
            workbook.SaveToFile(nombreArchivo, ExcelVersion.Version2016);
        }

        public void fusionarCeldas(string nombreArchivo, string rangoCeldas)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(nombreArchivo);
            Worksheet sheet = workbook.Worksheets[0];
            CellRange range = sheet.Range[rangoCeldas];
            range.Merge();
            range.Style.HorizontalAlignment = HorizontalAlignType.Center;
            workbook.SaveToFile(nombreArchivo, ExcelVersion.Version2016);
        }
    }
}
