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
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using System.Reflection.Metadata;

namespace JunquillalUserSystem.Areas.Admin.Controllers
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
    public class GeneradorDeReportes
    {
            //si bool es 0 se hace visitacion y si es 1 se hace ventas
        public string escribirXLS(List<ReportesModel> reportes, IFormCollection form, bool tipoReporteParaHacer)
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
            Workbook workbook = new Workbook();
            workbook.Worksheets.Clear();
            var dateNow = "del_" + primerDia + "_a_" + ultimoDia;
            string archivo;
            string header = "AREA DE CONSERVACION GUANACASTE";
            string[,] contenidoDeExcel;
            string directorioArchivo = "";
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
            directorioArchivo = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            try
            {
                if (!Directory.Exists(directorioArchivo))
                {
                    Directory.CreateDirectory(directorioArchivo);
                }
                directorioArchivo = System.IO.Path.Combine(directorioArchivo, archivo);
                Worksheet worksheet = workbook.Worksheets.Add(archivo);
                worksheet.InsertArray(contenidoDeExcel, 1, 1);
                worksheet.AllocatedRange.AutoFitColumns();
                workbook.SaveToFile(directorioArchivo, ExcelVersion.Version2016);
                int cantidadColumnas = contenidoDeExcel.GetLength(1);
                char letra = (char)(64 + cantidadColumnas);
                fusionarCeldas(directorioArchivo, "A1:" + letra + "1");
                int cantidadFilas = contenidoDeExcel.GetLength(0);
                alinearCeldasHaciaIzquierda(directorioArchivo, "A2:" + letra + cantidadFilas);
                if (tipoReporteParaHacer)
                {
                    alinearCeldasDerechaLiquidacion(directorioArchivo, "F3:" + letra + cantidadFilas);
                }
                cambiarFontColor(directorioArchivo, "A2:" + letra + "2");
                agregarBold(directorioArchivo, "A1:" + letra + "2");
                agregarBordeACeldas(directorioArchivo, "A1:" + letra + cantidadFilas);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Se generó el error: " + ex.Message);
                return archivo;
            }
            return archivo;
        }

        public string[,] agregarContenidoAExcelReporteLiquidacion(List<ReportesModel> reportes, string header)
        {
            string[] nombreColumnas = new string[] { "FECHA", "TIPO DE VISITANTE", "ESTATUS", "TIPO DE VISITA", "CANTIDAD", "VENTAS TOTALES" };
            string[,] contenidoDeExcel = new string[reportes.Count + 2, nombreColumnas.Length];
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
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].PrimerDia;
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
                            }
                            else
                            {
                                contenidoDeExcel[fila, columna] = reportes[fila - 2].Poblacion;
                            }
                        }
                        else if (columna == (int)ColumnaVentas.tipoDeVisita)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].Actividad;

                        }
                        else if (columna == (int)ColumnaVentas.cantidad)
                        {
                            contenidoDeExcel[fila, columna] = reportes[fila - 2].Cantidad.ToString();
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

        public void alinearCeldasDerechaLiquidacion(string nombreArchivo, string rangoCeldas)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(nombreArchivo);
            Worksheet sheet = workbook.Worksheets[0];
            CellRange range = sheet.Range[rangoCeldas];
            range.Style.HorizontalAlignment = HorizontalAlignType.Right;
            workbook.SaveToFile(nombreArchivo, ExcelVersion.Version2016);
        }

    }
   
}

