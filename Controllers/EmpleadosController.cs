using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using test2.Data;
using test2.Models;
using test2.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data.Entity.Validation;



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


        // GET: Empleado
        public async Task<IActionResult> Index()
        {
            var empleados = await _context.Empleados.ToListAsync();

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
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,FechaDeNacimiento,PosicionID,DepartamentoID,Salario")] Empleado empleado)
        {

            _logger.LogInformation("{q}",ModelState.IsValid);

            if (ModelState.IsValid)
            {
                try{

                    // Asegurarse de que la fecha esté en UTC
                    if (empleado.FechaDeNacimiento.Kind == DateTimeKind.Unspecified)
                    {
                        empleado.FechaDeNacimiento = DateTime.SpecifyKind(empleado.FechaDeNacimiento, DateTimeKind.Utc);
                    }
                    _context.Add(empleado);
                    await _context.SaveChangesAsync();
                
                    return  RedirectToAction(nameof(Index));

                } catch (DbEntityValidationException ex)
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
               
                    _logger.LogInformation("{S}",ex.Message);
                }
                catch (SqlException ex)
                {
                    // Manejar errores de SQL (conexión fallida, sintaxis errónea, etc.)
                   // ModelState.AddModelError("", "Error al conectarse a la base de datos. Por favor, intenta más tarde.");
                }
                catch (Exception ex)
                {
                    Dictionary<string, string> diccionario = new Dictionary<string, string>();
                    diccionario.Add("name","este  departamento ya existe");
                    string json = JsonConvert.SerializeObject(diccionario);
                    return Json(new{ status="error",message="validation error", error=json });
                    // Manejo genérico de excepciones
                  //  ModelState.AddModelError("", "Ocurrió un error inesperado: " + ex.Message);
                }
               

               
            }


             foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
    {
        // Muestra todos los errores
        Console.WriteLine($"Error: {error.ErrorMessage}");
    }
            var departamentos = getDepartamentos();

            var posiciones = _context.Posiciones.ToList();

            var viewModel = new CreateEmpleadoViewModel
            {   
                Empleado=empleado,
                Departamentos = departamentos,
                Posiciones = posiciones

            };
            return View(viewModel);
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