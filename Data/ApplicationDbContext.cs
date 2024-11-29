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

            public DbSet<Rol> Roles { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);  // Llamada al método base

                // Configuración de la relación entre Producto y Categoria
                modelBuilder.Entity<Empleado>()
                    .HasOne(p => p.Rol)
                    .WithMany(c => c.Empleados)
                    .HasForeignKey(p => p.RolID);  // Definir la clave foránea

                modelBuilder.Entity<Empleado>()
                .HasOne(p => p.Departamento)
                .WithMany(c => c.Empleados)
                .HasForeignKey(p => p.DepartamentoID);
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
