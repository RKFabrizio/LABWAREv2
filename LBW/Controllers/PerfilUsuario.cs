using Microsoft.AspNetCore.Mvc;

namespace LBW.Controllers
{
    public class PerfilUsuario : Controller
    {
        public IActionResult Perfil()
        {
            @ViewBag.perfil = "active";
            return View();
        }


    }
}
