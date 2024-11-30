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
using test2.Models.ViewModels;



namespace test2.Controllers
{
  
    public class PosicionesController : Controller
    {
        private readonly ILogger<PosicionesController> _logger;

        private readonly ApplicationDbContext _context;

        public PosicionesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Index(int page = 1)
        {
            // Calcular el total de elementos
            var totalItems = _context.Posiciones.Count();

            //page size
            int PageSize = 5;
            // Calcular los elementos a saltar para la paginación
            var skip = (page - 1) * PageSize;

            // Obtener los elementos de la página actual
            var posiciones = _context.Posiciones
                                         .Skip(skip)
                                         .Take(PageSize)
                                         .ToList();

            //var departamentos = _context.Departamentos.ToList();


            var model = new PaginacionViewModel<Posicion>
            {
                Items = posiciones,
                TotalItems = totalItems,
                PageSize = PageSize,
                CurrentPage = page
            };

            return View(model);
        }



        // Acción para cargar productos según la categoría seleccionada
        public async Task<JsonResult> get()
        {
            var posiciones = await  _context.Posiciones.ToListAsync();
            
            return Json(posiciones);
        }

        [HttpGet]
         public IActionResult Create(){


            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Posicion posicion)
        {
            /* if (id != empleado.Id)
            {
                return NotFound();
            } */

            if (posicion != null)
            {
                try
                {

                    _context.Add(posicion);
                    _context.SaveChanges();
                    return Json(new { status = "success", message = "possicion created successfully", data = posicion });


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
                catch (DbUpdateException ex)
                {
                    // Manejar la excepción de actualización en la base de datos (ej. errores de clave primaria o violación de restricciones)
                    // Log o maneja el error específico
                    // ModelState.AddModelError("", "Hubo un problema al guardar los datos en la base de datos. Intenta nuevamente.");
                    _logger.LogInformation("{info}",ex.Message);
                }
                catch (SqlException ex)
                {
                    _logger.LogInformation("{info}", ex.Message);
                    // Manejar errores de SQL (conexión fallida, sintaxis errónea, etc.)
                    // ModelState.AddModelError("", "Error al conectarse a la base de datos. Por favor, intenta más tarde.");
                }
                catch (Exception ex)
                {
                    _logger.LogInformation("{info}", ex.Message);
                    // Manejo genérico de excepciones
                    //  ModelState.AddModelError("", "Ocurrió un error inesperado: " + ex.Message);
                }

            }


            return Json(new { status = "error", message = "validation error", data = posicion });

        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones.FindAsync(id);
            if (posicion == null)
            {
                return NotFound();
            }
            return View(posicion);
        }
        
        
        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Posicion posicion)
        {
            if (id != posicion.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(posicion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PosicionExists(posicion))
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
            return View(posicion);
        }


        private bool PosicionExists(Posicion posicion){

            if(string.IsNullOrEmpty(posicion.Name)) return _context.Posiciones.Any(e => e.Name == posicion.Name);

            if(posicion.Id!=0) return _context.Posiciones.Any(e => e.Id == posicion.Id);
            

            return false;
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var posicion = await _context.Posiciones.FirstOrDefaultAsync(m => m.Id == id);
           
            if (posicion == null)
            {
                return NotFound();
            }

            return View(posicion);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var posicion = await _context.Posiciones.FindAsync(id);
            if (posicion != null)
            {
                _context.Posiciones.Remove(posicion);
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