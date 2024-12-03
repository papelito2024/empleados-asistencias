using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;
using test2.Data;
using test2.Models;
using test2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.Entity.Validation;
using System.Collections;
using System.ComponentModel.DataAnnotations;



namespace test2.Controllers
{

    public class EmpleadosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<EmpleadosController> _logger;

      
        public EmpleadosController(ApplicationDbContext context, ILogger<EmpleadosController> logger)
        {
            _logger = logger;
            _context = context;
        }



        /*   private async Task<IEnumerable<object>>  PaginateAndFilter(int pageNumber,int pageSize)
          {

              var Data = await (from empleado in _context.Empleados
                                       join departamento in _context.Departamentos
                                       on empleado.DepartamentoID equals departamento.Id
                                       join posicion in _context.Posiciones
                                       on empleado.PosicionID equals posicion.Id
                                      /*  where category.Name.Contains(categoryName)  // Filtro por categoría
                                       orderby product.Name  // Ordenar por nombre de producto 
                                       select new
                                       {
                                          empleado.Nombre,
                                          empleado.Apellido,
                                          empleado.FechaDeNacimiento,
                                          departamento=departamento.Name,
                                          posicion=posicion.Name,

                                       })
                            .Skip((pageNumber - 1) * pageSize)  // Paginación: saltar elementos de páginas anteriores
                            .Take(pageSize)  // Tomar los elementos de la página actual
                            .ToListAsync();  // Ejecutar la consulta de manera asíncrona


              return Data;
          }
   */

        public async Task<PaginacionViewModel<Empleado>> PaginateAndFilter(int pageNumber, int pageSize)
        {
            // Create the base query to fetch the employees
            var query = from empleado in _context.Empleados
                        join departamento in _context.Departamentos
                            on empleado.DepartamentoId equals departamento.Id
                        join posicion in _context.Posiciones
                            on empleado.PosicionId equals posicion.Id
                        select new Empleado
                        {
                            Nombre = empleado.Nombre,
                            Apellido = empleado.Apellido,
                            FechaDeNacimiento = empleado.FechaDeNacimiento,
                            Departamento = new Departamento {
                                Id=departamento.Id,
                                Name=departamento.Name
                            },
                            Posicion = new Posicion{
                                Id = posicion.Id,
                                Name=posicion.Name,
                            }
                        };

            // Total count before pagination (for calculating the total number of pages)
            var totalCount =  await query.CountAsync();

            // Apply pagination: skip previous pages and take the current page's items
            var items = await query
                .Skip((pageNumber - 1) * pageSize)  // Skip elements from previous pages
                .Take(pageSize)  // Take elements for the current page
                .ToListAsync();  // Execute the query asynchronously

            // Construct and return the PaginacionViewModel with pagination details
            return new PaginacionViewModel<Empleado>
            {
                Items = items,
                TotalItems = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };
        }
        // GET: Empleado
        public async Task<IActionResult> Index(int page=1)
        {
           var empleados = await PaginateAndFilter(page,5);

            var totalItems = _context.Empleados.Count();

          

            return View(empleados);
        }

        // GET: Empleado/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        [HttpGet]
        public IActionResult  Create()
        {

            
            var departamentos = getDepartamentos();

            var posiciones= _context.Posiciones.ToList();

            string json = JsonConvert.SerializeObject(departamentos);

            _logger.LogInformation("{departamentos}",departamentos);
            
            var viewModel = new CreateEmpleadoViewModel
            {
                Departamentos =  departamentos,
                Posiciones = posiciones
                
            };

            return View(viewModel);
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Empleado empleado)
        {

            if (!ModelState.IsValid)
            {

                string e = JsonConvert.SerializeObject(empleado);
                _logger.LogInformation(" {s}, ", e);
       
                // Asegurarse de que la fecha esté en UTC
                if (empleado.FechaDeNacimiento.Kind == DateTimeKind.Unspecified){
                        empleado.FechaDeNacimiento = DateTime.SpecifyKind(empleado.FechaDeNacimiento, DateTimeKind.Utc);
                    }
                try
                {
                    _context.Add(empleado);
                    Console.WriteLine(_context.SaveChanges());

                      TempData["success"] = "Empleado registrado exitosamente";

                return RedirectToAction("index");
                }
                catch (DbUpdateException ex)
                {
                    // Manejar errores de actualización en la base de datos (violación de clave, etc.)
                    Console.WriteLine($"Error en la base de datos: {ex.Message}");
                }
               
                catch (ValidationException ex)
                {
                    // Manejar errores de validación
                    Console.WriteLine($"Error de validación: {ex.Message}");
                }
                catch (Exception ex)
                {
                    // Captura cualquier otra excepción general
                    Console.WriteLine($"Error inesperado: {ex.Message}");
                }



                var departamentos = getDepartamentos();

                    var posiciones = _context.Posiciones.ToList();

                    var viewModel = new CreateEmpleadoViewModel
                    {
                        Empleado = empleado,
                        Departamentos = departamentos,
                        Posiciones = posiciones

                    };
                    return View(viewModel);

                
               
            }else{
                var departamentos = getDepartamentos();

                var posiciones = _context.Posiciones.ToList();

                var viewModel = new CreateEmpleadoViewModel
                {
                    Empleado = empleado,
                    Departamentos = departamentos,
                    Posiciones = posiciones

                };
                return View(viewModel);
            }
            

 
           
        }

        private List<Departamento> getDepartamentos(){

            List<Departamento> departamentos;
            
            try
            {
             departamentos = _context.Departamentos.ToList();

             if(departamentos != null)return departamentos;

            
               departamentos.Add(new Departamento{
                    Id=1,
                    Name="chango"
                });

                 return  departamentos;
            }
            
            catch (SqlException ex)
            {
                _logger.LogInformation("Se han recuperado {EmpleadoCount} empleados desde la base de datos.", 2);

                return null; 
            } 
            catch (Exception ex)
            {
                _logger.LogInformation("Se han recuperado {EmpleadoCount} empleados desde la base de datos.", 2);
                return null;
            }

            return null;           
        }
        // GET: Empleado/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado == null)
            {
                return NotFound();
            }
            return View(empleado);
        }


        // POST: Empleado/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Cargo,Salario")] Empleado empleado)
        {
            if (id != empleado.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(empleado);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmpleadoExists(empleado.Id))
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
            return View(empleado);
        }

        // GET: Empleado/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var empleado = await _context.Empleados
                .FirstOrDefaultAsync(m => m.Id == id);
            if (empleado == null)
            {
                return NotFound();
            }

            return View(empleado);
        }

        // POST: Empleado/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var empleado = await _context.Empleados.FindAsync(id);
            if (empleado != null)
            {
                _context.Empleados.Remove(empleado);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool EmpleadoExists(int id)
        {
            return _context.Empleados.Any(e => e.Id == id);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }

    internal class CreateEmpleadosViewModel
    {
        public SelectList Departamentos { get; set; }
        public SelectList Roles { get; set; }
    }
}