using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using System.Collections.Generic;


namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ReportesController : Controller
    {
     
        [HttpPost]
        public IActionResult GenerarReporte()
        {
            ReportesHandler reportesHandler = new ReportesHandler();
            GeneradorDeReportes generadorDeReportes =  new GeneradorDeReportes();
            
            if (CamposFaltantes(Request.Form))
            {
                TempData["Mensaje"] = "Recuerde completar todos los solicitados";
            } else
            {
                
                List<ReportesModel> listaCamping = reportesHandler.obtenerReporte(Request.Form, "Camping");
                List<ReportesModel> listaPicnic = reportesHandler.obtenerReporte(Request.Form, "Picnic");

                listaCamping.AddRange(listaPicnic);

                bool reporte = esReporteLiquidacion(Request.Form);

                string nombreArchivo = generadorDeReportes.escribirXLS(listaCamping, Request.Form, reporte);
                if (nombreArchivo.Equals(String.Empty))
                {
                    TempData["Mensaje"] = "Error: hubo un problema al intentar crear el archivo";
                } else {
                    TempData["Mensaje"] = "El reporte fue creado exitosamente";
                    TempData["ListarReportes"] = true;
                }
            }

            return RedirectToAction("Reportes", "Reportes");
        }

        public IActionResult Reportes()
        {
            var items = GetFiles();
            return View(items);
        }

        public bool CamposFaltantes(IFormCollection form)
        {
            string fechaInicial = Request.Form["fecha-entrada"];
            string fechaFinal = Request.Form["fecha-salida"];
            string reporteSeleccionado = Request.Form["reportes"];
            if (StringVacio(fechaInicial))
            {
                return true;
            } else
            {
                if (reporteSeleccionado == "visitas")
                {
                    if(StringVacio(fechaFinal))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public bool esReporteLiquidacion(IFormCollection form)
        {
            string reporteLiquidacion = Request.Form["tipoReporte"];
            if (reporteLiquidacion == "liquidacion")
            {
                return true;
            } else
            {
                return false;
            }
        }

        public bool StringVacio(string valor)
        {
            return valor == null || valor.Length == 0 ? true : false;
        }

        [HttpGet]
        public IActionResult DeleteFile(string FileName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            string filePath = Path.Combine(folderPath, Path.GetFileName(FileName));

            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            TempData["ListarReportes"] = true;
            return RedirectToAction("Reportes");
        }

        [HttpGet]
        public IActionResult DeleteFiles(List<string> FileNames)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            string filePath;
            foreach(var file in FileNames)
            {
                filePath = Path.Combine(folderPath, Path.GetFileName(file));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
            TempData["ListarReportes"] = true;  
            return RedirectToAction("Reportes");
        }

        [HttpGet("download")]
        public IActionResult Download(string FileName)
        {
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            string filePath = Path.Combine(folderPath, Path.GetFileName(FileName));
            // Verifica si el archivo existe en la ruta especificada
            if (System.IO.File.Exists(filePath))
            {
                // Devuelve el archivo para su descarga
                var resultado = PhysicalFile(filePath, "application/force-download", FileName);
                return resultado;
            }
            // Si el archivo no existe, redirige o muestra un mensaje de error
            return RedirectToAction("Reportes");
        }

        private List<string> GetFiles()
        {
            string nombreDirectorioArchivo = Path.Combine(Directory.GetCurrentDirectory(), "Reportes");
            var directorioArchivo = new DirectoryInfo(nombreDirectorioArchivo);
            List<string> items = new List<string>();
            if (Directory.Exists(nombreDirectorioArchivo))
            {
                System.IO.FileInfo[] fileNames = directorioArchivo.GetFiles("*.*");
                foreach (var file in fileNames)
                {
                    items.Add(file.Name);
                }
            }
            return items;
        }
    }
}
