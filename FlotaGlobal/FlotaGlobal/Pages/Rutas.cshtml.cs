using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class RutasModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public RutasModel(FlotaDbContext context)
        {
            _context = context;
        }

        public IList<Rutas> Rutas { get; set; } = new List<Rutas>();

        [TempData]
        public string? Mensaje { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? Buscar { get; set; }

        public async Task OnGetAsync()
        {
            var consulta = _context.Rutas.AsQueryable();

            if (!string.IsNullOrWhiteSpace(Buscar))
                consulta = consulta.Where(r => r.Origen.Contains(Buscar) || r.Destino.Contains(Buscar));

            Rutas = await consulta.OrderBy(r => r.Origen).ThenBy(r => r.Destino).ToListAsync();
        }
    }
}
