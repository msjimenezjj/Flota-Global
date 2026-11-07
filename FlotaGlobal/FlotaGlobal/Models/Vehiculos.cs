using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Index("Placa", Name = "IX_Vehiculos_Placa")]
[Index("Placa", Name = "UQ__Vehiculo__8310F99DDE48069F", IsUnique = true)]
[Index("NumeroInterno", Name = "UQ__Vehiculo__8D8E1290F6C36A2D", IsUnique = true)]
public partial class Vehiculos
{
    [Key]
    public int IdVehiculo { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Placa { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string NumeroInterno { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Marca { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Modelo { get; set; } = null!;

    public short Anio { get; set; }

    public int Capacidad { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Asignaciones> Asignaciones { get; set; } = new List<Asignaciones>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Combustible> Combustible { get; set; } = new List<Combustible>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Documentos> Documentos { get; set; } = new List<Documentos>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Gastos> Gastos { get; set; } = new List<Gastos>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Incidentes> Incidentes { get; set; } = new List<Incidentes>();

    [InverseProperty("IdVehiculoNavigation")]
    public virtual ICollection<Mantenimientos> Mantenimientos { get; set; } = new List<Mantenimientos>();
}
