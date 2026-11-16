using FlotaGlobal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    [Authorize(Roles = "1")]
    public class UsuariosEditarModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public UsuariosEditarModel(FlotaDbContext context)
        {
            _context = context;
        }

        public class RolOption
        {
            public int IdRol { get; set; }
            public string Nombre { get; set; } = string.Empty;
        }

        public List<RolOption> ListaRoles { get; set; } = new();

        [BindProperty] public int IdUsuario { get; set; }
        [BindProperty] public int IdRol { get; set; }
        [BindProperty] public string Nombre { get; set; } = string.Empty;
        [BindProperty] public string Correo { get; set; } = string.Empty;
        [BindProperty] public string Contrasena { get; set; } = string.Empty;
        [BindProperty] public string Estado { get; set; } = "A";

        [TempData] public string? Mensaje { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            await CargarRolesAsync();

            var conn = _context.Database.GetDbConnection();
            if (conn.State != System.Data.ConnectionState.Open) await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT IdUsuario, IdRol, Nombre, Correo, Contrasena, Estado FROM Usuarios WHERE IdUsuario = @id";
            cmd.Parameters.Add(new SqlParameter("@id", id));

            using var reader = await cmd.ExecuteReaderAsync();
            if (!await reader.ReadAsync()) return NotFound();

            IdUsuario = reader.GetInt32(0);
            IdRol = reader.GetInt32(1);
            Nombre = reader.GetString(2);
            Correo = reader.GetString(3);
            Contrasena = reader.GetString(4);
            Estado = reader.GetString(5).Trim();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.ProUsuariosActualizar @IdUsuario, @IdRol, @Nombre, @Correo, @Contrasena, @Estado",
                    new SqlParameter("@IdUsuario", IdUsuario),
                    new SqlParameter("@IdRol", IdRol),
                    new SqlParameter("@Nombre", Nombre),
                    new SqlParameter("@Correo", Correo),
                    new SqlParameter("@Contrasena", Contrasena),
                    new SqlParameter("@Estado", Estado)
                );

                Mensaje = $"Datos de {Nombre} actualizados correctamente.";
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
