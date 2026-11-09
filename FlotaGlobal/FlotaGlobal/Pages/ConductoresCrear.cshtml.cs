using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class ConductoresCrearModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public ConductoresCrearModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Conductores Conductor { get; set; } = default!;

        [TempData]
        public string? Mensaje { get; set; }

        public void OnGet()
        {
            // Inicializamos el objeto para evitar nulos en los inputs de la vista
            Conductor = new Conductores();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                // Ejecutamos tu SP 'ProRegistrarConductor' mapeando los parámetros reales de tu tabla
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC dbo.ProRegistrarConductor 
                        @Documento = {Conductor.Documento}, 
                        @PrimerNombre = {Conductor.PrimerNombre}, 
                        @SegundoNombre = {Conductor.SegundoNombre}, 
                        @PrimerApellido = {Conductor.PrimerApellido}, 
                        @SegundoApellido = {Conductor.SegundoApellido}, 
                        @Licencia = {Conductor.Licencia}, 
                        @FechaVencimientoLicencia = {Conductor.FechaVencimientoLicencia}, 
                        @Telefono = {Conductor.Telefono}, 
                        @Estado = {Conductor.Estado}");
            }
            catch (SqlException ex)
            {
                // Captura excepciones de la BD (ej. Documento o Licencia ya registrados)
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            Mensaje = $"El conductor {Conductor.PrimerNombre} {Conductor.PrimerApellido} fue registrado exitosamente.";
            return RedirectToPage("/Conductores");
        }
    }
}
