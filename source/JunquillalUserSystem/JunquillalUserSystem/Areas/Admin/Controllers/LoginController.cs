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
        private const string SessionKeyNombre = "_Nombre";
        private const string SessionKeyPuesto = "_Puesto";

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(TrabajadorModelo empleado, int GuardarSesion)
        {
            if(empleado != null)
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
                        //Iniciamos la sesion con ciertos datos
                        if(GuardarSesion == 1)
                        {
                            HttpContext.Session.SetString(SessionKeyNombre, (string)empleado2.Nombre);
                            HttpContext.Session.SetString(SessionKeyPuesto, (string)empleado2.Puesto);
                        }
                        //var nombre = HttpContext.Session.GetString(SessionKeyNombre);
                        var direccion = RedirectToAction("Index", "Home", new { area = "Admin" });
                        return direccion;
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
            else
            {
                ViewData["Mensaje"] = "Hubo un problema en el sistema";
                return View();
            }

        }
    }
}
