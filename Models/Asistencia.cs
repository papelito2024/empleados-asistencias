using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace test2.Models
{
    public class Asistencia
    {
        [Key] // Marca este campo como clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Esto indica que es autoincremental
        public  int Id {get;set;}
        
        [Required]
       public int EmpleadoId {get;set;}

        [Required]
        public string Estado {get;set;}="ausente";


       public  DateTime FechaControl { get; set; } = DateTime.Now;

        
         public virtual Empleado Empleado { get; set; }  // Relación con Categoria
    }
}