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



namespace test2.Controllers
{

    public class DepartamentosController : Controller
    {
        private readonly ILogger<RolesController> _logger;

        private readonly ApplicationDbContext _context;

        public DepartamentosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
        // Acción para cargar productos según la categoría seleccionada
        public async Task<JsonResult> get()
        {
            var roles = await _context.Departamentos.ToListAsync();
            return Json(roles);
        }

        
        
        public async Task<IActionResult> Create([FromBody] Departamento departamento)
        {
            /* if (id != empleado.Id)
            {
                return NotFound();
            } */
          
                 if(departamento!=null){
                    try
                    {
                    if(DepartamentoExists(departamento.Name)) throw new Exception("este departamento ya existe");
                    _context.Add(departamento);
                    _context.SaveChanges();
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
                catch (DbUpdateException ex)
                {
                    // Manejar la excepción de actualización en la base de datos (ej. errores de clave primaria o violación de restricciones)
                    // Log o maneja el error específico
                   // ModelState.AddModelError("", "Hubo un problema al guardar los datos en la base de datos. Intenta nuevamente.");
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
            
        
            return Json(new { status="error", message="validation error",data=departamento});
       
        }

        private bool DepartamentoExists(string name)
        {
            return _context.Departamentos.Any(e => e.Name == name);
        }





        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}