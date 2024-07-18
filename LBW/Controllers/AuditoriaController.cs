using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace LBW.Controllers
{
    public class AuditoriaController : Controller
    {
        private LbwContext _context;
        public AuditoriaController(LbwContext context)
        {
            _context = context;
        }

        public IActionResult AuditoriaResultado()
        {
            @ViewBag.audir = "active";
            return View();
        }
    }
}
