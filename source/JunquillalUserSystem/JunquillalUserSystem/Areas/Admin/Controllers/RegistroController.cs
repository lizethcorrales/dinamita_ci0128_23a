using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RegistroController : Controller
    {
        private RegistroHandler handlerRegistro = new RegistroHandler();

        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Registro(TrabajadorModelo empleadoNuevo)
        {
            try
            {
                string salNueva = empleadoNuevo.crearSal();
                empleadoNuevo.Sal = salNueva;
                string contraHash = empleadoNuevo.HashearContrasena(empleadoNuevo.Contrasena+empleadoNuevo.Sal);
                empleadoNuevo.Contrasena = contraHash;

                int resultado = handlerRegistro.registrarEmpleadoNuevo(empleadoNuevo);
                if (resultado == 1)
                {
                    ViewData["Mensaje"] = "Usuario Registrado Exitosamente";
                }
                else
                {
                    ViewData["Mensaje"] = "No fue posible registrar el Usuario";
                }
            }
            catch (NullReferenceException)
            {
                ViewData["Mensaje"] = "Hubo un problema en el sistema";
            }
            return View();
        }
    }
}
