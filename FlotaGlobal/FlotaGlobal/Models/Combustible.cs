using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Combustible
{
    [Key]
    public int IdCombustible { get; set; }

    public int IdVehiculo { get; set; }

    public DateOnly Fecha { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal Cantidad { get; set; }

    [Column(TypeName = "decimal(12, 2)")]
    public decimal Costo { get; set; }

    public int Kilometraje { get; set; }

    [ForeignKey("IdVehiculo")]
    [InverseProperty("Combustible")]
    public virtual Vehiculos IdVehiculoNavigation { get; set; } = null!;
}
