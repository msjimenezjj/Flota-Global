using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Incidentes
{
    [Key]
    public int IdIncidente { get; set; }

    public int? IdVehiculo { get; set; }

    public int? IdConductor { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Tipo { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string Descripcion { get; set; } = null!;

    [Column(TypeName = "decimal(12, 2)")]
    public decimal? Costo { get; set; }

    [ForeignKey("IdConductor")]
    [InverseProperty("Incidentes")]
    public virtual Conductores? IdConductorNavigation { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Incidentes")]
    public virtual Vehiculos? IdVehiculoNavigation { get; set; }
}
