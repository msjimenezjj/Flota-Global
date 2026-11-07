using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Index("Correo", Name = "UQ__Usuarios__60695A198DCE85AF", IsUnique = true)]
public partial class Usuarios
{
    [Key]
    public int IdUsuario { get; set; }

    public int IdRol { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Nombre { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Correo { get; set; } = null!;

    [StringLength(255)]
    [Unicode(false)]
    public string Contrasena { get; set; } = null!;

    [StringLength(1)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;

    [InverseProperty("IdUsuarioNavigation")]
    public virtual ICollection<Auditorias> Auditorias { get; set; } = new List<Auditorias>();

    [ForeignKey("IdRol")]
    [InverseProperty("Usuarios")]
    public virtual Roles IdRolNavigation { get; set; } = null!;
}
