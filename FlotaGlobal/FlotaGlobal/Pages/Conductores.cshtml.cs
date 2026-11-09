using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using FlotaGlobal.Data;
using Microsoft.AspNetCore.Mvc;

namespace FlotaGlobal.Pages
{
    public class ConductoresModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public ConductoresModel(FlotaDbContext context)
        {
            _context = context;
        }

        [TempData]
        public string? Mensaje { get; set; }

        // Aquí se almacena la lista de conductores que se lee de la base de datos
        public IList<FlotaGlobal.Models.Conductores> Conductores { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context != null)
            {
                // Consulta de forma asíncrona todos los registros de la tabla Conductores
                Conductores = await _context.Set<FlotaGlobal.Models.Conductores>().ToListAsync();
            }
        }
    }
}
