using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Auditorias
{
    [Key]
    public int IdAuditoria { get; set; }

    public int IdUsuario { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Accion { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string EntidadAfectada { get; set; } = null!;

    public int IdEntidadAfectada { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime Fecha { get; set; }

    [ForeignKey("IdUsuario")]
    [InverseProperty("Auditorias")]
    public virtual Usuarios IdUsuarioNavigation { get; set; } = null!;
}
