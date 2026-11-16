using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class CombustibleGastosModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public CombustibleGastosModel(FlotaDbContext context)
        {
            _context = context;
        }

        public class CombustibleItem
        {
            public string Placa { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public decimal Cantidad { get; set; }
            public decimal Costo { get; set; }
            public int Kilometraje { get; set; }
            public string MetodoPago { get; set; } = string.Empty;
        }

        public class GastoItem
        {
            public string Placa { get; set; } = string.Empty;
            public string Tipo { get; set; } = string.Empty;
            public decimal Monto { get; set; }
            public DateTime Fecha { get; set; }
            public string Descripcion { get; set; } = string.Empty;
        }

        public List<CombustibleItem> ListaCombustible { get; set; } = new();
        public List<GastoItem> ListaGastos { get; set; } = new();
        public List<Vehiculos> ListaVehiculos { get; set; } = new();

        [BindProperty(SupportsGet = true)] public int? FiltroVehiculo { get; set; }
        [BindProperty(SupportsGet = true)] public string? FiltroPeriodo { get; set; } // Formato YYYY-MM

        [TempData] public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            ListaVehiculos = await _context.Vehiculos.ToListAsync();

            int? anio = null, mes = null;
            if (!string.IsNullOrEmpty(FiltroPeriodo) && FiltroPeriodo.Contains('-'))
            {
                var partes = FiltroPeriodo.Split('-');
                anio = int.Parse(partes[0]);
                mes = int.Parse(partes[1]);
            }

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            // 1. Consultar Tabla 1: Control de Abastecimiento de Combustible
            using (var cmd1 = conn.CreateCommand())
            {
                cmd1.CommandText = @"
                    SELECT v.Placa, c.Fecha, c.Cantidad, c.Costo, c.Kilometraje, ISNULL(c.MetodoPago, 'Efectivo / Reembolso')
                    FROM Combustible c
                    INNER JOIN Vehiculos v ON c.IdVehiculo = v.IdVehiculo
                    WHERE (@idV IS NULL OR c.IdVehiculo = @idV)
                      AND (@anio IS NULL OR (YEAR(c.Fecha) = @anio AND MONTH(c.Fecha) = @mes))
                    ORDER BY c.Fecha DESC, c.IdCombustible DESC";

                cmd1.Parameters.Add(new SqlParameter("@idV", (object?)FiltroVehiculo ?? DBNull.Value));
                cmd1.Parameters.Add(new SqlParameter("@anio", (object?)anio ?? DBNull.Value));
                cmd1.Parameters.Add(new SqlParameter("@mes", (object?)mes ?? DBNull.Value));

                using var r1 = await cmd1.ExecuteReaderAsync();
                while (await r1.ReadAsync())
                {
                    ListaCombustible.Add(new CombustibleItem
                    {
                        Placa = r1.GetString(0),
                        Fecha = r1.GetDateTime(1),
                        Cantidad = r1.GetDecimal(2),
                        Costo = r1.GetDecimal(3),
                        Kilometraje = r1.GetInt32(4),
                        MetodoPago = r1.GetString(5)
                    });
                }
            }

            // 2. Consultar Tabla 2: Gastos Operativos Adicionales
            using (var cmd2 = conn.CreateCommand())
            {
                cmd2.CommandText = @"
                    SELECT v.Placa, g.Tipo, g.Monto, g.Fecha, ISNULL(g.Descripcion, '')
                    FROM Gastos g
                    INNER JOIN Vehiculos v ON g.IdVehiculo = v.IdVehiculo
                    WHERE (@idV IS NULL OR g.IdVehiculo = @idV)
                      AND (@anio IS NULL OR (YEAR(g.Fecha) = @anio AND MONTH(g.Fecha) = @mes))
                    ORDER BY g.Fecha DESC, g.IdGasto DESC";

                cmd2.Parameters.Add(new SqlParameter("@idV", (object?)FiltroVehiculo ?? DBNull.Value));
                cmd2.Parameters.Add(new SqlParameter("@anio", (object?)anio ?? DBNull.Value));
                cmd2.Parameters.Add(new SqlParameter("@mes", (object?)mes ?? DBNull.Value));

                using var r2 = await cmd2.ExecuteReaderAsync();
                while (await r2.ReadAsync())
                {
                    ListaGastos.Add(new GastoItem
                    {
                        Placa = r2.GetString(0),
                        Tipo = r2.GetString(1),
                        Monto = r2.GetDecimal(2),
                        Fecha = r2.GetDateTime(3),
                        Descripcion = r2.GetString(4)
                    });
                }
            }
        }
    }
}
