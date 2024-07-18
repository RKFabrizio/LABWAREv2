using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;

namespace LBW.Controllers
{
    public partial class InformesController : Controller
    {
        private LbwContext _context;
        public InformesController(LbwContext context)
        {
            _context = context;
        }

        public IActionResult Informes4()
        {
            ViewBag.informe = "active";
            @ViewBag.info = "active";
            return View();
        }

        public IActionResult Informes41()
        {
            ViewBag.informe = "active";
            @ViewBag.info2 = "active";
            return View();
        }
    }
}
