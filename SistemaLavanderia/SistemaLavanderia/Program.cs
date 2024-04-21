using AspNetCoreHero.ToastNotification;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemaLavanderia.Models;
using SistemaLavanderia.Servicios.Contrato;
using SistemaLavanderia.Servicios.Implementacion;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


//Obtener la conexion
builder.Services.AddDbContext<LavanderiaContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Cadena"))
);


//Add cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(option =>
    {
        option.LoginPath = "/Usuario/IniciarSesion";
        option.ExpireTimeSpan = TimeSpan.FromMinutes(20);
    });


//Obteniendo la interfaz
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddControllersWithViews(option =>
{
    option.Filters.Add(new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None,
    });
});

//AgregandoNotyf
builder.Services.AddNotyf(opt =>
{
    opt.DurationInSeconds = 10;
    opt.IsDismissable = true;
    opt.Position = NotyfPosition.TopRight;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Inicio}/{action=IniciarSesion}/{id?}");

app.Run();


//namespace Afectivo_p.Controllers
//{
//    public class InicioController : Controller
//    {

//        private readonly IUsuarioService _UsuarioService;

//        public InicioController(IUsuarioService usuarioService)
//        {
//            _UsuarioService = usuarioService;
//        }

//        public IActionResult Registrarse()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> Registrarse(Usuario modelo)
//        {
//            modelo.Clave = Utilidades.EncriptarClave(modelo.Clave);

//            Usuario usuario_creado = await _UsuarioService.Saveusuario(modelo);

//            if (usuario_creado.IdUsuario > 0)
//            {
//                return RedirectToAction("IniciarSesion", "Inicio");
//            }
//            else
//            {
//                ViewData["mensaje"] = "No se pudo crear el usuario";
//                return View();
//            }
//        }

//        public IActionResult IniciarSesion()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> IniciarSesion(string usuario, string clave)
//        {
//            Usuario usuario_encontrado = await _UsuarioService.GetUsuarios(usuario, Utilidades.EncriptarClave(clave));

//            if (usuario_encontrado == null)
//            {
//                ViewData["mensaje"] = "No se encontraron concidencias";
//                return View();
//            }

//            List<Claim> claims = new List<Claim>()
//            {
//                new Claim(ClaimTypes.Name,usuario_encontrado.Nombre)
//            };

//            foreach (var item in usuario_encontrado.IdRol.ToString())
//            {
//                claims.Add(new Claim(ClaimTypes.Role, item.ToString()));
//            }

//            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

//            AuthenticationProperties authProperties = new AuthenticationProperties()
//            {
//                AllowRefresh = true,
//            };

//            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

//            return RedirectToAction("Index", "Home");
//        }


//    }
//}

//@using Microsoft.AspNetCore.Identity
//@using System.Security.Claims