using Microsoft.EntityFrameworkCore;
using RutinApp.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RutinApp.Shared.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<UsuarioModel> Usuarios { get; set; }
        public DbSet<EjercicioModel> Ejercicios { get; set; }
        public DbSet<RutinaModel> Rutinas { get; set; }
        public DbSet<RutinaDetalleModel> RutinaEjercicios { get; set; }
        public DbSet<HistorialEntrenamientoModel> Historiales { get; set; }
        public DbSet<DetalleLogModel> DetalleLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UsuarioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Password).IsRequired();

                entity.HasMany(e => e.Rutinas)
                      .WithOne(r => r.Usuario)
                      .HasForeignKey(r => r.UsuarioId);

                entity.HasMany(e => e.Historiales)
                      .WithOne(h => h.Usuario)
                      .HasForeignKey(h => h.UsuarioId);
            });

            modelBuilder.Entity<EjercicioModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nombre).IsRequired().HasMaxLength(100);
                entity.Property(e => e.GrupoMuscular).IsRequired().HasMaxLength(50);
            });

            modelBuilder.Entity<RutinaModel>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.NombreRutina).IsRequired().HasMaxLength(100);
            });

            // Llave compuesta para la tabla intermedia
            modelBuilder.Entity<RutinaDetalleModel>(entity =>
            {
                entity.HasKey(e => new { e.RutinaId, e.EjercicioId });

                entity.HasOne(e => e.Rutina)
                      .WithMany(r => r.RutinaEjercicios)
                      .HasForeignKey(e => e.RutinaId);

                entity.HasOne(e => e.Ejercicio)
                      .WithMany(r => r.RutinaEjercicios)
                      .HasForeignKey(e => e.EjercicioId);
            });

            modelBuilder.Entity<HistorialEntrenamientoModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasMany(e => e.DetalleLogs)
                      .WithOne(d => d.Historial)
                      .HasForeignKey(d => d.HistorialId);
            });

            modelBuilder.Entity<DetalleLogModel>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.HasOne(e => e.Ejercicio)
                      .WithMany(r => r.DetalleLogs)
                      .HasForeignKey(e => e.EjercicioId);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
