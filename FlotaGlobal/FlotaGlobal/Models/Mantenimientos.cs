using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Index("Fecha", Name = "IX_Mantenimientos_Fecha")]
public partial class Mantenimientos
{
    [Key]
    public int IdMantenimiento { get; set; }

    public int IdVehiculo { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Tipo { get; set; } = null!;

    public DateOnly Fecha { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Responsable { get; set; } = null!;

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Costo { get; set; }

    public int Kilometraje { get; set; }

    [StringLength(1000)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Mantenimientos")]
    public virtual Vehiculos IdVehiculoNavigation { get; set; } = null!;
}
