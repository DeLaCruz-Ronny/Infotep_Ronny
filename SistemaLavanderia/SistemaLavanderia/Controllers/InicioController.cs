using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SistemaLavanderia.Models;
using SistemaLavanderia.Recursos;
using SistemaLavanderia.Servicios.Contrato;
using SistemaLavanderia.Servicios.Implementacion;
using System.Security.Claims;

namespace SistemaLavanderia.Controllers
{
    public class InicioController : Controller
    {
        private readonly IUsuarioService _usuarioService;

        public InicioController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        public IActionResult Registrase()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrarse(Usuario modelo)
        {
            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);

            Usuario usuario_creado = await _usuarioService.Saveusuario(modelo);

            if (usuario_creado.IdUsuario > 0)
            {
                return RedirectToAction("IniciarSesion", "Inicio");
            }
            else
            {
                ViewData["mensaje"] = "No se pudo crear el usuario";
                return View();
            }
        }

        public IActionResult IniciarSesion()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IniciarSesion(string usuario, string clave)
        {
            Usuario usuario_encontrado = await _usuarioService.GetUsuarios(usuario, Utilidades.EncriptarClave(clave));

            if (usuario_encontrado == null)
            {
                ViewData["mensaje"] = "No se encontraron concidencias";
                return View();
            }

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,usuario_encontrado.Usuario1)
            };

            foreach (var item in usuario_encontrado.Rol.ToString())
            {
                claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
            }

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            AuthenticationProperties authProperties = new AuthenticationProperties()
            {
                AllowRefresh = true,
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Salir()
        {
            HttpContext.SignOutAsync("Cookies"); // Cierra la sesión del usuario
            return RedirectToAction("IniciarSesion", "Inicio");
        }
    }
}
