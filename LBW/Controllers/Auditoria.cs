using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LBW.Controllers
{
    public class Auditoria : Controller
    {
        private LbwContext _context;
        public Auditoria(LbwContext context)
        {
            _context = context;
        }

        public IActionResult AuditoriaV()
        {
            @ViewBag.au = "active";
            return View();
        }
    }
}
