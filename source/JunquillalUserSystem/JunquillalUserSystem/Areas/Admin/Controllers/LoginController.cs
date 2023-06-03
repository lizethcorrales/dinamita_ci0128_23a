using Microsoft.AspNetCore.Mvc;
using JunquillalUserSystem.Models;
using JunquillalUserSystem.Areas.Admin.Controllers.Handlers;
using Microsoft.AspNetCore.Routing;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;

namespace JunquillalUserSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LoginController : Controller
    {
        private LoginHandler handlerLogin = new LoginHandler();
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(TrabajadorModelo empleado)
        {
            TrabajadorModelo empleado2 = handlerLogin.ObtenerCredencialesTrabajador(empleado.ID);
            string hashLocal = empleado2.Contrasena;
            if (empleado2.ID != "")
            {
                TrabajadorModelo empleadoLogin = empleado;
                empleadoLogin.Sal = empleado2.Sal;
                string hashLogin = empleadoLogin.HashearContrasena($"{empleadoLogin.Contrasena}{empleadoLogin.Sal}");
                if (String.Equals(hashLogin, hashLocal, StringComparison.OrdinalIgnoreCase))
                {
                    if(String.Equals(empleado.Puesto, empleado2.Puesto, StringComparison.OrdinalIgnoreCase))
                    {
                        return RedirectToAction("Index", "Home", new { area = "Admin" });
                    }
                    else
                    {
                        ViewData["Mensaje"] = "El puesto es incorrecto";
                        return View();
                    }
                    
                } else
                {
                    ViewData["Mensaje"] = "La contraseña es incorrecta";
                    return View();
                }
            } else
            {
                ViewData["Mensaje"] = "Usuario no registrado";
                return View();
            }

        }
        public IActionResult Bienvenida()
        {
            return View();
        }
    }
}
