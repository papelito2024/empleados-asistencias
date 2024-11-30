using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace test2.Models
{
    public class Asistencias
    {
        [Key] // Marca este campo como clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Esto indica que es autoincremental
        int Id {get;set;}
        
        int EmpleadoId {get;set;}


        string Estado {get;set;} = "Presente";


        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm:ss}", ApplyFormatInEditMode = true)]
        [DataType(DataType.DateTime, ErrorMessage = "debe ser una fecha validda")]
        public DateTime FechaControl { get; set; } = DateTime.Now;

        [ForeignKey("DepartamentoID")]
        public virtual Empleado Empleado { get; set; }  // Relación con Categoria
    }
}