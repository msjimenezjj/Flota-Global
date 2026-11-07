using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Rutas
{
    [Key]
    public int IdRuta { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Origen { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Destino { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string? Recorrido { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdRutaNavigation")]
    public virtual ICollection<Asignaciones> Asignaciones { get; set; } = new List<Asignaciones>();
}
