using FlotaGlobal.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class IndexModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public IndexModel(FlotaDbContext context)
        {
            _context = context;
        }

        public int VehiculosDisponibles { get; set; }
        public int VehiculosMantenimiento { get; set; }
        public int TurnosActivos { get; set; }
        public int VehiculosFueraServicio { get; set; }

        public class AlertaDashboard
        {
            public string Categoria { get; set; } = string.Empty;
            public string Titulo { get; set; } = string.Empty;
            public string Detalle { get; set; } = string.Empty;
            public string Tiempo { get; set; } = string.Empty;
        }

        public List<AlertaDashboard> AlertasRecientes { get; set; } = new();

        public async Task OnGetAsync()
        {
            if (User.Identity == null || !User.Identity.IsAuthenticated) return;

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using (var cmdV = conn.CreateCommand())
            {
                cmdV.CommandText = "SELECT UPPER(LTRIM(RTRIM(ISNULL(Estado,'')))) FROM Vehiculos";
                using var rV = await cmdV.ExecuteReaderAsync();
                while (await rV.ReadAsync())
                {
                    var est = rV.GetString(0);
                    if (est == "A" || est == "DISPONIBLE") VehiculosDisponibles++;
                    else if (est == "M" || est == "EN_MANTENIMIENTO") VehiculosMantenimiento++;
                    else VehiculosFueraServicio++;
                }
            }

            using (var cmdT = conn.CreateCommand())
            {
                cmdT.CommandText = "SELECT COUNT(*) FROM Asignaciones WHERE FechaAsignacion >= CAST(GETDATE() AS DATE)";
                var result = await cmdT.ExecuteScalarAsync();
                TurnosActivos = Convert.ToInt32(result);
            }

            using (var cmdM = conn.CreateCommand())
            {
                cmdM.CommandText = @"
                    SELECT TOP 3 v.Placa, m.Tipo, ISNULL(m.Descripcion, m.Responsable), m.Fecha
                    FROM Mantenimientos m
                    INNER JOIN Vehiculos v ON m.IdVehiculo = v.IdVehiculo
                    WHERE m.Estado IN ('Programado', 'En Progreso')
                    ORDER BY m.Fecha ASC";
                using var rM = await cmdM.ExecuteReaderAsync();
                while (await rM.ReadAsync())
                {
                    AlertasRecientes.Add(new AlertaDashboard
                    {
                        Categoria = "MANTENIMIENTO",
                        Titulo = $"Mantenimiento {rM.GetString(1)} (Placa {rM.GetString(0)})",
                        Detalle = rM.GetString(2),
                        Tiempo = rM.GetDateTime(3).ToString("dd/MM/yyyy")
                    });
                }
            }

            using (var cmdC = conn.CreateCommand())
            {
                cmdC.CommandText = @"
                    SELECT TOP 2 PrimerNombre + ' ' + PrimerApellido, Licencia, FechaVencimientoLicencia
                    FROM Conductores
                    WHERE Estado = 'A' AND FechaVencimientoLicencia <= DATEADD(DAY, 30, GETDATE())
                    ORDER BY FechaVencimientoLicencia ASC";
                using var rC = await cmdC.ExecuteReaderAsync();
                while (await rC.ReadAsync())
                {
                    AlertasRecientes.Add(new AlertaDashboard
                    {
                        Categoria = "DOCUMENTO / LICENCIA",
                        Titulo = $"Licencia por vencer ({rC.GetString(0)})",
                        Detalle = $"Licencia N° {rC.GetString(1)} — Vence el {rC.GetDateTime(2):dd/MM/yyyy}",
                        Tiempo = "Próximo"
                    });
                }
            }
        }
    }
}
