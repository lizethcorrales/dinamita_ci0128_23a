﻿using System.Collections.Generic;
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

namespace JunquillalUserSystem.Areas.Admin.Controllers.Handlers
{
    enum ColumnaVisitacion
    {
        fecha,
        tipoDeVisitante,
        lugarDeProcedencia,
        estatus,
        tipoDeVisita,
        cantidad,
        motivo
    }

    enum ColumnaVentas
    {
        fecha,
        tipoDeVisitante,
        estatus,
        tipoDeVisita,
        cantidad,
        ventasTotales
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

        //si bool es 0 se hace visitacion y si es 1 se hace ventas
        public bool escribirXLS(List<ReportesModel> reportes, IFormCollection form, bool tipoReporteParaHacer)
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
            string archivo;
            string header = "AREA DE CONSERVACION GUANACASTE";
            string[,] contenidoDeExcel;
            if (tipoReporteParaHacer)
            {
                archivo = "LiquidacionReporte_" + dateNow + ".xlsx";
                contenidoDeExcel = agregarContenidoAExcelReporteLiquidacion(reportes, header);
            }
            else
            {
                archivo = "VisitacionReporte_" + dateNow + ".xlsx";
                contenidoDeExcel = agregarContenidoAExcelReporteVisitacion(reportes, header);
            }
            Worksheet worksheet = workbook.Worksheets.Add(archivo);
            worksheet.InsertArray(contenidoDeExcel, 1, 1);
            worksheet.AllocatedRange.AutoFitColumns();
            workbook.SaveToFile(archivo, ExcelVersion.Version2016);
            int cantidadColumnas = contenidoDeExcel.GetLength(1);
            char letra = (char)(64 + cantidadColumnas);
            fusionarCeldas(archivo, "A1:" + letra+"1");
            int cantidadFilas = contenidoDeExcel.GetLength(0);
            alinearCeldasHaciaIzquierda(archivo, "A2:" + letra + cantidadFilas);
            cambiarFontColor(archivo, "A2:" + letra + "2");
            agregarBold(archivo, "A1:" + letra + "2");
            agregarBordeACeldas(archivo, "A1:" + letra + cantidadFilas);
            return resultado;
        }

