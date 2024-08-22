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
    public class AuditoriaResultadoesController : Controller
    {
        private LbwContext _context;

        public AuditoriaResultadoesController(LbwContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions)
        {
            var auditoriaresultados = _context.AuditoriaResultados
                .Select(i => new {
                    i.Id,
                    i.Fecha,
                    i.Fecha_N,
                    i.Analisis,
                    i.Muestra,
                    i.OldValue,
                    i.NewValue,
                    i.Login,
                    i.Cliente,
                    i.Proyecto,
                    i.IdLista
                })
                .OrderByDescending(i => i.Fecha); // Ordenar por Fecha_N descendente

            // Si los datos subyacentes son una tabla SQL grande, especifique PrimaryKey y PaginateViaPrimaryKey.
            // Esto puede hacer que los planes de ejecución de SQL sean más eficientes.
            // loadOptions.PrimaryKey = new[] { "Id" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(auditoriaresultados, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new AuditoriaResultado();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.AuditoriaResultados.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.Id });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.AuditoriaResultados.FirstOrDefaultAsync(item => item.Id == key);
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
            var model = await _context.AuditoriaResultados.FirstOrDefaultAsync(item => item.Id == key);

            _context.AuditoriaResultados.Remove(model);
            await _context.SaveChangesAsync();
        }

         
        private void PopulateModel(AuditoriaResultado model, IDictionary values) {
            string ID = nameof(AuditoriaResultado.Id);
            string FECHA = nameof(AuditoriaResultado.Fecha);
            string FECHA_N = nameof(AuditoriaResultado.Fecha_N);
            string ANALISIS = nameof(AuditoriaResultado.Analisis);
            string MUESTRA = nameof(AuditoriaResultado.Muestra);
            string OLD_VALUE = nameof(AuditoriaResultado.OldValue);
            string NEW_VALUE = nameof(AuditoriaResultado.NewValue);
            string LOGIN = nameof(AuditoriaResultado.Login);
            string CLIENTE = nameof(AuditoriaResultado.Cliente);
            string PROYECTO = nameof(AuditoriaResultado.Proyecto);
            string ID_LISTA = nameof(Resultado.IdLista);

            if (values.Contains(ID)) {
                model.Id = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(FECHA)) {
                model.Fecha = values[FECHA] != null ? Convert.ToDateTime(values[FECHA]) : (DateTime?)null;
            }

            if(values.Contains(FECHA_N)) {
                model.Fecha_N = Convert.ToDateTime(values[FECHA_N]);
            }

            if(values.Contains(ANALISIS)) {
                model.Analisis = Convert.ToString(values[ANALISIS]);
            }

            if(values.Contains(MUESTRA)) {
                model.Muestra = Convert.ToString(values[MUESTRA]);
            }

            if(values.Contains(OLD_VALUE)) {
                model.OldValue = Convert.ToString(values[OLD_VALUE]);
            }

            if(values.Contains(NEW_VALUE)) {
                model.NewValue = Convert.ToString(values[NEW_VALUE]);
            }

            if(values.Contains(LOGIN)) {
                model.Login = values[LOGIN] != null ? Convert.ToInt32(values[LOGIN]) : (int?)null;
            }

            if(values.Contains(CLIENTE)) {
                model.Cliente = Convert.ToString(values[CLIENTE]);
            }

            if(values.Contains(PROYECTO)) {
                model.Proyecto = Convert.ToString(values[PROYECTO]);
            }
            if (values.Contains(ID_LISTA))
            {
                model.IdLista = Convert.ToInt32(values[ID_LISTA]);
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