using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        public Vehiculos Vehiculo { get; set; } = new Vehiculos { Estado = "disponible" };

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (await _context.Vehiculos.AnyAsync(v => v.Placa == Vehiculo.Placa))
            {
                ModelState.AddModelError("Vehiculo.Placa", "Ya existe un vehículo con esa placa.");
                return Page();
            }

            _context.Vehiculos.Add(Vehiculo);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo guardar. Revisa los datos, por ejemplo que el número interno no esté repetido.");
                return Page();
            }

            return RedirectToPage("/Vehiculos");
        }
    }
}