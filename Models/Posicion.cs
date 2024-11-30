using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace test2.Models
{
    public class Posicion
    {
        [Key] // Marca este campo como clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Esto indica que es autoincremental
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Este campo es requerido")]
        public string Name { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }

    }
    
}