        public string[,] agregarContenidoAExcelReporteLiquidacion(List<ReportesModel> reportes, string header)
        {
            string[] nombreColumnas = new string[] { "FECHA", "TIPO DE VISITANTE", "ESTATUS", "TIPO DE VISITA", "CANTIDAD", "VENTAS TOTALES" };
            string[,] contenidoDeExcel = new string[reportes.Count+2, nombreColumnas.Length];
            string NINNOMAYOR = "Niño";
            string NINNOMENOR = "Niño menor 6 años";
            string NACIONAL = "Nacional";
            string nacionalidad = "";
            string COLONES = "₡";
            string DOLARES = "US$";
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
                        if (columna == (int)ColumnaVentas.fecha)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].PrimerDia;
                        }
                        else if (columna == (int)ColumnaVentas.tipoDeVisitante)
                        {
                            nacionalidad = reportes[fila - 2].Nacionalidad;
                            contenidoDeExcel[fila, columna] = nacionalidad;
                        }
                        else if (columna == (int)ColumnaVentas.estatus)
                        {
                            if (reportes[fila - 2].Poblacion == NINNOMAYOR)
                            {
                                contenidoDeExcel[fila, columna] = "Niño mayor de 6 años";
                            }
                            else if (reportes[fila - 2].Poblacion == NINNOMENOR)
                            {
                                contenidoDeExcel[fila, columna] = "Niño menor de 6 años";
                            } else
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila - 2].Poblacion;
                            }
                        }
                        else if (columna == (int)ColumnaVentas.tipoDeVisita)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].Actividad;

                        }
                        else if (columna == (int)ColumnaVentas.cantidad)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila-2].Cantidad.ToString();
                        }
                        else if (columna == (int)ColumnaVentas.ventasTotales)
                        {
                            if (nacionalidad.Equals(NACIONAL))
                            {
                                contenidoDeExcel[fila, columna] = COLONES + reportes[fila - 2].VentasTotales.ToString();
                            }
                            else
                            {
                                contenidoDeExcel[fila, columna] = DOLARES + reportes[fila - 2].VentasTotales.ToString();
                            }
                        }
                    }
                }
            }
            return contenidoDeExcel;
        }

        public string[,] agregarContenidoAExcelReporteVisitacion(List<ReportesModel> reportes, string header)
        {
            string[] nombreColumnas = new string[] { "FECHA", "TIPO DE VISITANTE", "LUGAR DE PROCEDENCIA", "ESTATUS", "TIPO DE VISITA", "CANTIDAD", "MOTIVO" };
            string[,] contenidoDeExcel = new string[reportes.Count + 2, nombreColumnas.Length];
            string nacionalidad = "";
            string NACIONAL = "Nacional";
            string NINNOMAYOR = "Niño";
            string NINNOMENOR = "Niño menor 6 años";
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
                        if (columna == (int)ColumnaVisitacion.fecha)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].PrimerDia;
                        }
                        else if (columna == (int)ColumnaVisitacion.tipoDeVisitante)
                        {
                            nacionalidad = reportes[fila - 2].Nacionalidad;
                            contenidoDeExcel[fila, columna] = nacionalidad;
                        }
                        else if (columna == (int)ColumnaVisitacion.lugarDeProcedencia)
                        {
                            if (nacionalidad.Equals(NACIONAL))
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila - 2].NombreProvincia;
                            }
                            else
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila - 2].NombrePais;
                            }
                        }
                        else if (columna == (int)ColumnaVisitacion.estatus)
                        {
                            if (reportes[fila - 2].Poblacion == NINNOMAYOR)
                            {
                                contenidoDeExcel[fila, columna] = "Niño mayor de 6 años";
                            }
                            else if (reportes[fila - 2].Poblacion == NINNOMENOR)
                            {
                                contenidoDeExcel[fila, columna] = "Niño menor de 6 años";
                            }
                            else
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila - 2].Poblacion;
                            }
                        }
                        else if (columna == (int)ColumnaVisitacion.tipoDeVisita)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].Actividad;

                        }
                        else if (columna == (int)ColumnaVisitacion.cantidad)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].Cantidad.ToString();
                        }
                        else if (columna == (int)ColumnaVisitacion.motivo)
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

        public void agregarBold(string nombreArchivo, string rangoCeldas)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(nombreArchivo);
            Worksheet sheet = workbook.Worksheets[0];
            CellRange range = sheet.Range[rangoCeldas];
            range.Style.Font.IsBold = true;
            workbook.SaveToFile(nombreArchivo, ExcelVersion.Version2016);            
        }

        public void cambiarFontColor(string nombreArchivo, string rangoCeldas)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(nombreArchivo);
            Worksheet sheet = workbook.Worksheets[0];
            CellRange range = sheet.Range[rangoCeldas];
            range.Style.Color = Color.FromArgb(153, 204, 204);
            workbook.SaveToFile(nombreArchivo, ExcelVersion.Version2016);
        }

        public void agregarBordeACeldas(string nombreArchivo, string rangoCeldas)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(nombreArchivo);
            Worksheet sheet = workbook.Worksheets[0];
            CellRange range = sheet.Range[rangoCeldas];
            IBorder leftBorder = range.Borders[BordersLineType.EdgeLeft];            
            leftBorder.LineStyle = LineStyleType.Thin;
            leftBorder.Color = Color.Black;
            IBorder rightBorder = range.Borders[BordersLineType.EdgeRight];            
            rightBorder.LineStyle = LineStyleType.Thin;
            rightBorder.Color = Color.Black;
            IBorder topBorder = range.Borders[BordersLineType.EdgeTop];            
            topBorder.LineStyle = LineStyleType.Thin;
            topBorder.Color = Color.Black;
            IBorder bottomBorder = range.Borders[BordersLineType.EdgeBottom];
            bottomBorder.LineStyle = LineStyleType.Thin;
            bottomBorder.Color = Color.Black;
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
