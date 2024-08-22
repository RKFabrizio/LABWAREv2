using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LBW.Models.Entity;
using System.Xml.Linq;
using System.Data;
using LBW.Models;
using SkiaSharp;
using System.Globalization;
using System.Data.SqlTypes;
using DevExtreme.AspNet.Mvc.Builders;

namespace LBW.Controllers
{
    [Route("api/[controller]/[action]")]
    public class MuestrasController : Controller
    {
        private LbwContext _context;

        public MuestrasController(LbwContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var muestras = _context.Muestras.Select(i => new {
                i.IdSample,
                i.IdPm,
                i.IdCliente,
                i.IdLocation,
                i.SampleNumber,
                i.TextID,
                i.Status,
                i.ChangedOn,
                i.OriginalSample,
                i.LoginDate,
                i.LoginBy,
                i.SampleDate,
                i.RecdDate,
                i.ReceivedBy,
                i.DateStarted,
                i.DueDate,
                i.DateCompleted,
                i.DateReviewed,
                i.PreBy,
                i.Reviewer,
                i.SamplingPoint,
                i.SampleType,
                i.IdProject,
                i.SampleName,
                i.Location,
                i.Customer,
                i.Observaciones,
                i.IdPlanta,
                i.IdGrado,
                i.AnalisisMuestra,
                i.ConteoPuntos,
                i.Fecha
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetMuestraPrevio(DataSourceLoadOptions loadOptions)
        {
            // Obtener la información del usuario desde las cookies
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            // Primera búsqueda: Obtener el último proyecto realizado por el usuario
            var ultimoProyecto = await _context.Proyectos
                .Where(p => p.Owner == usuario.IdUser)
                .OrderByDescending(p => p.IdProyecto)
                .FirstOrDefaultAsync();

            var muestras = _context.Muestras
                .Where(p => p.IdProject == ultimoProyecto.IdProyecto)
                .Select(i => new
                {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra,
                    i.IdProducto
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public Task<IActionResult> GetFilteredData(string date, DataSourceLoadOptions loadOptions)
        {

            Console.WriteLine(date);
            DateTime parsedDate = DateTime.Parse(date);


            var muestras = _context.Muestras
                .Where(m =>
                    m.Status == 254 &&
                    m.Fecha.HasValue &&
                    m.Fecha.ToString() == date)
                .Select(i => new
                {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra
                });

            return Task.FromResult<IActionResult>(Json(DataSourceLoader.Load(muestras, loadOptions)));
        }



        [HttpGet]
        public async Task<IActionResult> GetIdProject(int id, DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                .Where(m => m.IdProject == id &&  m.Status == 21)
                .Select(i => new {
                i.IdSample,
                i.IdPm,
                i.IdCliente,
                i.IdLocation,
                i.SampleNumber,
                i.TextID,
                i.Status,
                i.ChangedOn,
                i.OriginalSample,
                i.LoginDate,
                i.LoginBy,
                i.SampleDate,
                i.RecdDate,
                i.ReceivedBy,
                i.DateStarted,
                i.DueDate,
                i.DateCompleted,
                i.DateReviewed,
                i.PreBy,
                i.Reviewer,
                i.SamplingPoint,
                i.SampleType,
                i.IdProject,
                i.SampleName,
                i.Location,
                i.Customer,
                i.Observaciones,
                i.IdPlanta,
                i.IdGrado,
                    i.AnalisisMuestra
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetIdProjectA(int id, DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                .Where(m => m.IdProject == id && (m.Status == 24 || m.Status == 21))
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra
                });
            var muestras2 = _context.Muestras
              .Where(m => m.IdProject == id && (m.Status == 24 || m.Status == 21))
              .Select(i => new {
                  i.IdSample,
                  i.IdPm,
                  i.IdCliente,
                  i.IdLocation,
                  i.SampleNumber,
                  i.TextID,
                  i.Status,
                  i.ChangedOn,
                  i.OriginalSample,
                  i.LoginDate,
                  i.LoginBy,
                  i.SampleDate,
                  i.RecdDate,
                  i.ReceivedBy,
                  i.DateStarted,
                  i.DueDate,
                  i.DateCompleted,
                  i.DateReviewed,
                  i.PreBy,
                  i.Reviewer,
                  i.SamplingPoint,
                  i.SampleType,
                  i.IdProject,
                  i.SampleName,
                  i.Location,
                  i.Customer,
                  i.Observaciones,
                  i.IdPlanta,
                  i.IdGrado,
                  i.AnalisisMuestra
              }).ToList();
            if (muestras2.Count == 0)
            {
                // Manejar el caso cuando no se encuentran muestras
                return NotFound(); // Devolver un resultado 404 Not Found, o cualquier otro manejo adecuado
            }

            Console.WriteLine("............................");
            Console.Write("IdProyect:");
            Console.WriteLine(id);
            Console.WriteLine(muestras2[0]);
            Console.WriteLine("............................");
            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            var result = await DataSourceLoader.LoadAsync(muestras, loadOptions);

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetMuestrasA( DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                .Where(m => m.Status == 24 || m.Status == 21)
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra
                });
            var muestras2 = _context.Muestras
              .Where(m => m.Status == 24 || m.Status == 21)
              .Select(i => new {
                  i.IdSample,
                  i.IdPm,
                  i.IdCliente,
                  i.IdLocation,
                  i.SampleNumber,
                  i.TextID,
                  i.Status,
                  i.ChangedOn,
                  i.OriginalSample,
                  i.LoginDate,
                  i.LoginBy,
                  i.SampleDate,
                  i.RecdDate,
                  i.ReceivedBy,
                  i.DateStarted,
                  i.DueDate,
                  i.DateCompleted,
                  i.DateReviewed,
                  i.PreBy,
                  i.Reviewer,
                  i.SamplingPoint,
                  i.SampleType,
                  i.IdProject,
                  i.SampleName,
                  i.Location,
                  i.Customer,
                  i.Observaciones,
                  i.IdPlanta,
                  i.IdGrado,
                  i.AnalisisMuestra
              }).ToList();
            if (muestras2.Count == 0)
            {
                // Manejar el caso cuando no se encuentran muestras
                return NotFound(); // Devolver un resultado 404 Not Found, o cualquier otro manejo adecuado
            }

            Console.WriteLine("............................");
            Console.WriteLine(muestras2[0]);
            Console.WriteLine("............................");
            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            var result = await DataSourceLoader.LoadAsync(muestras, loadOptions);

            return Json(result);
        }



        [HttpGet]
        public async Task<IActionResult> GetIdProjectA1(int id, DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                .Where(m => m.IdProject == id)
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetMuestrasRec(DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                 .Where(m => m.Status >= 24 && m.Status <= 26)
                .Select(i => new {
                i.IdSample,
                i.IdPm,
                i.IdCliente,
                i.IdLocation,
                i.SampleNumber,
                i.TextID,
                i.Status,
                i.ChangedOn,
                i.OriginalSample,
                i.LoginDate,
                i.LoginBy,
                i.SampleDate,
                i.RecdDate,
                i.ReceivedBy,
                i.DateStarted,
                i.DueDate,
                i.DateCompleted,
                i.DateReviewed,
                i.PreBy,
                i.Reviewer,
                i.SamplingPoint,
                i.SampleType,
                i.IdProject,
                i.SampleName,
                i.Location,
                i.Customer,
                i.Observaciones,
                i.IdPlanta,
                i.IdGrado,
                    i.AnalisisMuestra,
                    i.Fecha

                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public IActionResult GetMuestrasPre(DataSourceLoadOptions loadOptions)
        {
            var muestrasConResultadosAuditoria = _context.Muestras
                .Where(m => m.Status == 799)
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra,
                    i.Fecha
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
   

            return Json(DataSourceLoader.Load(muestrasConResultadosAuditoria, loadOptions));
        }

        [HttpGet]
        public IActionResult GetSample(DataSourceLoadOptions loadOptions)
        {
            var muestrasConResultadosAuditoria = _context.Muestras
                .Where(m => m.Status == 24 || m.Status == 25 || m.Status == 26 || m.Status == 21)
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra,
                    i.Fecha
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.

            return Json(DataSourceLoader.Load(muestrasConResultadosAuditoria, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetMuestrasAutorizado(DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                 .Where(m => m.Status == 21 || m.Status == 24)
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra,
                    i.Fecha
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }



        [HttpGet]
        public IActionResult GetDate(int Estado)
        {
            Console.WriteLine(Estado + "ESTADO:");
            var muestras = _context.Muestras
                .Where(m => m.Status == Estado)
                .OrderByDescending(m => m.LoginDate)
                .Select(i => new {
                    i.IdSample,
                    i.IdPm,
                    i.IdCliente,
                    i.IdLocation,
                    i.SampleNumber,
                    i.TextID,
                    i.Status,
                    i.ChangedOn,
                    i.OriginalSample,
                    i.LoginDate,
                    i.LoginBy,
                    i.SampleDate,
                    i.RecdDate,
                    i.ReceivedBy,
                    i.DateStarted,
                    i.DueDate,
                    i.DateCompleted,
                    i.DateReviewed,
                    i.PreBy,
                    i.Reviewer,
                    i.SamplingPoint,
                    i.SampleType,
                    i.IdProject,
                    i.SampleName,
                    i.Location,
                    i.Customer,
                    i.Observaciones,
                    i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra,
                    i.Fecha
                })
                .ToList(); // Convertir a lista

            return Json(muestras);
        }

    

        [HttpGet]
        public async Task<IActionResult> Get2(DataSourceLoadOptions loadOptions)
        {
            var muestras = _context.Muestras
                .Where(m => m.Status == 254) // Filtrar por Status 254
                .OrderByDescending(m => m.Fecha)
                .Select(i => new {
                i.IdSample,
                i.IdPm,
                i.IdCliente,
                i.IdLocation,
                i.SampleNumber,
                i.TextID,
                i.Status,
                i.ChangedOn,
                i.OriginalSample,
                i.LoginDate,
                i.LoginBy,
                i.SampleDate,
                i.RecdDate,
                i.ReceivedBy,
                i.DateStarted,
                i.DueDate,
                i.DateCompleted,
                i.DateReviewed,
                i.PreBy,
                i.Reviewer,
                i.SamplingPoint,
                i.SampleType,
                i.IdProject,
                i.SampleName,
                i.Location,
                i.Customer,
                i.Observaciones,
                i.IdPlanta,
                i.IdGrado,
                    i.AnalisisMuestra,
                    i.Fecha
                });



            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> GetBySampleId(int idSample, DataSourceLoadOptions loadOptions)
        {

            Console.WriteLine(idSample.ToString());
            var muestras = _context.Muestras
                .Where(r => r.IdSample == idSample)
                .Select(i => new {
                i.IdSample,
                i.IdPm,
                i.IdCliente,
                i.IdLocation,
                i.SampleNumber,
                i.TextID,
                i.Status,
                i.ChangedOn,
                i.OriginalSample,
                i.LoginDate,
                i.LoginBy,
                i.SampleDate,
                i.RecdDate,
                i.ReceivedBy,
                i.DateStarted,
                i.DueDate,
                i.DateCompleted,
                i.DateReviewed,
                i.PreBy,
                i.Reviewer,
                i.SamplingPoint,
                i.SampleType,
                i.IdProject,
                i.SampleName,
                i.Location,
                i.Customer,
                i.Observaciones,
                i.IdPlanta,
                    i.IdGrado,
                    i.AnalisisMuestra
                });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public IActionResult GetByFilter(DateTime StartDate, DateTime EndDate)
        {
            var muestras = _context.Muestras
            .Where(ms => ms.LoginDate >= StartDate && ms.LoginDate <= EndDate && ms.IdCliente == 3)
            .Select(i => new
            {
                i.IdSample,
                i.IdPm,
                i.IdCliente,
                i.IdLocation,
                i.SampleNumber,
                i.TextID,
                i.Status,
                i.ChangedOn,
                i.OriginalSample,
                i.LoginDate,
                i.LoginBy,
                i.SampleDate,
                i.RecdDate,
                i.ReceivedBy,
                i.DateStarted,
                i.DueDate,
                i.DateCompleted,
                i.DateReviewed,
                i.PreBy,
                i.Reviewer,
                i.SamplingPoint,
                i.SampleType,
                i.IdProject,
                i.SampleName,
                i.Location,
                i.Customer,
                i.Observaciones,
                i.IdPlanta,
                i.IdGrado,
                i.AnalisisMuestra
            });

            return Json(muestras);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Muestra model, [FromForm] List<int> IdPm)
        {
            Console.WriteLine($"Valores de IdPm recibidos: {string.Join(", ", IdPm)}");

            if (IdPm == null || !IdPm.Any())
            {
                return BadRequest("Se requiere al menos un Punto de Muestra.");
            }

            var idPmArray = IdPm.ToArray();

            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);
            if (string.IsNullOrEmpty(usuario.Correo))
            {
                return Json(new { success = false, message = "Correo no establecido. Por favor, actualice su información." });
            }
            try
            {
                int sampleCount = idPmArray.Length;
                var ultimoProyecto = await _context.Proyectos
                    .Where(cl => cl.Owner == usuario.IdUser)
                    .OrderByDescending(cl => cl.IdProyecto)
                    .FirstOrDefaultAsync();
                var puntoMuestras = await _context.PuntoMuestras
                    .Where(pm => idPmArray.Contains(pm.IdPm))
                    .ToDictionaryAsync(pm => pm.IdPm, pm => new { pm.Description, pm.NamePm });
                var location = await _context.Ubicaciones
                    .Where(ub => ub.ID_LOCATION == model.IdLocation)
                    .Select(ub => ub.Name_location)
                    .FirstOrDefaultAsync();
                var cliente = await _context.Clientes
                    .Where(cl => cl.IdCliente == model.IdCliente)
                    .Select(cl => cl.NameCliente)
                    .FirstOrDefaultAsync();
                var producto = await _context.Productos
                    .Select(cl => cl.IdProducto)
                    .FirstOrDefaultAsync();

                model.IdLocation = 8;
                model.IdProducto = producto;

                List<Muestra> nuevasMuestras = new List<Muestra>();

                foreach (var idPm in idPmArray)
                {
                    if (!puntoMuestras.TryGetValue(idPm, out var puntoMuestra))
                    {
                        continue;
                    }
                    var newModel = new Muestra
                    {
                        IdCliente = model.IdCliente,
                        IdLocation = model.IdLocation,
                        IdPm = idPm,
                        TextID = $"{DateTime.Now:yyyyMMdd}_{puntoMuestra.Description}",
                        Status = 254,
                        ChangedOn = DateTime.Now,
                        LoginDate = DateTime.Now,
                        LoginBy = usuario.IdUser,
                        SamplingPoint = puntoMuestra.NamePm,
                        IdProject = ultimoProyecto.IdProyecto,
                        SampleName = puntoMuestra.Description,
                        SampleDate = null,
                        RecdDate = null,
                        ReceivedBy = 403,
                        DateStarted = null,
                        DueDate = null,
                        DateReviewed = null,
                        PreBy = null,
                        Reviewer = null,
                        Location = location,
                        Customer = cliente,
                        SampleNumber = sampleCount.ToString(),
                        ConteoPuntos = sampleCount.ToString(),
                        SampleType = model.SampleType,
                        IdPlanta = model.IdPlanta,
                        Observaciones = model.Observaciones,
                        IdProducto = model.IdProducto
                    };
                    nuevasMuestras.Add(newModel);
                }

                Console.WriteLine($"Número de IdPm recibidos: {idPmArray.Length}");
                // ... (en el bucle foreach)
                Console.WriteLine($"Creando muestra para IdPm: {IdPm}");
                // ... (después del bucle)
                Console.WriteLine($"Número de muestras a guardar: {nuevasMuestras.Count}");

                _context.Muestras.AddRange(nuevasMuestras);
                await _context.SaveChangesAsync();

                return Json(new { success = true, message = $"{nuevasMuestras.Count} muestras fueron creadas correctamente" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = $"Error al crear muestras: {ex.Message}" });
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Muestras.FirstOrDefaultAsync(item => item.IdSample == key);
            if(model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> PutStatus1(List<int> muestras)
        {
            try
            {
                if (muestras == null || !muestras.Any())
                {
                    return BadRequest("No IDs provided.");
                }


                // Obtener las muestras a actualizar
                var muestrasList = await _context.Muestras
                    .Where(m => muestras.Contains(m.IdSample))
                    .ToListAsync();

                // Actualizar el estado de cada muestra
                foreach (var muestraItem in muestrasList)
                {
                    muestraItem.Status = 25; // Modificar el estado según lo requerido
                    muestraItem.RecdDate = DateTime.Now;
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutStatus2(List<int> muestras)
        {
            try
            {
                if (muestras == null || !muestras.Any())
                {
                    return BadRequest("No IDs provided.");
                }


                // Obtener las muestras a actualizar
                var muestrasList = await _context.Muestras
                    .Where(m => muestras.Contains(m.IdSample))
                    .ToListAsync();

                // Actualizar el estado de cada muestra
                foreach (var muestraItem in muestrasList)
                {
                    muestraItem.Status = 27; // Modificar el estado según lo requerido
                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutStatus3(List<int> muestras)
        {
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            try
            {
                if (muestras == null || !muestras.Any())
                {
                    return BadRequest("No IDs provided.");
                }


                // Obtener las muestras a actualizar
                var muestrasList = await _context.Muestras
                    .Where(m => muestras.Contains(m.IdSample))
                    .ToListAsync();

                //Obtener los resultados a actualizar
                var resultadosList = await _context.Resultados
                    .Where(r => muestrasList.Select(m => m.IdSample).Contains(r.IdSample))
                    .ToListAsync();

                //Obtener los proyectos a actualizar
                var proyectosList = await _context.Proyectos
                    .Where(r => muestrasList.Select(m => m.IdProject).Contains(r.IdProyecto))
                    .ToListAsync();

                //Actualizar el estado de proyecto


                // Actualizar el estado de cada muestra
                foreach (var muestraItem in muestrasList)
                {
                    muestraItem.Status = 21; // Modificar el estado según lo requerido
                    muestraItem.RecdDate = DateTime.Now;
                    muestraItem.Reviewer = usuario.IdUser.ToString();
                }

                // Actualizar el estado de cada muestra
                foreach (var resultadoItem in resultadosList)
                {
                    resultadoItem.Status = 21; // Modificar el estado según lo requerido
                    resultadoItem.ChangedOn = DateTime.Now;
                    resultadoItem.Auditoria = false;
                }

                // Actualizar el estado de cada muestra
                foreach (var proyectoItem in proyectosList)
                {
                    proyectoItem.Status = 21; // Modificar el estado según lo requerido
                }


                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutStatus5(List<int> muestras)
        {
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            try
            {
                if (muestras == null || !muestras.Any())
                {
                    return BadRequest("No IDs provided.");
                }


                // Obtener las muestras a actualizar
                var muestrasList = await _context.Muestras
                    .Where(m => muestras.Contains(m.IdSample))
                    .ToListAsync();

                //Obtener los resultados a actualizar
                var resultadosList = await _context.Resultados
                    .Where(r => muestrasList.Select(m => m.IdSample).Contains(r.IdSample))
                    .ToListAsync();

                //Obtener los proyectos a actualizar
                var proyectosList = await _context.Proyectos
                    .Where(r => muestrasList.Select(m => m.IdProject).Contains(r.IdProyecto))
                    .ToListAsync();

                //Actualizar el estado de proyecto


                // Actualizar el estado de cada muestra
                foreach (var muestraItem in muestrasList)
                {
                    if(muestraItem.Status == 21)
                    {
                        muestraItem.Status = 799;
                        //muestraItem.Status = 24; // Modificar el estado según lo requerido
                    }
 
                }

                // Actualizar el estado de cada muestra
                foreach (var resultadoItem in resultadosList)
                {
                    if(resultadoItem.Status == 21)
                    {
                        resultadoItem.Status = 799; // Modificar el estado según lo requerido
                        resultadoItem.Auditoria = true;
                    }
                    
                }

                // Actualizar el estado de cada proyecto
                foreach (var proyectoItem in proyectosList)
                {
                    if (proyectoItem.Status == 21)
                    {
                        proyectoItem.Status = 799; // Modificar el estado según lo requerido
                    }

                }

                await _context.SaveChangesAsync();
                return Ok(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }



        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Muestras.FirstOrDefaultAsync(item => item.IdSample == key);

            _context.Muestras.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> ListasTipoMuestraLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Listas
                         where i.IdLista >= 617 && i.IdLista <= 635
                         orderby i.List
                         select new
                         {
                             Value = i.IdLista,
                             Text = i.NameLista
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ListasLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Listas
                         where (i.IdLista >= 21 && i.IdLista <= 27 || i.IdLista == 254) && i.IdLista != 22
                         orderby i.List
                         select new
                         {
                             Value = i.IdLista,
                             Text = i.Value
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }



        public IActionResult LastSample(int Cliente)
        {
            Cliente = 3;

            var lastIdSample = _context.Muestras.OrderByDescending(m => m.IdSample).Select(m => m.IdSample).FirstOrDefault();

            lastIdSample = lastIdSample + 1;

            Console.WriteLine(lastIdSample);
            return Json(new { LastIdSample = lastIdSample });
        }


        [HttpGet]
        public IActionResult ClienteShow(int Cliente)
        {

            var lookup = from i in _context.Clientes
                         where i.IdCliente == Cliente
                         orderby i.NameCliente
                         select new
                         {
                             Value = i.IdCliente,
                             Text = i.NameCliente
                         };

            return Json(lookup);
        }

        [HttpGet]
        public async Task<IActionResult> UsuariosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Usuarios
                         orderby i.NombreCompleto
                         select new {
                             Value = i.UsuarioID,
                             Text = i.NombreCompleto
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

 

        [HttpGet]
        public async Task<IActionResult> Usuarios1Lookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Usuarios
                         orderby i.NombreCompleto
                         select new
                         {
                             Value = i.IdUser,
                             Text = i.NombreCompleto
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> UbicacionesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Ubicaciones
                         orderby i.Name_location
                         select new {
                             Value = i.ID_LOCATION,
                             Text = i.Name_location
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> PuntoMuestrasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.PuntoMuestras
                         orderby i.NamePm
                         select new {
                             Value = i.IdPm,
                             Text = i.NamePm
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        //PuntoMuestrasLookup
        [HttpGet]
        public async Task<IActionResult> MuestrasPuntoLookup(DataSourceLoadOptions loadOptions)
        {
            // Obtener la información del usuario desde las cookies
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            var ultimoProyecto = await _context.Proyectos
                .Where(p => p.Owner == usuario.IdUser)
                .OrderByDescending(p => p.IdProyecto)
                .FirstOrDefaultAsync();

            if (ultimoProyecto == null)
            {
                return Json(new { });
            }

            var lookup = from i in _context.Muestras
                         where i.IdProject == ultimoProyecto.IdProyecto
                         orderby i.IdSample
                         select new
                         {
                             Value = i.IdSample,
                             Text = i.TextID
                         };

            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }




        [HttpGet]
        public async Task<IActionResult> CodigoPuntoMuestrasLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.PuntoMuestras
                         orderby i.NamePm
                         select new
                         {
                             Value = i.IdPm,
                             Text = i.C_CodPunto
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ProyectosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Proyectos
                         orderby i.Name
                         select new {
                             Value = i.IdProyecto,
                             Text = i.Name + " " + i.DateCreated
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ClientesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Clientes
                         orderby i.NameCliente
                         where (i.IdCliente == 3 || i.IdCliente == 6)
                         select new {
                             Value = i.IdCliente,
                             Text = i.NameCliente
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> Clientes1Lookup(DataSourceLoadOptions loadOptions)
        {
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            IQueryable<object> lookup;

            if (usuario.CCliente != 9)
            {
                lookup = from i in _context.Clientes
                         where i.IdCliente == usuario.CCliente
                         orderby i.NameCliente
                         select new
                         {
                             Value = i.IdCliente,
                             Text = i.NameCliente
                         };
            }
            else
            {
                lookup = from i in _context.Clientes
                         orderby i.NameCliente
                         select new
                         {
                             Value = i.IdCliente,
                             Text = i.NameCliente
                         };
            }

            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public IActionResult PlantaLookUp(int Cliente)
        {
            var lookup = from i in _context.Plantas
                         orderby i.NamePl
                         where i.IdCliente == Cliente
                         select new
                         {
                             Value = i.IdPlanta,
                             Text = i.NamePl
                         };


            return Json(lookup);
        }

        [HttpGet]
        public IActionResult PmLookUp(int Planta)
        {
            var lookup = from i in _context.PuntoMuestras
                         orderby i.NamePm
                         where i.IdPlanta == Planta
                         select new
                         {
                             Value = i.IdPm,
                             Text =  i.Description
                         };
 

            return Json(lookup);
        }

        [HttpGet]
        public async Task<IActionResult> PlantasPersonalizadoLookup(DataSourceLoadOptions loadOptions)
        {
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);
 
            var lookup = from i in _context.Plantas

                         orderby i.NamePl
                         select new
                         {
                             Value = i.IdPlanta,
                             Text = i.NamePl
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> TextIdLookup(DataSourceLoadOptions loadOptions)
        {
            // Obtener la información del usuario desde las cookies
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            // Primera búsqueda: Obtener el último proyecto realizado por el usuario
            var ultimoProyecto = await _context.Proyectos
                .Where(p => p.Owner == usuario.IdUser)
                .OrderByDescending(p => p.IdProyecto)
                .FirstOrDefaultAsync();

            if (ultimoProyecto == null)
            {
                // Manejar el caso en el que no se encuentre ningún proyecto para el usuario
                return NotFound("No se encontró ningún proyecto para el usuario");
            }

            // Calcular la fecha de diez minutos atrás desde el momento actual
            DateTime haceDiezMinutos = DateTime.UtcNow.AddMinutes(-10);

            // Segunda búsqueda: Obtener las muestras registradas en los últimos diez minutos asociadas al último proyecto
            var ultimasMuestras = await _context.Muestras
                .Where(m => m.IdProject == ultimoProyecto.IdProyecto)
                .OrderByDescending(m => m.IdProject)
                .Select(m => new
                {
                    Value = m.IdSample, // Asegúrate de que "TextID" es el nombre correcto del campo en tu base de datos
                    Text = m.TextID   // Para que coincida con ValueExpr
                })
                .ToListAsync();

            // Log de depuración
            if (ultimasMuestras.Any())
            {
                Console.WriteLine("Muestras encontradas: " + ultimasMuestras.Count);
            }
            else
            {
                Console.WriteLine("No se encontraron muestras en los últimos diez minutos");
            }

            if (!ultimasMuestras.Any())
            {
                // Manejar el caso en el que no se encuentren muestras para el último proyecto en los últimos diez minutos
                return NotFound("No se encontraron muestras para el último proyecto en los últimos diez minutos");
            }

            // Retornar las muestras encontradas en los últimos diez minutos
            return Json(ultimasMuestras);
        }


        

        [HttpGet]
        public async Task<IActionResult> GradosLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Grados
                         orderby i.Nombre
                         select new
                         {
                             Value = i.IdGrado,
                             Text = i.Nombre
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ProductosLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Productos
                         orderby i.Nombre
                         select new
                         {
                             Value = i.IdProducto,
                             Text = i.Nombre
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> PlantasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Plantas
                         orderby i.NamePl
                         select new {
                             Value = i.IdPlanta,
                             Text = i.NamePl
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> LaboratorioLookup(DataSourceLoadOptions loadOptions)
        {
            // Obtén todas las plantas con sus sitios asociados
            var lookup = from planta in _context.Plantas
                         join site in _context.Sites on planta.IdSite equals site.IdSite
                         orderby planta.NamePl
                         select new
                         {
                             Value = planta.IdPlanta,
                             Text = site.NameSite
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Muestra model, IDictionary values) {
            string ID_SAMPLE = nameof(Muestra.IdSample);
            string ID_PM = nameof(Muestra.IdPm);
            string ID_CLIENTE = nameof(Muestra.IdCliente);
            string ID_LOCATION = nameof(Muestra.IdLocation);
            string SAMPLE_NUMBER = nameof(Muestra.SampleNumber);
            string TEXT_ID = nameof(Muestra.TextID);
            string STATUS = nameof(Muestra.Status);
            string CHANGED_ON = nameof(Muestra.ChangedOn);
            string ORIGINAL_SAMPLE = nameof(Muestra.OriginalSample);
            string LOGIN_DATE = nameof(Muestra.LoginDate);
            string LOGIN_BY = nameof(Muestra.LoginBy);
            string SAMPLE_DATE = nameof(Muestra.SampleDate);
            string RECD_DATE = nameof(Muestra.RecdDate);
            string RECEIVED_BY = nameof(Muestra.ReceivedBy);
            string DATE_STARTED = nameof(Muestra.DateStarted);
            string DUE_DATE = nameof(Muestra.DueDate);
            string DATE_COMPLETED = nameof(Muestra.DateCompleted);
            string DATE_REVIEWED = nameof(Muestra.DateReviewed);
            string PRE_BY = nameof(Muestra.PreBy);
            string REVIEWER = nameof(Muestra.Reviewer);
            string SAMPLING_POINT = nameof(Muestra.SamplingPoint);
            string SAMPLE_TYPE = nameof(Muestra.SampleType);
            string ID_PROJECT = nameof(Muestra.IdProject);
            string SAMPLE_NAME = nameof(Muestra.SampleName);
            string LOCATION = nameof(Muestra.Location);
            string CUSTOMER = nameof(Muestra.Customer);
            string OBSERVACIONES = nameof(Muestra.Observaciones);
            string ID_PLANTA = nameof(Muestra.IdPlanta);
            string ID_GRADO = nameof(Muestra.IdGrado);
            string CONTEO_DE_PUNTOS = nameof(Muestra.ConteoPuntos);
            string ANALISIS_MUESTRA = nameof(Muestra.AnalisisMuestra);
            string FECHA = nameof(Muestra.Fecha);



            if (values.Contains(ID_SAMPLE)) {
                model.IdSample = Convert.ToInt32(values[ID_SAMPLE]);
            }

            if(values.Contains(ID_PM)) {
                model.IdPm = Convert.ToInt32(values[ID_PM]);
            }

            if(values.Contains(ID_CLIENTE)) {
                model.IdCliente = Convert.ToInt32(values[ID_CLIENTE]);
            }

            if(values.Contains(ID_LOCATION)) {
                model.IdLocation = Convert.ToInt32(values[ID_LOCATION]);
            }

            if(values.Contains(SAMPLE_NUMBER)) {
                model.SampleNumber = Convert.ToString(values[SAMPLE_NUMBER]);
            }

            if(values.Contains(TEXT_ID)) {
                model.TextID = Convert.ToString(values[TEXT_ID]);
            }

            if (values.Contains(ANALISIS_MUESTRA))
            {
                model.AnalisisMuestra = Convert.ToString(values[ANALISIS_MUESTRA]);
            }

            if (values.Contains(STATUS)) {
                model.Status = Convert.ToInt32(values[STATUS]);
            }

            if(values.Contains(CHANGED_ON)) {
                model.ChangedOn = values[CHANGED_ON] != null ? Convert.ToDateTime(values[CHANGED_ON]) : (DateTime?)null;
            }

            if(values.Contains(ORIGINAL_SAMPLE)) {
                model.OriginalSample = values[ORIGINAL_SAMPLE] != null ? Convert.ToInt32(values[ORIGINAL_SAMPLE]) : (int?)null;
            }

            if(values.Contains(LOGIN_DATE)) {
                model.LoginDate = values[LOGIN_DATE] != null ? Convert.ToDateTime(values[LOGIN_DATE]) : (DateTime?)null;
            }

            if(values.Contains(LOGIN_BY)) {
                model.LoginBy = Convert.ToInt32(values[LOGIN_BY]);
            }

            if(values.Contains(SAMPLE_DATE)) {
                model.SampleDate = values[SAMPLE_DATE] != null ? Convert.ToDateTime(values[SAMPLE_DATE]) : (DateTime?)null;
            }

            if(values.Contains(RECD_DATE)) {
                model.RecdDate = values[RECD_DATE] != null ? Convert.ToDateTime(values[RECD_DATE]) : (DateTime?)null;
            }

            if (values.Contains(FECHA))
            {
                model.Fecha = values[FECHA] != null ? Convert.ToDateTime(values[FECHA]) : (DateTime?)null;
            }

            if (values.Contains(RECEIVED_BY)) {
                model.ReceivedBy = Convert.ToInt32(values[RECEIVED_BY]);
            }

            if(values.Contains(DATE_STARTED)) {
                model.DateStarted = values[DATE_STARTED] != null ? Convert.ToDateTime(values[DATE_STARTED]) : (DateTime?)null;
            }

            if(values.Contains(DUE_DATE)) {
                model.DueDate = values[DUE_DATE] != null ? Convert.ToDateTime(values[DUE_DATE]) : (DateTime?)null;
            }

            if(values.Contains(DATE_COMPLETED)) {
                model.DateCompleted = values[DATE_COMPLETED] != null ? Convert.ToDateTime(values[DATE_COMPLETED]) : (DateTime?)null;
            }

            if(values.Contains(DATE_REVIEWED)) {
                model.DateReviewed = values[DATE_REVIEWED] != null ? Convert.ToDateTime(values[DATE_REVIEWED]) : (DateTime?)null;
            }

            if(values.Contains(PRE_BY)) {
                model.PreBy = Convert.ToString(values[PRE_BY]);
            }

            if(values.Contains(REVIEWER)) {
                model.Reviewer = Convert.ToString(values[REVIEWER]);
            }

            if(values.Contains(SAMPLING_POINT)) {
                model.SamplingPoint = Convert.ToString(values[SAMPLING_POINT]);
            }

            if(values.Contains(SAMPLE_TYPE)) {
                model.SampleType = values[SAMPLE_TYPE] != null ? Convert.ToInt32(values[SAMPLE_TYPE]) : (int?)null;
            }

            if(values.Contains(ID_PROJECT)) {
                model.IdProject = Convert.ToInt32(values[ID_PROJECT]);
            }

            if(values.Contains(SAMPLE_NAME)) {
                model.SampleName = Convert.ToString(values[SAMPLE_NAME]);
            }

            if(values.Contains(LOCATION)) {
                model.Location = Convert.ToString(values[LOCATION]);
            }

            if(values.Contains(CUSTOMER)) {
                model.Customer = Convert.ToString(values[CUSTOMER]);
            }

            if(values.Contains(OBSERVACIONES)) {
                model.Observaciones = Convert.ToString(values[OBSERVACIONES]);
            }

            if(values.Contains(ID_PLANTA)) {
                model.IdPlanta = Convert.ToInt32(values[ID_PLANTA]);
            }

            if (values.Contains(ID_GRADO))
            {
                model.IdGrado = Convert.ToInt32(values[ID_GRADO]);
            }

            if (values.Contains(CONTEO_DE_PUNTOS))
            {
                model.Observaciones = Convert.ToString(values[CONTEO_DE_PUNTOS]);
            }
        }

        private string GetFullErrorMessage(ModelStateDictionary modelState) {
            var messages = new List<string>();

            foreach(var entry in modelState) {
                foreach(var error in entry.Value.Errors)
                    messages.Add(error.ErrorMessage);
            }

            return String.Join(" ", messages);
        }
    }
}