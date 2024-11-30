using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace test2.Models.ViewModels
{
    public class CreateEmpleadoViewModel
    {
        public Empleado Empleado { get; set; }
        
        public List<Departamento> Departamentos { get; set; }

        public List<Posicion> Posiciones { get; set; }
    }
}