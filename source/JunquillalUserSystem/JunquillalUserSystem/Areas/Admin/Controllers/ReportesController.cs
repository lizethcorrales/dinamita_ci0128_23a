﻿using Microsoft.AspNetCore.Mvc;
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

            List<PrecioReservacionDesglose> listaCamping = reportesHandler.obtenerReporte(Request.Form, "Camping");
            List<PrecioReservacionDesglose> listaPicnic = reportesHandler.obtenerReporte(Request.Form, "Picnic");

            listaCamping.AddRange(listaPicnic);
            

            reportesHandler.escribirCSV(listaCamping, Request.Form);
            return RedirectToAction("Reportes", "Reportes");
        }

        public IActionResult Reportes()
        {
            return View();
        }


    }
}
