using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class VehiculosEditarModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public VehiculosEditarModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vehiculos Vehiculo { get; set; } = default!;

        [TempData]
        public string? Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var v = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(x => x.IdVehiculo == id);
            if (v == null) return NotFound();
            Vehiculo = v;
            return Page();
        }

        // Guarda los cambios usando tu SP: ProActualizarVehiculo
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC dbo.ProActualizarVehiculo @IdVehiculo = {Vehiculo.IdVehiculo}, @Marca = {Vehiculo.Marca}, @Modelo = {Vehiculo.Modelo}, @Anio = {Vehiculo.Anio}, @Capacidad = {Vehiculo.Capacidad}, @Estado = {Vehiculo.Estado}");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            Mensaje = "Vehículo actualizado con éxito.";
            return RedirectToPage("/Vehiculos");
        }

        // Cambia el estado usando tu SP: ProCambiarEstadoVehiculo
        public async Task<IActionResult> OnPostEstadoAsync(int id, string nuevoEstado)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC dbo.ProCambiarEstadoVehiculo @IdVehiculo = {id}, @Estado = {nuevoEstado}");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                var v = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(x => x.IdVehiculo == id);
                if (v == null) return NotFound();
                Vehiculo = v;
                return Page();
            }

            Mensaje = "Estado del vehículo actualizado correctamente.";
            return RedirectToPage("/Vehiculos");
        }
    }
}
