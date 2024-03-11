using System;
using JDMR20241103.Models;
using Microsoft.EntityFrameworkCore;


namespace JDMR20241103.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Proveedor> Proveedores { get; set; }
        public DbSet<DetalleProveedor> DetalleProveedores { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Proveedor>()
                .HasMany(p => p.DetalleProveedores)
                .WithOne(d => d.Proveedor)
                .OnDelete(DeleteBehavior.Cascade);
        }


    }
}
