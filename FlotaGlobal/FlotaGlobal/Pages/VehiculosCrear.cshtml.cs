using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class VehiculosCrearModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public VehiculosCrearModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vehiculos Vehiculo { get; set; } = default!;

        [TempData]
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            // Inicializamos el objeto para evitar nulos en la vista
            Vehiculo = new Vehiculos();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Remover la validación de NumeroInterno ya que no existe en el modelo de base de datos
            ModelState.Remove("Vehiculo.NumeroInterno");

            if (!ModelState.IsValid) return Page();

            try
            {
                // Llamamos a tu procedimiento almacenado ProVehiculosInsertar con los parámetros reales
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC dbo.ProVehiculosInsertar @Placa = {Vehiculo.Placa}, @Marca = {Vehiculo.Marca}, @Modelo = {Vehiculo.Modelo}, @Anio = {Vehiculo.Anio}, @Capacidad = {Vehiculo.Capacidad}, @Estado = {Vehiculo.Estado}");
            }
            catch (SqlException ex)
            {
                // Captura errores como claves duplicadas (Placa ya registrada) desde la BD
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            Mensaje = $"El vehículo con placa {Vehiculo.Placa} fue registrado exitosamente.";
            return RedirectToPage("/Vehiculos");
        }
    }
}
