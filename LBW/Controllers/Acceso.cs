using Microsoft.AspNetCore.Mvc;
using LBW.Data;
using LBW.Models;
using System.Text;
using System.Security.Cryptography;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using LBW.Models.Entity;
using Newtonsoft.Json;
using Usuario = LBW.Models.Usuario;

namespace LBW.Controllers
{
    public class Acceso : Controller
    {

        UsuarioDatos _UsuarioDatos = new UsuarioDatos();
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(Usuario _usuario)
        {

            var usuario = _UsuarioDatos.ValidarUsuario(_usuario.UsuarioID);
            
            try
            {
                if (usuario != null)
                {
                   var claims = new List<Claim>
                        {
                         new Claim(ClaimTypes.Name, usuario.NombreCompleto),
                         new Claim("UsuarioID", usuario.UsuarioID),
                         new Claim("CCliente", usuario.CCliente.ToString())
                        };

                    claims.Add(new Claim("UsuarioInfo", JsonConvert.SerializeObject(usuario)));

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsPrincipal));

                    var currentDateTime = DateTime.UtcNow;
                    var dateWithoutTime = new DateTime(currentDateTime.Year, currentDateTime.Month, currentDateTime.Day);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

                    // Aquí es donde se guarda la información del usuario en una cookie
                    string usuarioInfoJson = JsonConvert.SerializeObject(usuario);
                    var cookieOptions = new CookieOptions() { IsEssential = true };
                    HttpContext.Response.Cookies.Append("UsuarioInfo", usuarioInfoJson, cookieOptions);

                    return RedirectToAction("Bienvenida", "Inicio");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error al iniciar sesión: " + ex.Message;
                return View();
            }
            if(usuario == null)
            {
                TempData["Error"] = "No hay usuario";
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Acceso");
        }

    }
}
