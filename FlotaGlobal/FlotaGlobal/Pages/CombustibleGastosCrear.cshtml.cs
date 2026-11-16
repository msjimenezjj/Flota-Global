using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    [Authorize(Roles = "1,2")]
    public class CombustibleGastosCrearModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public CombustibleGastosCrearModel(FlotaDbContext context)
        {
            _context = context;
        }

        public List<Vehiculos> ListaVehiculos { get; set; } = new();

        [BindProperty] public string Categoria { get; set; } = "Combustible"; // "Combustible" o "Operativo"
        [BindProperty] public int IdVehiculo { get; set; }
        [BindProperty] public DateTime Fecha { get; set; } = DateTime.Today;

        // Campos para Combustible
        [BindProperty] public decimal Cantidad { get; set; }
        [BindProperty] public decimal Costo { get; set; }
        [BindProperty] public int Kilometraje { get; set; }
        [BindProperty] public string MetodoPago { get; set; } = "Tarjeta Corporativa";

        // Campos para Gastos Operativos
        [BindProperty] public string TipoGasto { get; set; } = "Peaje";
        [BindProperty] public decimal Monto { get; set; }
        [BindProperty] public string? Descripcion { get; set; }

        [TempData] public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            ListaVehiculos = await _context.Vehiculos.ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Categoria == "Combustible")
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC dbo.ProCombustibleInsertar @IdVehiculo, @Fecha, @Cantidad, @Costo, @Kilometraje, @MetodoPago",
                        new SqlParameter("@IdVehiculo", IdVehiculo),
                        new SqlParameter("@Fecha", Fecha.Date),
                        new SqlParameter("@Cantidad", Cantidad),
                        new SqlParameter("@Costo", Costo),
                        new SqlParameter("@Kilometraje", Kilometraje),
                        new SqlParameter("@MetodoPago", MetodoPago)
                    );
                    Mensaje = "Abastecimiento de combustible registrado correctamente.";
                }
                else
                {
                    await _context.Database.ExecuteSqlRawAsync(
                        "EXEC dbo.ProGastosInsertar @IdVehiculo, @Tipo, @Monto, @Fecha, @Descripcion",
                        new SqlParameter("@IdVehiculo", IdVehiculo),
                        new SqlParameter("@Tipo", TipoGasto),
                        new SqlParameter("@Monto", Monto),
                        new SqlParameter("@Fecha", Fecha.Date),
                        new SqlParameter("@Descripcion", string.IsNullOrWhiteSpace(Descripcion) ? (object)DBNull.Value : Descripcion)
                    );
                    Mensaje = "Gasto operativo registrado correctamente.";
                }

                return RedirectToPage("/CombustibleGastos");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                ListaVehiculos = await _context.Vehiculos.ToListAsync();
                return Page();
            }
        }
    }
}

