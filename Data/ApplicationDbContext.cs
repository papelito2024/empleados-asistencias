using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using test2.Models;



namespace test2.Data
{
        
        public class ApplicationDbContext : DbContext
        {
            public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

            public DbSet<Empleado> Empleados { get; set; }

            public DbSet<Departamento> Departamentos { get; set; }

            public DbSet<Posicion> Posiciones { get; set; }

             public DbSet<Asistencia> Asistencias { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);  // Llamada al método base

        
    
            }
    }

    public class DbModelBuilder
    {
        internal object Entity<T>()
        {
            throw new NotImplementedException();
        }
    }
}
