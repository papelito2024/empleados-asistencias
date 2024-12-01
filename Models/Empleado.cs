using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        [Required(ErrorMessage = "apellito Este campo es requerido")]
        [StringLength(40)]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int PosicionId { get; set; }

        [Required(ErrorMessage = "Este campo es requerido")]
        public int DepartamentoId { get; set; }

       
        public virtual  Departamento Departamento { get; set; }
       
        public virtual Posicion Posicion { get; set; } 

        [Required(ErrorMessage = "adasdasd Este campo es requerido")]
        public DateTime FechaDeNacimiento {get;set;}

        [Required(ErrorMessage = "12232312o Este campo es requerido")]
        public decimal Salario { get; set; }



        public DateTime FechaDeNacimientoUtc
    {
        get => FechaDeNacimiento.Kind == DateTimeKind.Unspecified 
               ? DateTime.SpecifyKind(FechaDeNacimiento, DateTimeKind.Utc)
               : FechaDeNacimiento.ToUniversalTime();
        set => FechaDeNacimiento = value.ToUniversalTime();
    }

    }
}