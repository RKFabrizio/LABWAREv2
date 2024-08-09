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
    public class SmtpsController : Controller
    {
        private LbwContext _context;

        public SmtpsController(LbwContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get(DataSourceLoadOptions loadOptions) {
            var smpts = _context.Smpts.Select(i => new {
                i.ID,
                i.Correo,
                i.Host,
                i.Puerto,
                i.Usuario,
                i.Contrasena,
                i.Body
            });

            // If underlying data is a large SQL table, specify PrimaryKey and PaginateViaPrimaryKey.
            // This can make SQL execution plans more efficient.
            // For more detailed information, please refer to this discussion: https://github.com/DevExpress/DevExtreme.AspNet.Data/issues/336.
            // loadOptions.PrimaryKey = new[] { "ID" };
            // loadOptions.PaginateViaPrimaryKey = true;

            return Json(await DataSourceLoader.LoadAsync(smpts, loadOptions));
        }

        [HttpPost]
        public async Task<IActionResult> Post(string values) {
            var model = new Smtp();
            var valuesDict = JsonConvert.DeserializeObject<IDictionary>(values);
            PopulateModel(model, valuesDict);

            if(!TryValidateModel(model))
                return BadRequest(GetFullErrorMessage(ModelState));

            var result = _context.Smpts.Add(model);
            await _context.SaveChangesAsync();

            return Json(new { result.Entity.ID });
        }

        [HttpPut]
        public async Task<IActionResult> Put(int key, string values) {
            var model = await _context.Smpts.FirstOrDefaultAsync(item => item.ID == key);
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
            var model = await _context.Smpts.FirstOrDefaultAsync(item => item.ID == key);

            _context.Smpts.Remove(model);
            await _context.SaveChangesAsync();
        }


        private void PopulateModel(Smtp model, IDictionary values) {
            string ID = nameof(Smtp.ID);
            string CORREO = nameof(Smtp.Correo);
            string HOST = nameof(Smtp.Host);
            string PUERTO = nameof(Smtp.Puerto);
            string USUARIO = nameof(Smtp.Usuario);
            string CONTRASENA = nameof(Smtp.Contrasena);
            string BODY = nameof(Smtp.Body);

            if (values.Contains(ID)) {
                model.ID = Convert.ToInt32(values[ID]);
            }

            if(values.Contains(CORREO)) {
                model.Correo = Convert.ToString(values[CORREO]);
            }

            if(values.Contains(HOST)) {
                model.Host = Convert.ToString(values[HOST]);
            }

            if(values.Contains(PUERTO)) {
                model.Puerto = Convert.ToInt32(values[PUERTO]);
            }

            if(values.Contains(USUARIO)) {
                model.Usuario = Convert.ToString(values[USUARIO]);
            }

            if(values.Contains(CONTRASENA)) {
                model.Contrasena = Convert.ToString(values[CONTRASENA]);
            }

            if (values.Contains(BODY))
            {
                model.Body = Convert.ToString(values[BODY]);
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