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
using LBW.Reportes;
using static DevExpress.Xpo.Helpers.AssociatedCollectionCriteriaHelper;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;


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
        public IActionResult ExportReport(int proyecto, string format)
        {
            var Proyecto = _context.Proyectos
                .Where(p => p.IdProyecto == proyecto)
                .FirstOrDefault();

            if (Proyecto == null)
            {
                return NotFound("Proyecto no encontrado");
            }

            string fileName = Proyecto.Name;
            fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_");

            Report1 report = new Report1();

            report.Parameters["reporte_proyecto"].Value = proyecto;
            string contentType;
            string fileExtension;

            using (MemoryStream stream = new MemoryStream())
            {
                switch (format.ToLower())
                {

                    case "xlsx":
                        report.ExportToXlsx(stream);
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        fileExtension = ".xlsx";
                        break;

                    case "pdf":
                        report.ExportToPdf(stream);
                        contentType = "application/pdf";
                        fileExtension = ".pdf";
                        break;

                    default:
                        return BadRequest("Formato no soportado");
                }
                var fileNameWithExtension = $"{fileName}{fileExtension}";
                Response.Headers.Add("Content-Disposition", $"attachment; filename*=UTF-8''{Uri.EscapeDataString(fileNameWithExtension)}");
                return File(stream.ToArray(), contentType);
            }
        }

        [HttpPost]
        public IActionResult ExportPlantaReport(DateTime fechaInicio, DateTime fechaFin)
        {
            
            Report2 report = new Report2();

            report.Parameters["FechaInicio"].Value = fechaInicio;
            report.Parameters["FechaFin"].Value = fechaFin;


            using (MemoryStream stream = new MemoryStream())
            {
                report.ExportToPdf(stream);
                return File(stream.ToArray(), "application/pdf", "Reporte_Planta.pdf");
            }
        }

        [HttpPost]
        public IActionResult ExportAnalisisReport(DateTime fechaInicio, DateTime fechaFin)
        {

            ReportAnalisis report = new ReportAnalisis();

            report.Parameters["FechaInicio"].Value = fechaInicio;
            report.Parameters["FechaFin"].Value = fechaFin;


            using (MemoryStream stream = new MemoryStream())
            {
                report.ExportToPdf(stream);
                return File(stream.ToArray(), "application/pdf", "Reporte_Analisis.pdf");
            }
        }

        [HttpPost]
        public IActionResult ExportGeneralPlantaReport(DateTime fechaInicio, DateTime fechaFin, int IdCliente)
        {

            ReportPlantaGeneral report = new ReportPlantaGeneral();

            report.Parameters["FechaInicio"].Value = fechaInicio;
            report.Parameters["FechaFin"].Value = fechaFin;
            report.Parameters["IdCliente"].Value = IdCliente;

            using (MemoryStream stream = new MemoryStream())
            {
                report.ExportToPdf(stream);
                return File(stream.ToArray(), "application/pdf", "Reporte_General_Planta.pdf");
            }
        }

        [HttpPost]
        public IActionResult ExportGeneralAnalisisReport(DateTime fechaInicio, DateTime fechaFin, int IdCliente)
        {

            ReportGeneralAnalisis report = new ReportGeneralAnalisis();

            report.Parameters["FechaInicio"].Value = fechaInicio;
            report.Parameters["FechaFin"].Value = fechaFin;
            report.Parameters["IdCliente"].Value = IdCliente;

            using (MemoryStream stream = new MemoryStream())
            {
                report.ExportToPdf(stream);
                return File(stream.ToArray(), "application/pdf", "Reporte_General_Analisis.pdf");
            }
        }

        [HttpPost]
        public ActionResult EnviarCorreo(int proyecto)
        {
            try
            {
                var Proyecto = _context.Proyectos
                    .Where(p => p.IdProyecto == proyecto)
                    .FirstOrDefault();

                var Idusuario = _context.Proyectos
                    .Where(p => p.IdProyecto == proyecto)
                    .Select(p => p.Owner)
                    .FirstOrDefault();

                var correoUsuario = _context.Usuarios
                    .Where(p => p.IdUser == Idusuario)
                    .Select(p => p.Correo)
                    .FirstOrDefault();

                var IdCliente = _context.Usuarios
                    .Where(p => p.IdUser == Idusuario)
                    .Select(p => p.CCliente)
                    .FirstOrDefault();

                var usuarioConCopia = _context.Usuarios
                    .Where(p => p.CCliente == IdCliente && p.ConCopia == true)
                    .Select(p => p.Correo)
                    .ToList();

                if (Proyecto == null)
                {
                    return Json(new { success = false, message = "Proyecto no encontrado" });
                }

                string fileName = Proyecto.Name;
                fileName = Regex.Replace(fileName, @"[^A-Za-z0-9_\-]", "_");


                string Correo =  _context.Smpts.Where(p => p.ID == 1).Select(p => p.Correo).FirstOrDefault();
                string HostDb =  _context.Smpts.Where(p => p.ID == 1).Select(p => p.Host).FirstOrDefault();
                int Puerto = _context.Smpts.Where(p => p.ID == 1).Select(p => p.Puerto).FirstOrDefault();
                string Usuario =  _context.Smpts.Where(p => p.ID == 1).Select(p => p.Usuario).FirstOrDefault();
                string Contrasena = _context.Smpts.Where(p => p.ID == 1).Select(p => p.Contrasena).FirstOrDefault();
                string BodyFromDB = _context.Smpts.Where(p => p.ID == 1).Select(p => p.Body).FirstOrDefault();



                //string correo_emisor = "leedryk@gmail.com";
                //string clave_emisor = "sfkickzqjzwicuqr";

                string correo_emisor = Correo;
                string clave_emisor = Contrasena;
                string correo_receptor = correoUsuario;

                 

                MailAddress receptor = new(correo_receptor);
                MailAddress emisor = new(correo_emisor);

                MailMessage email = new MailMessage(emisor, receptor);

                foreach (var correo in usuarioConCopia)
                {
                    email.CC.Add(new MailAddress(correo));
                }

                email.Subject = "Reporte Resultado " + Proyecto.Name;

                string body = $@"<body>
                <p>Estimado Cliente</p>
                <p>{BodyFromDB}</p>
                <p>Saludos</p>
                <p>Barrick Pierina</p>
                </body>";


                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                email.AlternateViews.Add(htmlView);
                email.Body = body;
                email.IsBodyHtml = true;

              
                Report1 report = new Report1();
                report.Parameters["reporte_proyecto"].Value = proyecto;

                using (MemoryStream pdfStream = new MemoryStream())
                {
                    report.ExportToPdf(pdfStream);
                    pdfStream.Seek(0, SeekOrigin.Begin);
                    Attachment pdfAttachment = new Attachment(pdfStream, $"{fileName}.pdf", "application/pdf");
                    email.Attachments.Add(pdfAttachment);

                    using (MemoryStream xlsxStream = new MemoryStream())
                    {
                        report.ExportToXlsx(xlsxStream);
                        xlsxStream.Seek(0, SeekOrigin.Begin);
                        Attachment xlsxAttachment = new Attachment(xlsxStream, $"{fileName}.xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                        email.Attachments.Add(xlsxAttachment);

                        SmtpClient smtp = new SmtpClient
                        {
                         
                            Host = HostDb,
                            Port = 587,
                            Credentials = new NetworkCredential(correo_emisor, clave_emisor),
                            DeliveryMethod = SmtpDeliveryMethod.Network,
                            EnableSsl = true
                        };
                        smtp.Send(email);
                    }
                }

                
 
         
 

                return Json(new { success = true, message = "Email enviado correctamente." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al enviar el email: " + ex.Message });
            }
        }

    }

}