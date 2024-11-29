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
           return View(await _context.Empleados.ToListAsync());
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


        public IActionResult  Create()
        {

            
            var departamentos = getDepartamentos();

            var roles = _context.Roles.ToList();


            var viewModel = new CreateEmpleadoViewModel
            {
                Departamentos =  departamentos,
                Roles = new SelectList(roles, "RolID", "Name"),
                
            };

            return View(viewModel);
        }

        // POST: Empleado/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Apellido,FechaDeNacimineto,RolID,DepartamentoID,Salario")] Empleado empleado)
        {
            if (ModelState.IsValid)
            {
                _context.Add(empleado);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(empleado);
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