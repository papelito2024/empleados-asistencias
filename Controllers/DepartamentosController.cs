using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using test2.Data;
using test2.Models;
using test2.Models.ResponseModels;
using test2.Models.ViewModels;



namespace test2.Controllers
{

    public class DepartamentosController : Controller
    {
        private readonly ILogger<DepartamentosController> _logger;

        private readonly ApplicationDbContext _context;

        public DepartamentosController(ApplicationDbContext context,ILogger<DepartamentosController> logger)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index(int page=1)
        {
            // Calcular el total de elementos
            var totalItems = _context.Departamentos.Count();

            //page size
            int PageSize=5;
            // Calcular los elementos a saltar para la paginación
            var skip = (page - 1) * PageSize;

            // Obtener los elementos de la página actual
            var departamentos = _context.Departamentos
                                         .Skip(skip)
                                         .Take(PageSize)
                                         .ToList();

          

            //var departamentos = _context.Departamentos.ToList();


            var model = new PaginacionViewModel<Departamento>
            {
                Items = departamentos,
                TotalItems = totalItems,
                PageSize = PageSize,
                CurrentPage = page
            };

            return View(model);
        }
        // Acción para cargar productos según la categoría seleccionada
        public async Task<JsonResult> get()
        {
            var roles = await _context.Departamentos.ToListAsync();
            return Json(roles);
        }

        [HttpGet]
        public IActionResult Create(){


            return View();
        }
        


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Departamento departamento)
        {
            
          
                 if(departamento!=null){
                    try
                    {
                        if(DepartamentoExists(departamento.Name)) throw new Exception("este departamento ya existe");
                        _context.Add(departamento);
                        await _context.SaveChangesAsync();
                        return Json(new { status = "success", message = "departamento added", data = departamento });


                    }
                catch (DbEntityValidationException ex)
                {
                    Dictionary<string, string> diccionario = new Dictionary<string, string>();
                    // Manejar la excepción de validación de entidad
                    foreach (var eve in ex.EntityValidationErrors)
                    {
                        foreach (var ve in eve.ValidationErrors)
                        {
                            // Aquí puedes registrar el error, o agregar detalles adicionales
                            diccionario.Add(ve.PropertyName, ve.ErrorMessage);
                           // ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                            
                        }
                    }
                    string json = JsonConvert.SerializeObject(diccionario);

                    return Json(json);
                }
              
                
                var message = ResponseMessages.GetErrorMessage(ResponseMessages.ErrorCode.database,"ERROR");
            
               var response = new ResponseModel{
                        Status="ERROR",
                       
                      
                };
                
                return  Json(response);
           
       
        }

        
            return Json(new {
                status="error",
            });
        }

        private bool DepartamentoExists(string name)
        {
            return _context.Departamentos.Any(e => e.Name == name);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {  

             TempData["ErrorMessage"] = "Debes proporcionar un id  para modificar el departamento";

             return RedirectToAction("index");
            }

            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null)
            {
                TempData["ErrorMessage"] = "Este departamento ya no existe";

                return RedirectToAction("index");
            }
            return View(departamento);
        }
        // POST: Empleado/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Departamento departamento)
        {
            if (id != departamento.Id)
            {
                TempData["ErrorMessage"] = "Debes proporcionar un id  para modificar el departamento";

                return RedirectToAction("index");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departamento);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartamentoExists(departamento.Name))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(departamento);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departamento = await _context.Departamentos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (departamento == null)
            {
                return NotFound();
            }

            return View(departamento);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento != null)
            {
                _context.Departamentos.Remove(departamento);
                await _context.SaveChangesAsync();
            }
                 return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}