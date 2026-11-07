using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Index("FechaAsignacion", Name = "IX_Asignaciones_FechaAsignacion")]
public partial class Asignaciones
{
    [Key]
    public int IdAsignacion { get; set; }

    public int IdVehiculo { get; set; }

    public int IdConductor { get; set; }

    public int IdRuta { get; set; }

    public int IdTurno { get; set; }

    public DateOnly FechaAsignacion { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdConductor")]
    [InverseProperty("Asignaciones")]
    public virtual Conductores IdConductorNavigation { get; set; } = null!;

    [ForeignKey("IdRuta")]
    [InverseProperty("Asignaciones")]
    public virtual Rutas IdRutaNavigation { get; set; } = null!;

    [ForeignKey("IdTurno")]
    [InverseProperty("Asignaciones")]
    public virtual Turnos IdTurnoNavigation { get; set; } = null!;

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Asignaciones")]
    public virtual Vehiculos IdVehiculoNavigation { get; set; } = null!;
}
