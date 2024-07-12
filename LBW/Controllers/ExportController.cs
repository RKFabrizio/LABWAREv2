using AspNetCore.Reporting;
using LBW.Models.Entity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LBW.Controllers
{
    public class ExportController : Controller
    {
        private readonly IWebHostEnvironment _iwebHostEnvironment;
        private readonly LbwContext _context;

        public ExportController(IWebHostEnvironment iwebHostEnvironment, LbwContext context)
        {
            _iwebHostEnvironment = iwebHostEnvironment;
            _context = context;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

        }

        [HttpPost]
        public ActionResult ExportPdf(int proyecto)
        {
            var Proyecto = _context.Proyectos
                        .Where(p => p.IdProyecto == proyecto)
                        .FirstOrDefault();
            if (Proyecto == null)
            {
                return NotFound("Proyecto no encontrado");
            }
            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformePrincipal\\Report1.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("proyecto", proyecto.ToString());
                LocalReport localReport = new LocalReport(path);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);
                if (result.MainStream.Length == 0)
                {
                    return BadRequest("No se pudo generar el PDF");
                }
                return File(result.MainStream, "application/pdf", Proyecto.Name + ".pdf");
            }
            catch (Exception ex)
            {
                // Log the full exception details
                Console.WriteLine($"Error details: {ex}");

                // Check for specific inner exceptions
                if (ex.InnerException != null)
                {
                    if (ex.InnerException is System.Data.SqlClient.SqlException sqlEx)
                    {
                        return StatusCode(500, $"Database error: {sqlEx.Message}");
                    }
                }

                return StatusCode(500, "Error interno del servidor al generar el PDF");
            }
        }
    }
}
