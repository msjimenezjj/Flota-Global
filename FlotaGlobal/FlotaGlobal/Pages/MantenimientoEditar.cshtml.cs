using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    [Authorize(Roles = "1,2")]
    public class MantenimientoEditarModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public MantenimientoEditarModel(FlotaDbContext context)
        {
            _context = context;
        }

        public List<Vehiculos> ListaVehiculos { get; set; } = new();

        [BindProperty] public int IdMantenimiento { get; set; }
        [BindProperty] public int IdVehiculo { get; set; }
        [BindProperty] public string Tipo { get; set; } = string.Empty;
        [BindProperty] public DateTime Fecha { get; set; }
        [BindProperty] public string Responsable { get; set; } = string.Empty;
        [BindProperty] public decimal Costo { get; set; }
        [BindProperty] public int Kilometraje { get; set; }
        [BindProperty] public string Descripcion { get; set; } = string.Empty;
        [BindProperty] public string Estado { get; set; } = string.Empty;

        [TempData] public string? Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            ListaVehiculos = await _context.Vehiculos.ToListAsync();

            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IdMantenimiento, IdVehiculo, Tipo, Fecha, ISNULL(Responsable,''), ISNULL(Costo,0), ISNULL(Kilometraje,0), ISNULL(Descripcion,''), ISNULL(Estado,'Programado') FROM Mantenimientos WHERE IdMantenimiento = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return NotFound();

            IdMantenimiento = reader.GetInt32(0);
            IdVehiculo = reader.GetInt32(1);
            Tipo = reader.GetString(2);
            Fecha = reader.GetDateTime(3);
            Responsable = reader.GetString(4);
            Costo = reader.GetDecimal(5);
            Kilometraje = reader.GetInt32(6);
            Descripcion = reader.GetString(7);
            Estado = reader.GetString(8);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.ProMantenimientosActualizar @IdMantenimiento, @IdVehiculo, @Tipo, @Fecha, @Responsable, @Costo, @Kilometraje, @Descripcion, @Estado",
                    new SqlParameter("@IdMantenimiento", IdMantenimiento),
                    new SqlParameter("@IdVehiculo", IdVehiculo),
                    new SqlParameter("@Tipo", Tipo),
                    new SqlParameter("@Fecha", Fecha.Date),
                    new SqlParameter("@Responsable", Responsable ?? (object)DBNull.Value),
                    new SqlParameter("@Costo", Costo),
                    new SqlParameter("@Kilometraje", Kilometraje),
                    new SqlParameter("@Descripcion", Descripcion ?? (object)DBNull.Value),
                    new SqlParameter("@Estado", Estado)
                );

                Mensaje = "Mantenimiento actualizado correctamente.";
                return RedirectToPage("/Mantenimiento");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ListaVehiculos = await _context.Vehiculos.ToListAsync();
                return Page();
            }
        }
    }
}
