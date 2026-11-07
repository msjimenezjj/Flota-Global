using System;
using System.Collections.Generic;
using FlotaGlobal.Models;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Data;

public partial class FlotaDbContext : DbContext
{
    public FlotaDbContext(DbContextOptions<FlotaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Asignaciones> Asignaciones { get; set; }

    public virtual DbSet<Auditorias> Auditorias { get; set; }

    public virtual DbSet<Combustible> Combustible { get; set; }

    public virtual DbSet<Conductores> Conductores { get; set; }

    public virtual DbSet<Documentos> Documentos { get; set; }

    public virtual DbSet<Gastos> Gastos { get; set; }

    public virtual DbSet<Incidentes> Incidentes { get; set; }

    public virtual DbSet<Mantenimientos> Mantenimientos { get; set; }

    public virtual DbSet<Roles> Roles { get; set; }

    public virtual DbSet<Rutas> Rutas { get; set; }

    public virtual DbSet<Turnos> Turnos { get; set; }

    public virtual DbSet<Usuarios> Usuarios { get; set; }

    public virtual DbSet<Vehiculos> Vehiculos { get; set; }

    public virtual DbSet<VisAsignacionesDetalle> VisAsignacionesDetalle { get; set; }

    public virtual DbSet<VisDocumentosVehiculo> VisDocumentosVehiculo { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Asignaciones>(entity =>
        {
            entity.HasKey(e => e.IdAsignacion).HasName("PK__Asignaci__A7235DFFE44432DA");

            entity.HasOne(d => d.IdConductorNavigation).WithMany(p => p.Asignaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignaciones_Conductores");

            entity.HasOne(d => d.IdRutaNavigation).WithMany(p => p.Asignaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignaciones_Rutas");

            entity.HasOne(d => d.IdTurnoNavigation).WithMany(p => p.Asignaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignaciones_Turnos");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Asignaciones)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Asignaciones_Vehiculos");
        });

        modelBuilder.Entity<Auditorias>(entity =>
        {
            entity.HasKey(e => e.IdAuditoria).HasName("PK__Auditori__7FD13FA09B8D651D");

            entity.HasOne(d => d.IdUsuarioNavigation).WithMany(p => p.Auditorias)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Auditorias_Usuarios");
        });

        modelBuilder.Entity<Combustible>(entity =>
        {
            entity.HasKey(e => e.IdCombustible).HasName("PK__Combusti__699FEC5A400E14BF");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Combustible)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Combustible_Vehiculos");
        });

        modelBuilder.Entity<Conductores>(entity =>
        {
            entity.HasKey(e => e.IdConductor).HasName("PK__Conducto__D6D2960316087F35");

            entity.Property(e => e.Estado).IsFixedLength();
        });

        modelBuilder.Entity<Documentos>(entity =>
        {
            entity.HasKey(e => e.IdDocumento).HasName("PK__Document__E5207347A8E153BA");

            entity.HasOne(d => d.IdConductorNavigation).WithMany(p => p.Documentos).HasConstraintName("FK_Documentos_Conductores");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Documentos).HasConstraintName("FK_Documentos_Vehiculos");
        });

        modelBuilder.Entity<Gastos>(entity =>
        {
            entity.HasKey(e => e.IdGasto).HasName("PK__Gastos__C630244D5EE98EFD");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Gastos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gastos_Vehiculos");
        });

        modelBuilder.Entity<Incidentes>(entity =>
        {
            entity.HasKey(e => e.IdIncidente).HasName("PK__Incident__E92B13DFF3A30743");

            entity.HasOne(d => d.IdConductorNavigation).WithMany(p => p.Incidentes).HasConstraintName("FK_Incidentes_Conductores");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Incidentes).HasConstraintName("FK_Incidentes_Vehiculos");
        });

        modelBuilder.Entity<Mantenimientos>(entity =>
        {
            entity.HasKey(e => e.IdMantenimiento).HasName("PK__Mantenim__DD1C4417FAE57056");

            entity.HasOne(d => d.IdVehiculoNavigation).WithMany(p => p.Mantenimientos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Mantenimientos_Vehiculos");
        });

        modelBuilder.Entity<Roles>(entity =>
        {
            entity.HasKey(e => e.IdRol).HasName("PK__Roles__2A49584C35657A41");
        });

        modelBuilder.Entity<Rutas>(entity =>
        {
            entity.HasKey(e => e.IdRuta).HasName("PK__Rutas__887538FE4911F22F");

            entity.Property(e => e.Estado).IsFixedLength();
        });

        modelBuilder.Entity<Turnos>(entity =>
        {
            entity.HasKey(e => e.IdTurno).HasName("PK__Turnos__C1ECF79AFA754BAB");
        });

        modelBuilder.Entity<Usuarios>(entity =>
        {
            entity.HasKey(e => e.IdUsuario).HasName("PK__Usuarios__5B65BF9773E80CFA");

            entity.Property(e => e.Estado).IsFixedLength();

            entity.HasOne(d => d.IdRolNavigation).WithMany(p => p.Usuarios)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Usuarios_Roles");
        });

        modelBuilder.Entity<Vehiculos>(entity =>
        {
            entity.HasKey(e => e.IdVehiculo).HasName("PK__Vehiculo__708612153A92FA1B");
        });

        modelBuilder.Entity<VisAsignacionesDetalle>(entity =>
        {
            entity.ToView("VisAsignacionesDetalle");
        });

        modelBuilder.Entity<VisDocumentosVehiculo>(entity =>
        {
            entity.ToView("VisDocumentosVehiculo");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
