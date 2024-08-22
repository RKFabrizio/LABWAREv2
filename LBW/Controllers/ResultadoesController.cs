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

namespace LBW.Controllers
{
    [Route("api/[controller]/[action]")]
    public class ResultadoesController : Controller
    {
        private LbwContext _context;

        public ResultadoesController(LbwContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var resultados = _context.Resultados.Select(i => new {
                i.IdResult,
                i.IdSample,
                i.IdUnidad,
                i.IdComponent,
                i.IdAnalysis,
                i.SampleNumber,
                i.ResultNumber,
                i.OrderNum,
                i.AnalysisData,
                i.NameComponent,
                i.ReportedName,
                i.Status,
                i.Reportable,
                i.ChangedOn,
                i.Instrument,
                i.IdLista
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdResult" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(resultados, loadOptions));
        }

        public async Task<IActionResult> GetIdSample(int id, DataSourceLoadOptions loadOptions)
        {
            var resultados = _context.Resultados
                .Where(m => m.IdSample == id)
                .Select(i => new {
                i.IdResult,
                i.IdSample,
                i.IdUnidad,
                i.IdComponent,
                i.IdAnalysis,
                i.SampleNumber,
                i.ResultNumber,
                i.OrderNum,
                i.AnalysisData,
                i.NameComponent,
                i.ReportedName,
                i.Status,
                i.Reportable,
                i.ChangedOn,
                i.Instrument,
                i.IdLista
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdResult" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(resultados, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(Resultado model, List<int> Muestra, List<int> Analisis)
        {
            Console.WriteLine("Lista de An�lisis:");
            foreach (var analisis in Analisis)
            {
                Console.WriteLine(analisis);
            }

            try
            {

                if (Muestra == null || Analisis == null || !Muestra.Any() || !Analisis.Any())
                    return BadRequest("Se requieren muestras y an�lisis.");

                var nuevosResultados = new List<Resultado>();

                var analisisNames = _context.Analisiss
                    .Where(a => Analisis.Contains(a.IdAnalisis))
                    .Select(a => a.NameAnalisis)
                    .ToList();

                var analisisMuestraString = string.Join(", ", analisisNames);



                foreach (var idMuestra in Muestra)
                {
                    var muestra = await _context.Muestras.FindAsync(idMuestra);
                   

                    foreach (var idAnalisis in Analisis)
                    {
                        var detallesAnalisis = _context.AnalisisDetalles
                            .Where(ad => ad.IdAnalisis == idAnalisis)
                            .GroupBy(ad => ad.NameComponent)
                            .Select(g => g.OrderByDescending(ad => ad.Version).FirstOrDefault())
                            .ToList();

                        foreach (var detalle in detallesAnalisis)
                        {
                            var existeResultado = _context.Resultados
                                .Any(r => r.IdSample == idMuestra && r.IdAnalysis == idAnalisis && r.IdComponent == detalle.IdComp);

                            if (!existeResultado)
                            {
                                var nuevoResultado = new Resultado
                                {
                                    IdSample = idMuestra,
                                    IdAnalysis = idAnalisis,
                                    IdComponent = detalle.IdComp,
                                    SampleNumber = model.SampleNumber,
                                    ResultNumber = model.ResultNumber,
                                    OrderNum = model.OrderNum,
                                    IdUnidad = detalle.IdUnidad,
                                    AnalysisData = _context.Analisiss.Find(idAnalisis)?.NameAnalisis,
                                    NameComponent = detalle.NameComponent,
                                    ReportedName = detalle.NameComponent,
                                    Reportable = detalle.Reportable,
                                    Status = 254,
                                    ChangedOn = DateTime.Now,
                                    Instrument = 1,
                                    Auditoria = false
                                };
                                nuevosResultados.Add(nuevoResultado);
                            }
                        }
                    }
                }

                _context.Resultados.AddRange(nuevosResultados);
                await _context.SaveChangesAsync();
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }


        [HttpPost]
        public IActionResult PostTest([FromBody] IdPmListModel model)
        {
            try
            {
                string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
                LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

                var ultimoProyecto = _context.Proyectos
                   .Where(cl => cl.Owner == usuario.IdUser)
                   .OrderByDescending(cl => cl.IdProyecto)
                   .FirstOrDefault();

                int idProyecto = ultimoProyecto.IdProyecto;

                Console.WriteLine($"ID del proyecto: {idProyecto}");

                // Obtener las muestras basadas en IdPm e IdProject
                var muestras = _context.Muestras
                    .Where(m => m.IdProject == idProyecto && model.idPmList.Contains(m.IdPm))
                    .ToList();

                List<int> idMuestras = muestras.Select(m => m.IdSample).ToList();

                var plantillas = _context.Plantillas
                   .Where(cl => cl.IdCliente == usuario.CCliente)
                   .FirstOrDefault();

                int idTl = plantillas.IdTL;

                var plantillaDetalle = _context.PlantillaDetalles
                .Where(cl => cl.Id_TL == idTl)
                .ToList();

                List<int> idAnalisiss = plantillaDetalle.Select(detalle => detalle.Id_Analysis).ToList();

                Console.WriteLine("-------------------------SI FUNCIONAAAAA");
                var nuevosResultados = new List<Resultado>();

                foreach (int muestra in idMuestras)
                {
                    foreach (int idAnalisis in idAnalisiss)
                    {
                        var detallesAnalisis = _context.AnalisisDetalles
                            .Where(ad => ad.IdAnalisis == idAnalisis)
                            .GroupBy(ad => ad.NameComponent)
                            .Select(g => g.OrderByDescending(ad => ad.Version).FirstOrDefault())
                            .ToList();

                        foreach (var detalle in detallesAnalisis)
                        {
                            var existeResultado = _context.Resultados
                                .Any(r => r.IdSample == muestra && r.IdAnalysis == idAnalisis && r.IdComponent == detalle.IdComp);

                            if (!existeResultado)
                            {
                                var nuevoResultado = new Resultado
                                {
                                    IdSample = muestra,
                                    IdAnalysis = idAnalisis,
                                    IdComponent = detalle.IdComp,
                                    SampleNumber = null,
                                    ResultNumber = null,
                                    OrderNum = null,
                                    IdUnidad = detalle.IdUnidad,
                                    AnalysisData = _context.Analisiss.Find(idAnalisis)?.NameAnalisis,
                                    NameComponent = detalle.NameComponent,
                                    ReportedName = detalle.NameComponent,
                                    Reportable = detalle.Reportable,
                                    Status = 254,
                                    ChangedOn = DateTime.Now,
                                    Instrument = 1
                                };
                                nuevosResultados.Add(nuevoResultado);
                            }
                        }
                    }
                }

                try
                {
                    Console.WriteLine("--------SIIII----------------------");
                    _context.Resultados.AddRange(nuevosResultados);
                    _context.SaveChanges();
                    Console.WriteLine("------------------------------");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                    return Json(new { success = false, error = "Error al guardar los resultados: " + e.Message });
                }

                return Json(new { success = true });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
                return Json(new { success = false, error = "Error general: " + error.Message });
            }
        }

        public class IdPmListModel
        {
            public List<int> idPmList { get; set; }
        }

        [HttpPut]
        public async Task<IActionResult> Put0(int key, string values)
        {
            var model = await _context.Resultados.FirstOrDefaultAsync(item => item.IdResult == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpPut]
        public async Task<IActionResult> Put(int key, string values)
        {
            var model = await _context.Resultados.FirstOrDefaultAsync(item => item.IdResult == key);
            if (model == null)
                return StatusCode(409, "Object not found");

            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if (!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            string usuarioInfoJson = HttpContext.Request.Cookies["UsuarioInfo"];
            LBW.Models.Usuario usuario = JsonConvert.DeserializeObject<LBW.Models.Usuario>(usuarioInfoJson);

            if (model.ResultNumber == null)
            {
                model.Status = 254; 
            }
            else if(model.ResultNumber != null && model.Status == 799)
            {
                model.Status = 799;
            }
            else
            {
                model.Status = 26;
            }

            model.Login = usuario.IdUser;

            int idResult = model.IdResult;
            int idMuestra = model.IdSample;

            await _context.SaveChangesAsync();

            // Actualizar el estado de la muestra seg�n los criterios mencionados
            await UpdateMuestraStatus(idMuestra);

            return Json(new { message = $"Resultados fueron creados correctamente" });
        }

        private async Task UpdateMuestraStatus(int idMuestra)
        {
            var muestra = await _context.Muestras.FirstOrDefaultAsync(m => m.IdSample == idMuestra);
            if (muestra == null)
                return; // Manejar error, muestra no encontrada

            // Verificar si todos los Resultados de esta muestra tienen un valor
            var resultadosSinValor = await _context.Resultados
                .Where(r => r.IdSample == idMuestra && r.ResultNumber == null)
                .CountAsync();

            int idProject = muestra.IdProject.Value;

            if (muestra.Status == 799)
            {
                // No hacer nada si el estado es 799
                return;
            }
            else if (resultadosSinValor == 0)
            {
                // Todos los Resultados tienen un valor, actualizar el estado a 24
                muestra.Status = 24;
                await _context.SaveChangesAsync();
                await UpdateProyectoStatus(idProject);
            }
            else if (muestra.Status == 24)
            {
                // Mantener el estado en 24 si ya estaba en 24
                muestra.Status = 24;
            }
            else
            {
                // Algunos Resultados a�n no tienen un valor, actualizar el estado a 26
                muestra.Status = 26;
                await _context.SaveChangesAsync();
            }

            await _context.SaveChangesAsync();
        }

        private async Task UpdateProyectoStatus(int idProyecto)
        {
            var proyecto = await _context.Proyectos.FirstOrDefaultAsync(p => p.IdProyecto == idProyecto);
            if (proyecto == null)
                return; // Manejar error, proyecto no encontrado

            var muestrasCompletadas = await _context.Muestras
                .Where(m => m.IdProject == idProyecto)
                .CountAsync();

            var muestrasTotal = await _context.Muestras
                .Where(m => m.IdProject == idProyecto)
                .CountAsync();

            if (muestrasCompletadas == muestrasTotal)
            {
                // Todas las muestras del proyecto est�n completadas
                proyecto.Status = 24;
            }
            else
            {
                // Algunas muestras del proyecto est�n completadas
                proyecto.Status = 26;
            }
 

            await _context.SaveChangesAsync();
        }

        [HttpDelete]
        public async Task Delete(int key) {
            var model = await _context.Resultados.FirstOrDefaultAsync(item => item.IdResult == key);

            _context.Resultados.Remove(model);
            await _context.SaveChangesAsync();
        }


        [HttpGet]
        public async Task<IActionResult> ListasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Listas
                         orderby i.List
                         select new {
                             Value = i.IdLista,
                             Text = i.List
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> ListasLookup1(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Listas
                         orderby i.IdLista
                         where i.IdLista >= 28 && i.IdLista <= 32
                         select new
                         {
                             Value = i.IdLista,
                             Text = i.Value
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }


        [HttpGet]
        public async Task<IActionResult> InstrumentosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Instrumentos
                         orderby i.IdCodigo
                         select new {
                             Value = i.IdInstrumento,
                             Text = i.Nombre
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> MuestrasLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Muestras
                         orderby i.IdSample
                         select new {
                             Value = i.IdSample,
                             Text = i.TextID
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> AnalisissLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Analisiss
                         orderby i.NameAnalisis
                         select new {
                             Value = i.IdAnalisis,
                             Text = i.NameAnalisis
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> UnidadesLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Unidades
                         orderby i.NameUnidad
                         select new {
                             Value = i.IdUnidad,
                             Text = i.NameUnidad
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> UnidadesStringLookup(DataSourceLoadOptions loadOptions)
        {
            var lookup = from i in _context.Unidades
                         orderby i.DisplayString
                         select new
                         {
                             Value = i.IdUnidad,
                             Text = i.DisplayString
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Resultado model, IDictionary values) {
            string ID_RESULT = nameof(Resultado.IdResult);
            string ID_SAMPLE = nameof(Resultado.IdSample);
            string ID_UNIDAD = nameof(Resultado.IdUnidad);
            string ID_COMPONENT = nameof(Resultado.IdComponent);
            string ID_ANALYSIS = nameof(Resultado.IdAnalysis);
            string SAMPLE_NUMBER = nameof(Resultado.SampleNumber);
            string RESULT_NUMBER = nameof(Resultado.ResultNumber);
            string ORDER_NUM = nameof(Resultado.OrderNum);
            string ANALYSIS_DATA = nameof(Resultado.AnalysisData);
            string NAME_COMPONENT = nameof(Resultado.NameComponent);
            string REPORTED_NAME = nameof(Resultado.ReportedName);
            string STATUS = nameof(Resultado.Status);
            string REPORTABLE = nameof(Resultado.Reportable);
            string CHANGED_ON = nameof(Resultado.ChangedOn);
            string INSTRUMENT = nameof(Resultado.Instrument);
            string ID_LISTA = nameof(Resultado.IdLista);

            if (values.Contains(ID_RESULT)) {
                model.IdResult = Convert.ToInt32(values[ID_RESULT]);
            }

            if(values.Contains(ID_SAMPLE)) {
                model.IdSample = Convert.ToInt32(values[ID_SAMPLE]);
            }

            if(values.Contains(ID_UNIDAD)) {
                model.IdUnidad = Convert.ToInt32(values[ID_UNIDAD]);
            }

            if(values.Contains(ID_COMPONENT)) {
                model.IdComponent = Convert.ToInt32(values[ID_COMPONENT]);
            }

            if(values.Contains(ID_ANALYSIS)) {
                model.IdAnalysis = Convert.ToInt32(values[ID_ANALYSIS]);
            }

            if(values.Contains(SAMPLE_NUMBER)) {
                model.SampleNumber = Convert.ToString(values[SAMPLE_NUMBER]);
            }

            if(values.Contains(RESULT_NUMBER)) {
                model.ResultNumber = values[RESULT_NUMBER] != null ? Convert.ToDecimal(values[RESULT_NUMBER], CultureInfo.InvariantCulture) : (decimal?)null;
            }

            if(values.Contains(ORDER_NUM)) {
                model.OrderNum = values[ORDER_NUM] != null ? Convert.ToInt32(values[ORDER_NUM]) : (int?)null;
            }

            if(values.Contains(ANALYSIS_DATA)) {
                model.AnalysisData = Convert.ToString(values[ANALYSIS_DATA]);
            }

            if(values.Contains(NAME_COMPONENT)) {
                model.NameComponent = Convert.ToString(values[NAME_COMPONENT]);
            }

            if(values.Contains(REPORTED_NAME)) {
                model.ReportedName = Convert.ToString(values[REPORTED_NAME]);
            }

            if(values.Contains(STATUS)) {
                model.Status = values[STATUS] != null ? Convert.ToInt32(values[STATUS]) : (int?)null;
            }

            if(values.Contains(REPORTABLE)) {
                model.Reportable = Convert.ToBoolean(values[REPORTABLE]);
            }

            if (values.Contains(ID_LISTA))
            {
                model.IdLista = Convert.ToInt32(values[ID_LISTA]);
            }

            if (values.Contains(CHANGED_ON)) {
                model.ChangedOn = values[CHANGED_ON] != null ? Convert.ToDateTime(values[CHANGED_ON]) : (DateTime?)null;
            }

            if(values.Contains(INSTRUMENT)) {
                model.Instrument = values[INSTRUMENT] != null ? Convert.ToInt32(values[INSTRUMENT]) : (int?)null;
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