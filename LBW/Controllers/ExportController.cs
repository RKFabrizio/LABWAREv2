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
using NuGet.Protocol;
using System.Net.Mime;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using CoreHtmlToImage;
using AspNetCore.Reporting;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using DevExtreme.AspNet.Mvc;

namespace LBW.Controllers
{
    public class ExportController : Controller
    {
        private readonly IWebHostEnvironment _iwebHostEnvironment1;

        public ExportController(IWebHostEnvironment iwebHostEnvironment)
        {
            this._iwebHostEnvironment1 = iwebHostEnvironment;
        }


        [HttpPost]
        public ActionResult ExportPdf(int proyecto)
        {
            string mimtype = "";
            int extension = 1;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            var path = $"{this._iwebHostEnvironment1.WebRootPath}\\InformePrincipal\\Report1.rdl";
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("proyecto", proyecto.ToString());
            LocalReport localReport = new LocalReport(path);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimtype);
            return File(result.MainStream, "application/pdf", "reporte_" + proyecto.ToString() + ".pdf");
        }

      
    }
}