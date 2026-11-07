using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Keyless]
public partial class VisAsignacionesDetalle
{
    public int IdAsignacion { get; set; }

    public int IdVehiculo { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Placa { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Marca { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Modelo { get; set; } = null!;

    public int IdConductor { get; set; }

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

    public int IdRuta { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Origen { get; set; } = null!;

    [StringLength(100)]
    [Unicode(false)]
    public string Destino { get; set; } = null!;

    public int IdTurno { get; set; }

    public DateOnly Fecha { get; set; }

    public TimeOnly HoraInicio { get; set; }

    public TimeOnly HoraFin { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Estado { get; set; } = null!;
}
