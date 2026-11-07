using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Gastos
{
    [Key]
    public int IdGasto { get; set; }

    public int IdVehiculo { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Tipo { get; set; } = null!;

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Monto { get; set; }

    public DateOnly Fecha { get; set; }

    [StringLength(500)]
    [Unicode(false)]
    public string? Descripcion { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Gastos")]
    public virtual Vehiculos IdVehiculoNavigation { get; set; } = null!;
}
