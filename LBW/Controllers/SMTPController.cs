using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LBW.Controllers
{
    public class SMTPController : Controller
    {
        public IActionResult SMTP()
        {
            @ViewBag.SMTP = "active";
            return View();
        }


    }
}
