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
    }
}
