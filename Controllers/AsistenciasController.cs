using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using test2.Data;
using test2.Models;
using test2.Models.ViewModels;

namespace test2.Controllers
{
    
    public class AsistenciasController : Controller
    {
        private readonly ILogger<AsistenciasController> _logger;
        private readonly ApplicationDbContext _context;


        public AsistenciasController(ApplicationDbContext context,ILogger<AsistenciasController> logger)
        {

            _context = context;
            _logger = logger;
        }

        public IActionResult Index(int? id)
        {

            
            _logger.LogInformation("{}",id);
            if(id==null){

                return View(null);
            }
            
            var empleado = _context.Empleados.Where(u => u.Id==id).FirstOrDefault();
            
            if(empleado==null) {
                
                ViewData["error"] = "No se encontraron resultados";

                return View(new AsistenciasViewModel{
                    Empleado=null,
                    Estado = null
                });
            }

            var asistencia = _context.Asistencias.Where(a=>a.EmpleadoId==id)
            .OrderByDescending(a=>a.Id).FirstOrDefault();

            

            var ViewModel = new AsistenciasViewModel
            {
              Empleado=empleado,
              Estado =asistencia!=null ? asistencia.Estado : "ausente",

            };
            return View(ViewModel);

        }

        /**
        
        //////////////////////
        */

        
        [HttpGet]
        public IActionResult  ToggleAsistencia([FromQuery]int id){


            if(id==null){
                _logger.LogInformation("asdasd12312     {}",id);
                TempData["error"]="Debes propocionar el id del empleado";
                return RedirectToAction("index", "Asistencias", id);
            }

            

            var asistencia  = _context.Asistencias
            .Where(a=>a.EmpleadoId==id)
            .OrderByDescending(e => e.Id)
            .FirstOrDefault();


            if(asistencia==null){

                _logger.LogInformation("err     {}", id);

                var empleado = _context.Empleados.Where(e=>e.Id==id).FirstOrDefault();
                
                if(empleado == null) {
                    _logger.LogInformation("err     {}", id);
                    TempData["error"] = "Este empleado no existe en la nomina";
                    return RedirectToAction("index", "Asistencias", new {id=id});
                }
                     
            }



            Asistencia newAsistencia = new Asistencia
            {
                Estado = "asistido",
                EmpleadoId = id,

            };

            if (newAsistencia.FechaControl.Kind != DateTimeKind.Utc)
            {
                newAsistencia.FechaControl = DateTime.SpecifyKind(newAsistencia.FechaControl, DateTimeKind.Utc);
            }

           
           if(asistencia!=null){

                if (asistencia.Estado == "retirado") newAsistencia.Estado = "asistido";

                else  newAsistencia.Estado = "retirado";
               
            }

             _context.Add(newAsistencia);
             _context.SaveChanges();
            
            TempData["error"]=null;

            TempData["success"] = "Estado del empleado registrado exitosamente";

            return RedirectToAction("index", "Asistencias");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}