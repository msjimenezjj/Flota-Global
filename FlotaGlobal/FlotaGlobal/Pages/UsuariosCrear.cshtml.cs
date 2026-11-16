using FlotaGlobal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlotaGlobal.Pages
{
    [Authorize(Roles = "1")]
    public class UsuariosCrearModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public UsuariosCrearModel(FlotaDbContext context)
        {
            _context = context;
        }

        public class RolOption
        {
            public int IdRol { get; set; }
            public string Nombre { get; set; } = string.Empty;
        }

        public List<RolOption> ListaRoles { get; set; } = new();

        [BindProperty] public int IdRol { get; set; }
        [BindProperty] public string Nombre { get; set; } = string.Empty;
        [BindProperty] public string Correo { get; set; } = string.Empty;
        [BindProperty] public string Contrasena { get; set; } = string.Empty;
        [BindProperty] public string Estado { get; set; } = "A";

        [TempData] public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            await CargarRolesAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                var paramIdOut = new SqlParameter("@IdUsuario", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.ProUsuariosInsertar @IdUsuario OUTPUT, @IdRol, @Nombre, @Correo, @Contrasena, @Estado",
                    paramIdOut,
                    new SqlParameter("@IdRol", IdRol),
                    new SqlParameter("@Nombre", Nombre),
                    new SqlParameter("@Correo", Correo),
                    new SqlParameter("@Contrasena", Contrasena),
                    new SqlParameter("@Estado", Estado)
                );

                Mensaje = $"Usuario {Nombre} creado y asignado correctamente.";
                return RedirectToPage("/Usuarios");
            }
            catch (SqlException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                await CargarRolesAsync();
                return Page();
            }
        }

        private async Task CargarRolesAsync()
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IdRol, Nombre FROM Roles ORDER BY IdRol ASC";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ListaRoles.Add(new RolOption
                {
                    IdRol = reader.GetInt32(0),
                    Nombre = reader.GetString(1)
                });
            }
        }
    }
}
