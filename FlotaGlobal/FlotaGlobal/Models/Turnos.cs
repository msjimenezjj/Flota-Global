using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

public partial class Turnos
{
    [Key]
    public int IdTurno { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    [InverseProperty("IdTurnoNavigation")]
    public virtual ICollection<Asignaciones> Asignaciones { get; set; } = new List<Asignaciones>();
}
