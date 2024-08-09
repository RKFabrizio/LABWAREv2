using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBW.Controllers
{
    public class AdministracionController : Controller
    {
        private LbwContext _context;
        public AdministracionController(LbwContext context)
        {
            _context = context;
        }

        public IActionResult TablaUsuario()
        {
            @ViewBag.admin = "active";
            @ViewBag.adusr = "active";
            return View();
        }

        public IActionResult TablaPuntoMuestra()
        {
            @ViewBag.admin = "active";
            @ViewBag.adpm = "active";
            return View();
        }

        public IActionResult TablaAnalisis()
        {
            @ViewBag.admin = "active";
            @ViewBag.adana = "active";
            return View();
        }

        public IActionResult TablaComponente()
        {
            @ViewBag.admin = "active";
            @ViewBag.adcom = "active";
            return View();
        }

    }
}
