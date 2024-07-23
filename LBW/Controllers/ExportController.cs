using AspNetCore.Reporting;
using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Text.RegularExpressions;
using Spire.Pdf;
using Spire.Pdf.Exporting;
using Microsoft.EntityFrameworkCore;
using Nito.Disposables;
using System.IO.Compression;


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
            Console.WriteLine("Reporte por Proyecto");

            using var reportCleanup = Disposable.Create(() =>
            {
                // Lógica de limpieza aquí
                GC.Collect();
                GC.WaitForPendingFinalizers();
            });

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
                // Limpia los parámetros existentes
                parameters.Clear();

                // Agrega el nuevo parámetro
                parameters.Add("Reporte_proyecto", proyecto.ToString());

                // Depuración: Imprimir los parámetros
                foreach (var param in parameters)
                {
                    Console.WriteLine($"Key: {param.Key}, Value: {param.Value}");
                }


                LocalReport localReport0 = null;
                try
                {
                    // Crear una nueva instancia de LocalReport cada vez para evitar parámetros retenidos
                    localReport0 = new LocalReport(path);

                    // Exportar a Excel
                    var result = localReport0.Execute(RenderType.Pdf, extension, parameters, mimetype);

                    string fileName = Proyecto.Name;
                    fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Reemplaza caracteres no válidos
                   
                    return File(result.MainStream, "application/pdf", fileName + ".pdf");
                     
                }
                finally
                {
                     
                }
                 
 
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
        public ActionResult ExportExcel(int proyecto)
        {
            Console.WriteLine("Reporte por Proyecto");

            using var reportCleanup = Disposable.Create(() =>
            {
                // Lógica de limpieza aquí
                GC.Collect();
                GC.WaitForPendingFinalizers();
            });

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
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformePrincipal\\Report1Excel.rdl";

                Dictionary<string, string> parameters = new Dictionary<string, string>();

                // Limpia los parámetros existentes
                parameters.Clear();

                // Agrega el nuevo parámetro
                parameters.Add("proyecto", proyecto.ToString());

                // Depuración: Imprimir los parámetros
                foreach (var param in parameters)
                {
                    Console.WriteLine($"Key: {param.Key}, Value: {param.Value}");
                }


                LocalReport localReport = null;
                try
                {
                    localReport = new LocalReport(path);

                    // Exportar a Excel
                    var result = localReport.Execute(RenderType.Excel, extension, parameters, mimetype);

                   

                    string fileName = Proyecto.Name;
                    fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Reemplaza caracteres no válidos
                    parameters.Remove("proyecto");
                    return File(result.MainStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName + ".xlsx");
                }
                finally
                {
                    // Aquí puedes agregar cualquier lógica de limpieza adicional si es necesario
                }
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

                return StatusCode(500, "Error interno del servidor al generar el archivo Excel");
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
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformeClienteFechas\\Reporte_PlantaGeneral.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Informe3_FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("Informe3_FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));
                parameters.Add("Informe3_IDClienteParam", idCliente.ToString());
                LocalReport localReport3 = new LocalReport(path);
                var result = localReport3.Execute(RenderType.Pdf, extension, parameters, mimetype);

                GC.Collect();
                GC.WaitForPendingFinalizers();

                // Generate a filename based on the date range
                string fileName = $"LIMSGS_Reporte_Planta_General_{fechaInicio:yyyyMMdd}_a_{fechaFin:yyyyMMdd}";
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_"); // Replace invalid characters
                parameters.Remove("Informe3_FechaInicioParam");
                parameters.Remove("Informe3_FechaFinParam");
                parameters.Remove("Informe3_IDClienteParam");
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
                var path = $"{this._iwebHostEnvironment.WebRootPath}\\InformeClienteFechas\\ReportAnalisis2.rdl";
                Dictionary<string, string> parameters = new Dictionary<string, string>();
                parameters.Add("Informe4_FechaInicioParam", fechaInicio.ToString("yyyy-MM-dd"));
                parameters.Add("Informe4_FechaFinParam", fechaFin.ToString("yyyy-MM-dd"));
                parameters.Add("Informe4_IDClienteParam", idCliente.ToString());

                LocalReport localReport4 = new LocalReport(path);
                var result = localReport4.Execute(RenderType.Pdf, extension, parameters, mimetype);

                GC.Collect();
                GC.WaitForPendingFinalizers();

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
