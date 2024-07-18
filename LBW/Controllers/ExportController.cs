using AspNetCore.Reporting;
using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using Spire.Pdf;
using Spire.Pdf.Exporting;



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


                // Asegúrate de que el nombre del archivo sea válido
                string fileName = Proyecto.Name;
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Reemplaza caracteres no válidos
                return File(result.MainStream, "application/pdf", fileName + ".pdf");
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

        [HttpPost]
        public ActionResult ExportPdfPlanta(DateTime fechaInicio, DateTime fechaFin)
        {
            Console.WriteLine(fechaInicio);
            Console.WriteLine(fechaFin);

            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformePrincipal\\ReportPlanta1.rdl";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));

                LocalReport localReport = new LocalReport(path);

                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                // Generate a filename based on the date range
                string fileName = $"LIMSGS_Reporte_Planta_{fechaInicio:yyyyMMdd}_a_{fechaFin:yyyyMMdd}";
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Replace invalid characters

                return File(result.MainStream, "application/pdf", fileName + ".pdf");
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
                return StatusCode(500, "Error interno del servidor al generar el PDF por planta");
            }
        }

        [HttpPost]
        public ActionResult ExportPdfAnalisis(DateTime fechaInicio, DateTime fechaFin)
        {
            Console.WriteLine(fechaInicio);
            Console.WriteLine(fechaFin);

            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformePrincipal\\Report3.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));


                LocalReport localReport = new LocalReport(path);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                // Generate a filename based on the date range
                string fileName = $"LIMSGS_Reporte_Analisis_{fechaInicio:yyyyMMdd}_a_{fechaFin:yyyyMMdd}";
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Replace invalid characters

                return File(result.MainStream, "application/pdf", fileName + ".pdf");
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
                return StatusCode(500, "Error interno del servidor al generar el PDF por planta");
            }
        }

        [HttpPost]
        public ActionResult ExportPdfPlantaGeneral(DateTime fechaInicio, DateTime fechaFin, int idCliente)
        {
            Console.WriteLine(fechaInicio);
            Console.WriteLine(fechaFin);

            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformePrincipal\\ReportGerencia1.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));
                parameters.Add("IDClienteParam", idCliente.ToString());
                LocalReport localReport = new LocalReport(path);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                // Generate a filename based on the date range
                string fileName = $"LIMSGS_Reporte_Planta_General_{fechaInicio:yyyyMMdd}_a_{fechaFin:yyyyMMdd}";
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Replace invalid characters

                return File(result.MainStream, "application/pdf", fileName + ".pdf");
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
                return StatusCode(500, "Error interno del servidor al generar el PDF por planta");
            }
        }


        [HttpPost]
        public ActionResult ExportPdfAnalisisGeneral(DateTime fechaInicio, DateTime fechaFin, int idCliente)
        {
            Console.WriteLine(fechaInicio);
            Console.WriteLine(fechaFin);

            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformePrincipal\\ReportAnalisis2.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));
                parameters.Add("IDClienteParam", idCliente.ToString());

                LocalReport localReport = new LocalReport(path);
                var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

                // Generate a filename based on the date range
                string fileName = $"LIMSGS_Reporte_Analisis_General_{fechaInicio:yyyyMMdd}_a_{fechaFin:yyyyMMdd}";
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Replace invalid characters

                return File(result.MainStream, "application/pdf", fileName + ".pdf");
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
                return StatusCode(500, "Error interno del servidor al generar el PDF por planta");
            }
        }
    }
}
