using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using LBW.Models.Entity;
using System.Xml.Linq;

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
                i.IdPlanta
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
            Console.WriteLine("dasdadasdasd/-------------------------");
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
                i.IdPlanta
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdSample" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(muestras, loadOptions));
        }

        [HttpGet]
        public IActionResult GetByFilter(DateTime StartDate, DateTime EndDate, int Cliente, int Estado)
        {
            var muestras = _context.Muestras
            .Where(ms => ms.LoginDate >= StartDate && ms.LoginDate <= EndDate && ms.IdCliente == Cliente && ms.Status == Estado)
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
                i.IdPlanta
            });

            return Json(muestras);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Muestra model)
        {
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            // Validar y convertir SampleNumber
            if (string.IsNullOrEmpty(model.SampleNumber) || !int.TryParse(model.SampleNumber, out int sampleCount))
            {
                return BadRequest("SampleNumber is invalid.");
            }

            model.IdCliente = 3;
            model.IdLocation = 8;

            var description = await _context.PuntoMuestras
                .Where(pm => pm.IdPm == model.IdPm)
                .Select(pm => pm.Description)
                .FirstOrDefaultAsync();
            var namePm = await _context.PuntoMuestras
                .Where(pm => pm.IdPm == model.IdPm)
                .Select(pm => pm.NamePm)
                .FirstOrDefaultAsync();
            var location = await _context.Ubicaciones
                .Where(ub => ub.ID_LOCATION == model.IdLocation)
                .Select(ub => ub.Name_location)
                .FirstOrDefaultAsync();
            var cliente = await _context.Clientes
                .Where(cl => cl.IdCliente == model.IdCliente)
                .Select(cl => cl.NameCliente)
                .FirstOrDefaultAsync();

            var ultimoProyecto = await _context.Proyectos
                .Where(cl => cl.Owner == usuario.IdUser)
                .OrderByDescending(cl => cl.DateCreated) // Suponiendo que hay un campo "FechaIngreso" que indica cuándo se ingresó el proyecto
                .FirstOrDefaultAsync();

            model.TextID = $"{description}_{DateTime.Now:yyyyMMdd}";
            model.Status = 254;
            model.ChangedOn = DateTime.Now;
            model.LoginDate = DateTime.Now;
            model.LoginBy = usuario.IdUser;
            model.SamplingPoint = namePm;
            model.IdProject = ultimoProyecto.IdProyecto;
            model.SampleName = description;
            model.SampleDate = null;
            model.RecdDate = null;
            model.ReceivedBy = 403;
            model.DateStarted = null;
            model.DueDate = null;
            model.DateReviewed = null;
            model.PreBy = null;
            model.Reviewer = null;
            model.Location = location;
            model.Customer = cliente;

 

            if (!TryValidateModel(model))
            {
                return BadRequest(GetFullErrorMessage(ModelState));
            }

            // Crear y guardar múltiples instancias del modelo
            for (int i = 0; i < sampleCount; i++)
            {
 
                var newModel = new Muestra
                {
 
                    IdCliente = model.IdCliente,
                    IdLocation = model.IdLocation,
                    IdPm = model.IdPm,
                    TextID = model.TextID,
                    Status = model.Status,
                    ChangedOn = model.ChangedOn,
                    OriginalSample = model.OriginalSample,
                    LoginDate = model.LoginDate,
                    LoginBy = model.LoginBy,
                    SamplingPoint = model.SamplingPoint,
                    IdProject = model.IdProject,
                    SampleName = model.SampleName,
                    SampleDate = model.SampleDate,
                    RecdDate = model.RecdDate,
                    ReceivedBy = model.ReceivedBy,

                    SampleType = model.SampleType,
                    IdPlanta = model.IdPlanta,
                    ConteoPuntos = model.ConteoPuntos,
 
                    DateStarted = model.DateStarted,
                    DueDate = model.DueDate,
                    DateReviewed = model.DateReviewed,
                    PreBy = model.PreBy,
                    Reviewer = model.Reviewer,
                    Location = model.Location,
                    Customer = model.Customer,
                    SampleNumber = sampleCount.ToString()
                };
                Console.WriteLine("----------------------");
                Console.WriteLine(newModel.IdPlanta);
                Console.WriteLine(newModel.SampleType);
                Console.WriteLine(newModel.ConteoPuntos);
                Console.WriteLine("----------------------");

                _context.Muestras.Add(newModel);
                await _context.SaveChangesAsync();
            }

            

            return Json(new { message = $"{sampleCount} muestras fueron creadas correctamente" });
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

        [HttpGet]
        public async Task<IActionResult> ProyectosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Proyectos
                         orderby i.Name
                         select new {
                             Value = i.IdProyecto,
                             Text = i.Name
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ClientesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Clientes
                         orderby i.NameCliente
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
        public IActionResult PmLookUp(int Planta)
        {
            var lookup = from i in _context.PuntoMuestras
                         orderby i.NamePm
                         where i.IdPlanta == Planta
                         select new
                         {
                             Value = i.IdPm,
                             Text = i.NamePm
                         };
 

            return Json(lookup);
        }

        [HttpGet]
        public async Task<IActionResult> PlantasPersonalizadoLookup(DataSourceLoadOptions loadOptions)
        {
            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);
 

            var lookup = from i in _context.Plantas
                         where i.IdCliente == usuario.CCliente
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
                .OrderByDescending(p => p.DateCreated)
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
                .OrderByDescending(m => m.DateStarted)
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
        public async Task<IActionResult> PlantasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Plantas
                         orderby i.NamePl
                         select new {
                             Value = i.IdPlanta,
                             Text = i.NamePl
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
            string CONTEO_DE_PUNTOS = nameof(Muestra.ConteoPuntos);

            if(values.Contains(ID_SAMPLE)) {
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

            if(values.Contains(STATUS)) {
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

            if(values.Contains(RECEIVED_BY)) {
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