using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class MantenimientoModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public MantenimientoModel(FlotaDbContext context)
        {
            _context = context;
        }

        public class MantenimientoViewModel
        {
            public int IdMantenimientos { get; set; }
            public int IdVehiculo { get; set; }
            public string VehiculoTexto { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public string Responsable { get; set; } = string.Empty;
            public decimal Costo { get; set; }
            public int Kilometraje { get; set; }
            public string Descripcion { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
        }

        public List<MantenimientoViewModel> Historial { get; set; } = new();
        public List<MantenimientoViewModel> AlertasProximos { get; set; } = new();
        public List<Vehiculos> ListaVehiculos { get; set; } = new();

        [BindProperty(SupportsGet = true)] public int? FiltroVehiculo { get; set; }
        [BindProperty(SupportsGet = true)] public string? FiltroTipo { get; set; }

        [TempData] public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            ListaVehiculos = await _context.Vehiculos.ToListAsync();

            var todos = new List<MantenimientoViewModel>();
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    m.IdMantenimiento,
                    m.IdVehiculo,
                    v.Placa + ' (' + v.Marca + ' ' + v.Modelo + ')' AS VehiculoTexto,
                    m.Tipo,
                    m.Fecha,
                    ISNULL(m.Responsable, 'Sin asignar') AS Responsable,
                    ISNULL(m.Costo, 0) AS Costo,
                    ISNULL(m.Kilometraje, 0) AS Kilometraje,
                    ISNULL(m.Descripcion, '') AS Descripcion,
                    ISNULL(m.Estado, 'Programado') AS Estado
                FROM Mantenimientos m
                INNER JOIN Vehiculos v ON m.IdVehiculo = v.IdVehiculo
                ORDER BY m.Fecha DESC, m.IdMantenimiento DESC";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                todos.Add(new MantenimientoViewModel
                {
                    IdMantenimientos = reader.GetInt32(0),
                    IdVehiculo = reader.GetInt32(1),
                    VehiculoTexto = reader.GetString(2),
                    Tipo = reader.GetString(3),
                    Fecha = reader.GetDateTime(4),
                    Responsable = reader.GetString(5),
                    Costo = reader.GetDecimal(6),
                    Kilometraje = reader.GetInt32(7),
                    Descripcion = reader.GetString(8),
                    Estado = reader.GetString(9)
                });
            }

            // 1. Alimentar el recuadro de PRÓXIMOS MANTENIMIENTOS / ALERTAS (estilo Figma)
            AlertasProximos = todos
                .Where(x => x.Estado.Equals("Programado", StringComparison.OrdinalIgnoreCase) ||
                            x.Estado.Equals("En Progreso", StringComparison.OrdinalIgnoreCase))
                .Take(4)
                .ToList();

            // 2. Aplicar filtros para el historial preventivo/correctivo
            var consulta = todos.AsEnumerable();
            if (FiltroVehiculo.HasValue && FiltroVehiculo.Value > 0)
            {
                consulta = consulta.Where(x => x.IdVehiculo == FiltroVehiculo.Value);
            }
            if (!string.IsNullOrEmpty(FiltroTipo))
            {
                consulta = consulta.Where(x => x.Tipo.Equals(FiltroTipo, StringComparison.OrdinalIgnoreCase));
            }

            Historial = consulta.ToList();
        }
    }
}
