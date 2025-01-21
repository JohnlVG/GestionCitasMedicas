using GestionCitasMedicas;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace GestionCitasMedicas
{
    public class AppDbContext : DbContext
    {
        // Constructor para configurar el DbContext
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // DbSet para cada entidad
        public DbSet<Entidades.Paciente> Pacientes { get; set; } = null!;
        public DbSet<Entidades.Doctor> Doctores { get; set; } = null!;
        public DbSet<Entidades.Cita> Citas { get; set; } = null!;
        public DbSet<Entidades.Procedimiento> Procedimientos { get; set; } = null!;

        // Configuración adicional del modelo
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuración para la tabla Pacientes
            modelBuilder.Entity<Entidades.Paciente>(entity =>
            {
                entity.ToTable("Pacientes");
                entity.HasKey(p => p.IdPaciente);
                entity.Property(p => p.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Apellido).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Telefono).HasMaxLength(15);
            });

            // Configuración para la tabla Doctores
            modelBuilder.Entity<Entidades.Doctor>(entity =>
            {
                entity.ToTable("Doctores");
                entity.HasKey(d => d.IdDoctor);
                entity.Property(d => d.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(d => d.Especialidad).IsRequired().HasMaxLength(100);
            });

            // Configuración para la tabla Citas
            modelBuilder.Entity<Entidades.Cita>(entity =>
            {
                entity.ToTable("Citas");
                entity.HasKey(c => c.IdCita);
                entity.Property(c => c.Fecha).IsRequired();

                entity.HasOne(c => c.Paciente)
                      .WithMany(p => p.Citas)
                      .HasForeignKey(c => c.IdPaciente)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(c => c.Doctor)
                      .WithMany(d => d.Citas)
                      .HasForeignKey(c => c.IdDoctor)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Configuración para la tabla Procedimientos
            modelBuilder.Entity<Entidades.Procedimiento>(entity =>
            {
                entity.ToTable("Procedimientos");
                entity.HasKey(p => p.IdProcedimiento);
                entity.Property(p => p.Descripcion).IsRequired().HasMaxLength(250);
                entity.Property(p => p.Costo).IsRequired().HasColumnType("decimal(10,2)");

                // Relación con Cita
                entity.HasOne(p => p.Cita)
                      .WithMany(c => c.Procedimientos)
                      .HasForeignKey(p => p.IdCita)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
