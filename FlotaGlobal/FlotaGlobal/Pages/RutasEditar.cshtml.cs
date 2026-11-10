using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class RutasEditarModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public RutasEditarModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Rutas Ruta { get; set; } = default!;

        [TempData]
        public string? Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var r = await _context.Rutas.AsNoTracking().FirstOrDefaultAsync(x => x.IdRuta == id);
            if (r == null) return NotFound();
            Ruta = r;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $"EXEC dbo.ProRutasActualizar @IdRuta = {Ruta.IdRuta}, @Origen = {Ruta.Origen}, @Destino = {Ruta.Destino}, @Recorrido = {Ruta.Recorrido}, @Estado = {Ruta.Estado}");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            Mensaje = "Ruta actualizada.";
            return RedirectToPage("/Rutas");
        }
    }
}