using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;

namespace test2.Models
{
    public class Empleado
    {
        [Key] // Marca este campo como clave primaria
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] // Esto indica que es autoincremental
        public int Id { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(40)]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        [StringLength(40)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int RolID { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int DepartamentoID { get; set; }

        [DataType(DataType.Date, ErrorMessage = "debe ser una fecha validda")]
        public DataType FechaDeNacimineto {get;set;}
        public decimal Salario { get; set; }

        [ForeignKey("DepartamentoID")]
        public virtual Departamento Departamento { get; set; }  // Relación con Categoria
        [ForeignKey("RolID")]
        public virtual Rol Rol { get; set; }  // Relación con Categoria
    }
}