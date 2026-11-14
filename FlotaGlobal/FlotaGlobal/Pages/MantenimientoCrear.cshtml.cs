using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlotaGlobal.Pages
{
    [Authorize(Roles = "1,2")]
    public class MantenimientoCrearModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public MantenimientoCrearModel(FlotaDbContext context)
        {
            _context = context;
        }

        public List<Vehiculos> ListaVehiculos { get; set; } = new();

        [BindProperty] public int IdVehiculo { get; set; }
        [BindProperty] public string Tipo { get; set; } = "Preventivo";
        [BindProperty] public DateTime Fecha { get; set; } = DateTime.Today;
        [BindProperty] public string Responsable { get; set; } = string.Empty;
        [BindProperty] public decimal Costo { get; set; }
        [BindProperty] public int Kilometraje { get; set; }
        [BindProperty] public string Descripcion { get; set; } = string.Empty;
        [BindProperty] public string Estado { get; set; } = "Completado";

        [TempData] public string? Mensaje { get; set; }

        public async Task OnGetAsync(string? modo)
        {
            ListaVehiculos = await _context.Vehiculos.ToListAsync();

            // Si viene del botón blanco "Programar Taller Futuro", preselecciona Programado
            if (modo == "futuro")
            {
                Estado = "Programado";
                Fecha = DateTime.Today.AddDays(1);
            }
            else
            {
                Estado = "En Progreso";
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                ListaVehiculos = await _context.Vehiculos.ToListAsync();
                return Page();
            }

            try
            {
                var paramIdOut = new SqlParameter("@IdMantenimiento", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.ProMantenimientosInsertar @IdMantenimiento OUTPUT, @IdVehiculo, @Tipo, @Fecha, @Responsable, @Costo, @Kilometraje, @Descripcion, @Estado",
                    paramIdOut,
                    new SqlParameter("@IdVehiculo", IdVehiculo),
                    new SqlParameter("@Tipo", Tipo),
                    new SqlParameter("@Fecha", Fecha.Date),
                    new SqlParameter("@Responsable", Responsable ?? (object)DBNull.Value),
                    new SqlParameter("@Costo", Costo),
                    new SqlParameter("@Kilometraje", Kilometraje),
                    new SqlParameter("@Descripcion", Descripcion ?? (object)DBNull.Value),
                    new SqlParameter("@Estado", Estado)
                );

                Mensaje = "Registro de mantenimiento guardado exitosamente en el historial.";
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
