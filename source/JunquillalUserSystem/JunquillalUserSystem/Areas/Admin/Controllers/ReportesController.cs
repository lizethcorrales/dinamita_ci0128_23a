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
            
            if (CamposFaltantes(Request.Form))
            {
                TempData["Mensaje"] = "Recuerde completar todos los solicitados";
            } else
            {
                
                List<ReportesModel> listaCamping = reportesHandler.obtenerReporte(Request.Form, "Camping");
                List<ReportesModel> listaPicnic = reportesHandler.obtenerReporte(Request.Form, "Picnic");

                listaCamping.AddRange(listaPicnic);

                bool reporte = esReporteLiquidacion(Request.Form);

                string nombreArchivo = reportesHandler.escribirXLS(listaCamping, Request.Form, reporte);
                if (nombreArchivo.Equals(String.Empty))
                {
                    TempData["Mensaje"] = "Error: hubo un problema al intentar crear el archivo";
                } else {
                    TempData["Mensaje"] = "El reporte fue creado exitosamente";
                }
            }

            return RedirectToAction("Reportes", "Reportes");
        }

        public IActionResult Reportes()
        {
            return View();
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
    }
}
