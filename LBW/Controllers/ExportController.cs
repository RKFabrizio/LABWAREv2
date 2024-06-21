using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using Microsoft.AspNetCore.Mvc;
using LBW.Models.Entity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml.Style;
using System.Drawing;
using Microsoft.VisualBasic;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net.Mail;



namespace LBW.Controllers
{
    public class ExportController : Controller
    {
        private LbwContext _context;

        public ExportController(LbwContext context)
        {
            _context = context;
        }

        public ActionResult ExportToExcel()
        {



            // Recopila los datos de las tablas anidadas
            var projects = _context.Proyectos
            .Include(p => p.MuestraPr.Where(m => m.Status == 21))
            .ThenInclude(s => s.ResultadosM)
            .Join(
                    _context.Usuarios,
                    p => p.Owner,
                    u => u.IdUser,
                    (p, u) => new { Proyecto = p, Usuario = u }
                )
                .Select(item => new
                {
                    item.Proyecto,
                    item.Usuario,
                    MuestrasPr = item.Proyecto.MuestraPr.Select(m => new
                    {
                        m.TextID,
                        m.Customer,
                        ResultadosM = m.ResultadosM.Select(r => new
                        {
                            r.IdAnalysis,
                            NameAnalysis = _context.Analisiss.Where(a => a.IdAnalisis == r.IdAnalysis).Select(a => a.NameAnalisis).FirstOrDefault(),
                            r.IdComponent,
                            NameComponent = _context.AnalisisDetalles.Where(ad => ad.IdComp == r.IdComponent).Select(ad => ad.NameComponent).FirstOrDefault(),
                            r.ResultNumber,
                            r.IdUnidad,
                            UnidadDisplayString = _context.Unidades.Where(u => u.IdUnidad == r.IdUnidad).Select(u => u.DisplayString).FirstOrDefault()
                        }).ToList()
                    }).ToList()
                })
                .ToList();

            // Crea el archivo Excel
            using (var package = new ExcelPackage())
            {
                // Hoja para Proyectos, Muestras y Resultados
                var worksheet = package.Workbook.Worksheets.Add("Datos");

                // Encabezados de Proyectos
                worksheet.Cells["A1"].Value = "Proyecto";
                worksheet.Cells["B1"].Value = "Creador";

                // Aplicar color de fondo a los encabezados
                worksheet.Cells["A1:B1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:B1"].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#DAE9F8"));


                int row = 2;
                foreach (var item in projects)
                {
                    // Datos de Proyectos
                    worksheet.Cells[row, 1].Value = item.Proyecto.Description;
                    worksheet.Cells[row, 2].Value = item.Usuario.NombreCompleto;

                    // Aplicar color de fondo a los datos
                    worksheet.Cells[row, 1, row, 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 2].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F2F2F2"));


                    row++;

                    // Encabezados de Muestras

                    worksheet.Cells[row, 2].Value = "Muestra";
                    worksheet.Cells[row, 3].Value = "Cliente";

                    // Aplicar color de fondo #C1F0C8 a las celdas de las columnas 1, 2 y 3
                    worksheet.Cells[row, 1, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                    worksheet.Cells[row, 1, row, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#C1F0C8"));

                    foreach (var sample in item.Proyecto.MuestraPr)
                    {
                        row++;

                        // Datos de Muestras
                        worksheet.Cells[row, 2].Value = sample.TextID;
                        worksheet.Cells[row, 3].Value = sample.Customer;

                        // Aplicar color de fondo a los datos
                        worksheet.Cells[row, 2, row, 3].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, 2, row, 3].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#F2F2F2"));

                        row++;

                        // Encabezados de Resultados

                        worksheet.Cells[row, 3].Value = "Análisis";
                        worksheet.Cells[row, 4].Value = "Componente";
                        worksheet.Cells[row, 5].Value = "Valor";
                        worksheet.Cells[row, 6].Value = "Unidad";

                        // Aplicar color de fondo #C1F0C8 a las celdas de las columnas 1, 2 y 3
                        worksheet.Cells[row, 2, row, 6].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        worksheet.Cells[row, 2, row, 6].Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml("#FFFF99"));

                        foreach (var result in sample.ResultadosM)
                        {
                            row++;

                            var analisis = _context.Analisiss.FirstOrDefault(a => a.IdAnalisis == result.IdAnalysis);
                            var componente = _context.AnalisisDetalles.FirstOrDefault(ad => ad.IdComp == result.IdComponent);
                            var unidad = _context.Unidades.FirstOrDefault(u => u.IdUnidad == result.IdUnidad);

                            // Datos de Resultados
                            worksheet.Cells[row, 3].Value = analisis?.NameAnalisis ?? result.IdAnalysis.ToString();
                            worksheet.Cells[row, 4].Value = result.NameComponent;
                            worksheet.Cells[row, 5].Value = result.ResultNumber;
                            worksheet.Cells[row, 6].Value = unidad?.DisplayString ?? result.IdUnidad.ToString();
                        }
                    }

                    row += 1; // Espaciado entre Proyectos
                }
                // Ajustar el ancho de las columnas automáticamente
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                // Guarda el archivo en un stream
                var stream = new MemoryStream();
                package.SaveAs(stream);
                var content = stream.ToArray();

                // Retorna el archivo para su descarga
                return File(content, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
            }
        }

        public ActionResult ExportToPdf()
        {
            // Recopila los datos de las tablas anidadas
            var projects = _context.Proyectos
     .Join(
         _context.Usuarios,
         p => p.Owner,
         u => u.IdUser,
         (p, u) => new { Proyecto = p, Usuario = u }
     )
     .Select(item => new
     {
         item.Proyecto,
         item.Usuario,
         MuestrasPr = item.Proyecto.MuestraPr
             .Where(m => m.Status == 21) // Filtro aplicado aquí
             .Select(m => new
             {
                 m.TextID,
                 m.Customer,
                 ResultadosM = m.ResultadosM.Select(r => new
                 {
                     r.IdAnalysis,
                     NameAnalysis = _context.Analisiss.Where(a => a.IdAnalisis == r.IdAnalysis).Select(a => a.NameAnalisis).FirstOrDefault(),
                     r.IdComponent,
                     NameComponent = _context.AnalisisDetalles.Where(ad => ad.IdComp == r.IdComponent).Select(ad => ad.NameComponent).FirstOrDefault(),
                     r.ResultNumber,
                     r.IdUnidad,
                     UnidadDisplayString = _context.Unidades.Where(u => u.IdUnidad == r.IdUnidad).Select(u => u.DisplayString).FirstOrDefault()
                 }).ToList()
             }).ToList()
     })
     .ToList();



            // Crear el documento PDF
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                document.Open();

                iTextSharp.text.Font titleFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 12, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font headerFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                iTextSharp.text.Font normalFont = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.NORMAL);

                foreach (var item in projects)
                {
                    // Agregar información del proyecto
                    Paragraph projectParagraph = new Paragraph();
                    projectParagraph.Add(new Chunk("Proyecto: ", titleFont));
                    projectParagraph.Add(new Chunk(item.Proyecto.Description, normalFont));
                    projectParagraph.Add(Chunk.NEWLINE);
                    projectParagraph.Add(new Chunk("Creador: ", titleFont));
                    projectParagraph.Add(new Chunk(item.Usuario.NombreCompleto, normalFont));
                    projectParagraph.SpacingAfter = 10f;
                    document.Add(projectParagraph);

                    foreach (var sample in item.MuestrasPr)
                    {
                        Paragraph sampleParagraph = new Paragraph();
                        sampleParagraph.Add(new Chunk("Muestra: ", headerFont));
                        sampleParagraph.Add(new Chunk(sample.TextID, normalFont));
                        sampleParagraph.Add(Chunk.NEWLINE);
                        sampleParagraph.Add(new Chunk("Cliente: ", headerFont));
                        sampleParagraph.Add(new Chunk(sample.Customer, normalFont));
                        sampleParagraph.SpacingAfter = 10f;
                        document.Add(sampleParagraph);



                        // Crear tabla para los resultados
                        PdfPTable table = new PdfPTable(4);
                        table.WidthPercentage = 100;

                        // Agregar encabezados de la tabla
                        table.AddCell(new PdfPCell(new Phrase("Análisis", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Componente", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Valor", headerFont)));
                        table.AddCell(new PdfPCell(new Phrase("Unidad", headerFont)));

                        foreach (var result in sample.ResultadosM)
                        {
                         
                            // Aquí movemos la lógica de búsqueda dentro del bucle
                            var analisis = _context.Analisiss.FirstOrDefault(a => a.IdAnalisis == result.IdAnalysis);
                            var unidad = _context.Unidades.FirstOrDefault(u => u.IdUnidad == result.IdUnidad);

                            table.AddCell(new Phrase(analisis?.NameAnalisis ?? result.IdAnalysis.ToString(), normalFont));
                            table.AddCell(new Phrase(result.NameComponent, normalFont));
                            table.AddCell(new Phrase(result.ResultNumber.ToString(), normalFont));
                            table.AddCell(new Phrase(unidad?.DisplayString ?? result.IdUnidad.ToString(), normalFont));
                        }

                        document.Add(table);
                        document.Add(Chunk.NEWLINE);

                    }
                 

                }
                document.Close();
                writer.Close();

     
                return File(ms.ToArray(), "application/pdf", "Report.pdf");
            }
        }

