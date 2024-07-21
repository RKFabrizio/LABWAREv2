using AspNetCore.Reporting;
using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using Spire.Pdf;
using Spire.Pdf.Exporting;
using Microsoft.EntityFrameworkCore;
using Nito.Disposables;


namespace LBW.Controllers
{
    public class ExportFechasController : Controller
    {
        private readonly IWebHostEnvironment _iwebHostEnvironment;
        private readonly LbwContext _context;
        private bool _isReportConfigured = false;

        public ExportFechasController(IWebHostEnvironment iwebHostEnvironment, LbwContext context)
        {
            _iwebHostEnvironment = iwebHostEnvironment;
            _context = context;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
        }

        
        [HttpPost]
        public ActionResult ExportPdfPlanta(DateTime fechaInicio, DateTime fechaFin)
        {
            Console.WriteLine(fechaInicio);
            Console.WriteLine(fechaFin);

            Console.WriteLine("Reporte por Planta Unitaria");

            using var reportCleanup = Disposable.Create(() =>
            {
                // Lógica de limpieza aquí
                GC.Collect();
                GC.WaitForPendingFinalizers();
            });

            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformeFechas\\Reporte_Planta.rdl";

                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Informe1_FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("Informe1_FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));

                LocalReport localReport1 = new LocalReport(path);

                var result = localReport1.Execute(RenderType.Pdf, extension, parameters, mimetype);

                reportCleanup.Abandon();

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
            Console.WriteLine("Reporte por Analisis");
            try
            {
                string mimetype = "";
                int extension = 1;
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformeFechas\\Report3.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Informe2_FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("Informe2_FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));

                 

                LocalReport localReport2 = new LocalReport(path);
                var result = localReport2.Execute(RenderType.Pdf, extension, parameters, mimetype);

                GC.Collect();
                GC.WaitForPendingFinalizers();

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
    }
}
