﻿using JunquillalUserSystem.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JunquillalUserSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            TempData["IsAdminArea"] = null;
            return View();
        }

        public IActionResult CamposDisponibles()
        {
            return View();
        }


        public IActionResult FormularioCantidadPersonas()
        {
            return View();
        }

        public IActionResult terminos_Condiciones()
        {
            return View();
        }

		public IActionResult Reservaciones()
		{
			return View();
		}

        public IActionResult DatosReserva()
        {
            return View();
        }

        public IActionResult FinalizarReserva()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}