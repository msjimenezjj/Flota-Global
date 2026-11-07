using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Index("Nombre", Name = "UQ__Roles__75E3EFCF1593D634", IsUnique = true)]
public partial class Roles
{
    [Key]
    public int IdRol { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(500)]
    [Unicode(false)]
    public string Permisos { get; set; } = null!;

    [InverseProperty("IdRolNavigation")]
    public virtual ICollection<Usuarios> Usuarios { get; set; } = new List<Usuarios>();
}
