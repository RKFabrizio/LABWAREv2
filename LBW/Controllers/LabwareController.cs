using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBW.Controllers
{
    public class LabwareController : Controller
    {
        private LbwContext _context;
        public LabwareController(LbwContext context)
        {
            _context = context;
        }
        public IActionResult TablaMuestra()
        {
            @ViewBag.lbw = "active";
            @ViewBag.lbwmue = "active";
            return View();
        }

        public IActionResult TablaResultado()
        {
            @ViewBag.lbw = "active";
            @ViewBag.lbwres = "active";
            return View();
        }
    }
}
