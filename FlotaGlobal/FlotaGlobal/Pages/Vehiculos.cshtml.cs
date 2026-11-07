using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;


namespace FlotaGlobal.Pages
{
    public class VehiculosModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public VehiculosModel(FlotaDbContext context)
        {
            _context = context;
        }

        // Esta lista almacenará los vehículos traídos de la base de datos
        public IList<Vehiculos> Vehiculos { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Vehiculos != null)
            {
                // Hacemos la consulta a la tabla Vehiculos en SQL Server
                Vehiculos = await _context.Vehiculos.ToListAsync();
            }
        }
    }
}
