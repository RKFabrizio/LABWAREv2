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
    public class AnalisisController : Controller
    {
        private LbwContext _context;

        public AnalisisController(LbwContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var analisiss = _context.Analisiss.Select(i => new {
                i.IdAnalisis,
                i.IdTipoA,
                i.NameAnalisis,
                i.Version,
                i.Active,
                i.CommonName,
                i.Description,
                i.AliasName,
                i.ChangedOn,
                i.ChangedBy
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "IdAnalisis" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(analisiss, loadOptions));
        }

        [HttpGet]
        public async Task<IActionResult> Get1(DataSourceLoadOptions loadOptions)
        {
            var maxVersionQuery = _context.Analisiss
                .GroupBy(i => i.NameAnalisis)
                .Select(g => new
                {
                    NameAnalisis = g.Key,
                    MaxVersion = g.Max(i => i.Version)
                });

            var analisiss = from a in _context.Analisiss
                            join mv in maxVersionQuery on new { a.NameAnalisis, a.Version } equals new { mv.NameAnalisis, Version = mv.MaxVersion }
                            where a.Active == true
                            select new
                            {
                                a.IdAnalisis,
                                a.IdTipoA,
                                a.NameAnalisis,
                                a.Version,
                                a.Active,
                                a.CommonName,
                                a.Description,
                                a.AliasName,
                                a.ChangedOn,
                                a.ChangedBy
                            };

            return Json(await DataSourceLoader.LoadAsync(analisiss, loadOptions));
        }


        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Analisis();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Analisiss.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.IdAnalisis });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Analisiss.FirstOrDefaultAsync(item => item.IdAnalisis == key);
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
            var model = await _context.Analisiss.FirstOrDefaultAsync(item => item.IdAnalisis == key);

            _context.Analisiss.Remove(model);
            await _context.SaveChangesAsync();
        }

        [HttpGet]
        public IActionResult NameAnalisissLookup(DataSourceLoadOptions loadOptions)
        {
            // Obtener los análisis activos con la máxima versión para cada NAME_ANALISIS
            var lookup = (from a in _context.Analisiss
                          where a.Active == true &&
                                _context.AnalisisDetalles.Select(ad => ad.IdAnalisis).Distinct().Contains(a.IdAnalisis) &&
                                a.Version == _context.Analisiss
                                                 .Where(b => b.NameAnalisis == a.NameAnalisis)
                                                 .Max(b => b.Version)
                          select new
                          {
                              Value = a.IdAnalisis,
                              Text = a.NameAnalisis
                          })
                         .OrderBy(x => x.Text)
                         .ToList();

            return Json(DataSourceLoader.Load(lookup, loadOptions));
        }



        [HttpGet]
        public IActionResult NameAnalisis2(DataSourceLoadOptions loadOptions, int Muestra)
        {
            Console.WriteLine(Muestra);
            Console.WriteLine("-------------Awsadasd");

            var idAnalisisExistentes = _context.Resultados
                .Where(r => r.IdSample == Muestra)
                .Select(r => r.IdAnalysis)
                .Distinct()
                .ToList();

            Console.WriteLine(string.Join(", ", idAnalisisExistentes));
            Console.WriteLine("-------------Awsadasd");

            var lookup = from i in _context.Analisiss
                         where idAnalisisExistentes.Contains(i.IdAnalisis)
                         orderby i.NameAnalisis
                         select new
                         {
                             Value = i.IdAnalisis,
                             Text = i.NameAnalisis
                         };

            return Json(lookup);
        }


        [HttpGet]
        public async Task<IActionResult> TipoAnalisissLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.TipoAnalisiss
                         orderby i.NombreA
                         select new {
                             Value = i.IdTipoA,
                             Text = i.NombreA
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

  


        [HttpGet]
        public async Task<IActionResult> UsuariosLookup(DataSourceLoadOptions loadOptions) {
            var lookup = from i in _context.Usuarios
                         orderby i.NombreCompleto
                         select new {
                             Value = i.IdUser,
                             Text = i.NombreCompleto
                         };
            return Json(await DataSourceLoader.LoadAsync(lookup, loadOptions));
        }

        private void PopulateModel(Analisis model, IDictionary values) {
            string ID_ANALISIS = nameof(Analisis.IdAnalisis);
            string ID_TIPO_A = nameof(Analisis.IdTipoA);
            string NAME_ANALISIS = nameof(Analisis.NameAnalisis);
            string VERSION = nameof(Analisis.Version);
            string ACTIVE = nameof(Analisis.Active);
            string COMMON_NAME = nameof(Analisis.CommonName);
            string DESCRIPTION = nameof(Analisis.Description);
            string ALIAS_NAME = nameof(Analisis.AliasName);
            string CHANGED_ON = nameof(Analisis.ChangedOn);
            string CHANGED_BY = nameof(Analisis.ChangedBy);

            if(values.Contains(ID_ANALISIS)) {
                model.IdAnalisis = Convert.ToInt32(values[ID_ANALISIS]);
            }

            if(values.Contains(ID_TIPO_A)) {
                model.IdTipoA = Convert.ToInt32(values[ID_TIPO_A]);
            }

            if(values.Contains(NAME_ANALISIS)) {
                model.NameAnalisis = Convert.ToString(values[NAME_ANALISIS]);
            }

            if(values.Contains(VERSION)) {
                model.Version = values[VERSION] != null ? Convert.ToInt32(values[VERSION]) : (int?)null;
            }

            if(values.Contains(ACTIVE)) {
                model.Active = values[ACTIVE] != null ? Convert.ToBoolean(values[ACTIVE]) : (bool?)null;
            }

            if(values.Contains(COMMON_NAME)) {
                model.CommonName = Convert.ToString(values[COMMON_NAME]);
            }

            if(values.Contains(DESCRIPTION)) {
                model.Description = Convert.ToString(values[DESCRIPTION]);
            }

            if(values.Contains(ALIAS_NAME)) {
                model.AliasName = Convert.ToString(values[ALIAS_NAME]);
            }

            if(values.Contains(CHANGED_ON)) {
                model.ChangedOn = values[CHANGED_ON] != null ? Convert.ToDateTime(values[CHANGED_ON]) : (DateTime?)null;
            }

            if(values.Contains(CHANGED_BY)) {
                model.ChangedBy = values[CHANGED_BY] != null ? Convert.ToInt32(values[CHANGED_BY]) : (int?)null;
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