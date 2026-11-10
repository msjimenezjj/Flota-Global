using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class RutasCrearModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public RutasCrearModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Rutas Ruta { get; set; } = new Rutas { Estado = "A" };

        [TempData]
        public string? Mensaje { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync($@"
                    DECLARE @NuevoId INT;
                    EXEC dbo.ProRutasInsertar @IdRuta = @NuevoId OUTPUT, @Origen = {Ruta.Origen}, @Destino = {Ruta.Destino}, @Recorriedo = {Ruta.Recorrido}, @Estado = {Ruta.Estado}");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return Page();
            }

            Mensaje = "Ruta registrada.";
            return RedirectToPage("/Rutas");
        }
    }
}