using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace test2.Models.ViewModels
{
    public class PosicionCreateViewModel
    {
        public Posicion Posicion { get; set; }
        
        public List<Departamento> Departamentos { get; set; }

        
    }
}