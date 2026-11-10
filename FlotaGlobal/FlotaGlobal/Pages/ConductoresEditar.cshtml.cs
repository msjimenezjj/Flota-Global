using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class ConductoresEditarModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public ConductoresEditarModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Conductores Conductor { get; set; } = default!;

        [TempData]
        public string? Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var c = await _context.Set<Conductores>().AsNoTracking().FirstOrDefaultAsync(x => x.IdConductor == id);
            if (c == null) return NotFound();
            Conductor = c;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Removemos las validaciones de los campos que no se envían al SP para evitar bloqueos
            ModelState.Remove("Conductor.Documento");

            if (!ModelState.IsValid) return Page();

            try
            {
                // Enviamos exactamente los 8 parámetros que tu procedimiento almacenado espera
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    EXEC dbo.ProConductoresActualizar 
                        @idConductor = {Conductor.IdConductor}, 
                        @primerNombre = {Conductor.PrimerNombre}, 
                        @segundoNombre = {Conductor.SegundoNombre}, 
                        @primerApellido = {Conductor.PrimerApellido}, 
                        @segundoApellido = {Conductor.SegundoApellido}, 
                        @licencia = {Conductor.Licencia}, 
                        @fechaVencimientoLicencia = {Conductor.FechaVencimientoLicencia}, 
                        @telefono = {Conductor.Telefono}");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            Mensaje = $"El conductor {Conductor.PrimerNombre} {Conductor.PrimerApellido} ha sido actualizado correctamente.";
            return RedirectToPage("/Conductores");
        }
    }
}

