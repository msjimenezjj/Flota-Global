using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Index("Documento", Name = "IX_Conductores_Documento")]
[Index("Licencia", Name = "IX_Conductores_Licencia")]
[Index("Licencia", Name = "UQ__Conducto__3D4BD6BCAFF7D712", IsUnique = true)]
[Index("Documento", Name = "UQ__Conducto__AF73706D6041A1B7", IsUnique = true)]
public partial class Conductores
{
    [Key]
    public int IdConductor { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Documento { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string PrimerNombre { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string SegundoNombre { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string PrimerApellido { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string SegundoApellido { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Licencia { get; set; } = null!;

    public DateOnly FechaVencimientoLicencia { get; set; }

    [StringLength(15)]
    [Unicode(false)]
    public string Telefono { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdConductorNavigation")]
    public virtual ICollection<Asignaciones> Asignaciones { get; set; } = new List<Asignaciones>();

    [InverseProperty("IdConductorNavigation")]
    public virtual ICollection<Documentos> Documentos { get; set; } = new List<Documentos>();

    [InverseProperty("IdConductorNavigation")]
    public virtual ICollection<Incidentes> Incidentes { get; set; } = new List<Incidentes>();
}
