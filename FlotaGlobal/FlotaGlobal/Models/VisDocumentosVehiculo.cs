using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Models;

[Keyless]
public partial class VisDocumentosVehiculo
{
    public int IdDocumento { get; set; }

    public int? IdVehiculo { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Placa { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Marca { get; set; } = null!;

    [StringLength(50)]
    [Unicode(false)]
    public string Modelo { get; set; } = null!;

    [StringLength(30)]
    [Unicode(false)]
    public string Tipo { get; set; } = null!;

    public DateOnly FechaEmision { get; set; }

    public DateOnly FechaVencimiento { get; set; }

    public int? DiasRestantes { get; set; }

    [StringLength(10)]
    [Unicode(false)]
    public string Vigencia { get; set; } = null!;
}
