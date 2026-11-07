using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Documentos
{
    [Key]
    public int IdDocumento { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Tipo { get; set; } = null!;

    public DateOnly FechaEmision { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    [StringLength(255)]
    [Unicode(false)]
    public string? Archivo { get; set; }

    public int? IdVehiculo { get; set; }

    public int? IdConductor { get; set; }

    [ForeignKey("IdConductor")]
    [InverseProperty("Documentos")]
    public virtual Conductores? IdConductorNavigation { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Documentos")]
    public virtual Vehiculos? IdVehiculoNavigation { get; set; }
}