        public ActionResult ExportEmail()
        {
            //string lastReferenciaOCFilePath = null;
            //foreach (var file in ReferenciaOC)
            //{
            //    string fileName = file.FileName;

            //    // Verificar si el nombre del archivo tiene dos puntos al final
            //    if (fileName.EndsWith(".."))
            //    {
            //        // Remover los dos puntos al final
            //        fileName = fileName.TrimEnd('.', ' ');
            //    }

            //    string filePath = Path.Combine(folderPath, fileName);

            //    // Comprobar si ya existe un archivo con ese nombre
            //    if (System.IO.File.Exists(filePath))
            //    {
            //        // Obtener la extensión del archivo
            //        string fileExtension = Path.GetExtension(fileName);

            //        // Generar un nuevo nombre de archivo único
            //        int fileCount = 1;
            //        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            //        string newFileName = $"{fileNameWithoutExtension}1{fileExtension}";

            //        while (System.IO.File.Exists(Path.Combine(folderPath, newFileName)))
            //        {
            //            fileCount++;
            //            newFileName = $"{fileNameWithoutExtension}_{fileCount}{fileExtension}";
            //        }

            //        fileName = newFileName;
            //        filePath = Path.Combine(folderPath, fileName);
            //    }

            //    // Guardar el archivo con el nombre verificado
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await file.CopyToAsync(fileStream);
            //    }

            //    Console.WriteLine($"Archivo ReferenciaOC guardado: {filePath}");
            //    model.ReferenciaOC = fileName;
            //    lastReferenciaOCFilePath = filePath;
            //}


            ////----------------------
            //string lastProformacotizacionFilePath = null;
            //foreach (var file in Proformacotizacion)
            //{
            //    string fileName = file.FileName;

            //    // Verificar si el nombre del archivo tiene dos puntos al final
            //    if (fileName.EndsWith(".."))
            //    {
            //        // Remover los dos puntos al final
            //        fileName = fileName.TrimEnd('.', ' ');
            //    }

            //    string filePath = Path.Combine(folderPath, fileName);

            //    // Comprobar si ya existe un archivo con ese nombre
            //    if (System.IO.File.Exists(filePath))
            //    {
            //        // Obtener la extensión del archivo
            //        string fileExtension = Path.GetExtension(fileName);

            //        // Generar un nuevo nombre de archivo único
            //        int fileCount = 1;
            //        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            //        string newFileName = $"{fileNameWithoutExtension}1{fileExtension}";

            //        while (System.IO.File.Exists(Path.Combine(folderPath, newFileName)))
            //        {
            //            fileCount++;
            //            newFileName = $"{fileNameWithoutExtension}_{fileCount}{fileExtension}";
            //        }

            //        fileName = newFileName;
            //        filePath = Path.Combine(folderPath, fileName);
            //    }

            //    // Guardar el archivo con el nombre verificado
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await file.CopyToAsync(fileStream);
            //    }

            //    Console.WriteLine($"Archivo ReferenciaOC guardado: {filePath}");
            //    model.ProformaCotizacion = fileName;
            //    lastProformacotizacionFilePath = filePath;
            //}

            ////-----------------------


            //string lastFacturaFilePath = null;
            //foreach (var file in Factura)
            //{
            //    string fileName = file.FileName;

            //    // Verificar si el nombre del archivo tiene dos puntos al final
            //    if (fileName.EndsWith(".."))
            //    {
            //        // Remover los dos puntos al final
            //        fileName = fileName.TrimEnd('.', ' ');
            //    }

            //    string filePath = Path.Combine(folderPath, fileName);

            //    // Comprobar si ya existe un archivo con ese nombre
            //    if (System.IO.File.Exists(filePath))
            //    {
            //        // Obtener la extensión del archivo
            //        string fileExtension = Path.GetExtension(fileName);

            //        // Generar un nuevo nombre de archivo único
            //        int fileCount = 1;
            //        string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
            //        string newFileName = $"{fileNameWithoutExtension}1{fileExtension}";

            //        while (System.IO.File.Exists(Path.Combine(folderPath, newFileName)))
            //        {
            //            fileCount++;
            //            newFileName = $"{fileNameWithoutExtension}_{fileCount}{fileExtension}";
            //        }

            //        fileName = newFileName;
            //        filePath = Path.Combine(folderPath, fileName);
            //    }

            //    // Guardar el archivo con el nombre verificado
            //    using (var fileStream = new FileStream(filePath, FileMode.Create))
            //    {
            //        await file.CopyToAsync(fileStream);
            //    }

            //    Console.WriteLine($"Archivo ReferenciaOC guardado: {filePath}");
            //    model.Factura = fileName;
            //    lastFacturaFilePath = filePath;
            //}

            //string correo_emisor = SERVER_EMAIL;
            //string clave_emisor = "maVafRevUp23";

            ////string correo_emisor = "leedryk@gmail.com";
            ////string clave_emisor = "xxrlviitjlpqytrj";

            //MailAddress receptor = new(correoAprobador);
            //MailAddress emisor = new(correo_emisor);

            //MailMessage email = new MailMessage(emisor, receptor);
            //// Agrega los correos de los usuarios con idPerfil 2 y 3 a CC


            //if (lastFacturaFilePath != null)
            //{
            //    Attachment attachment = new Attachment(lastFacturaFilePath);
            //    email.Attachments.Add(attachment);
            //}
            //if (lastReferenciaOCFilePath != null)
            //{
            //    Attachment attachment = new Attachment(lastReferenciaOCFilePath);
            //    email.Attachments.Add(attachment);
            //}
            //if (lastProformacotizacionFilePath != null)
            //{
            //    Attachment attachment = new Attachment(lastProformacotizacionFilePath);
            //    email.Attachments.Add(attachment);
            //}

            return Ok();
        }
    }
}