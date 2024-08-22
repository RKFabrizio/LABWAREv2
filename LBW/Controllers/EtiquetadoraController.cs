using DevExpress.DataAccess.EntityFramework;
using Microsoft.AspNetCore.Http;
using LBW.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using Zebra.Sdk.Comm;
using Zebra.Sdk.Printer;
using Zebra.Sdk.Printer.Discovery;
using DevExpress.XtraReports.UI;
using AspNetCore.Reporting;
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
    public class EtiquetadoraController : Controller
    {
        public IActionResult Etiquetadora()
        {
            @ViewBag.Etiquetadora = "active";
            return View();
        }

        private readonly IWebHostEnvironment _iwebHostEnvironment;
        private readonly LbwContext _context;

        public EtiquetadoraController(LbwContext context)
        {
            _context = context;
        }

        public void PrintXtraReportToZebraPrinter(int IdMuestra)
        {
            var muestra = _context.Muestras
               .Where(p => p.IdSample == IdMuestra)
               .FirstOrDefault();

            var conf = _context.Smpts
               .FirstOrDefault();

            // Obtener el proyecto por su ID
            var proyecto = _context.Proyectos
               .Where(p => p.IdProyecto == muestra.IdProject)
               .FirstOrDefault();

            if (proyecto == null)
            {
                Console.WriteLine("Proyecto no encontrado.");
                return;
            }

            string NameProyecto = proyecto.Name;
            DateTime? Fecha = proyecto.DateCreated;
            string FechaProyecto = Fecha?.ToString("dd-MMM-yyyy", new System.Globalization.CultureInfo("es-ES")).ToUpper();

             

           
                // Generar el ZPL para cada muestra
                string zplCommand = $@"
                    ^XA
                    ^LH0,0
                    ^PW812
                    ^LL600
                    ^FO20,40^A0N,40,40^FDPierina Laboratorio Quimico^FS

                    ^FO20,90^BY2
                    ^BCN,100,Y,N,N^FD{muestra.TextID}^FS

                    ^FO20,260^A0N,35,35^FDESPECIAL-1-17/06/24 10:55:05^FS
                    ^FO20,310^A0N,35,35^FD{NameProyecto}^FS
                    ^FO20,360^A0N,35,35^FD{FechaProyecto}^FS
                    ^PQ1
                    ^XZ";

                // Enviar el ZPL a la impresora Zebra
                SendZplToPrinter(zplCommand, conf.Ip, conf.Puerto); // Reemplaza con la IP y puerto de tu impresora
           
        }

        private void SendZplToPrinter(string zplCommand, string printerIpAddress, int printerPort)
        {
            TcpConnection connection = null;
            try
            {
                // Crear la conexión a la impresora Zebra
                connection = new TcpConnection(printerIpAddress, printerPort);
                connection.Open();

                // Crear instancia de la impresora
                ZebraPrinter printer = ZebraPrinterFactory.GetInstance(connection);

                // Enviar el comando ZPL
                printer.SendCommand(zplCommand);
            }
            catch (ConnectionException e)
            {
                // Manejar la excepción de conexión
                Console.WriteLine("Error al conectar con la impresora: " + e.Message);
            }
            finally
            {
                // Cerrar la conexión
                if (connection != null)
                {
                    connection.Close();  // Asegúrate de liberar recursos si es necesario
                }
            }
        }
        }
    

}
 