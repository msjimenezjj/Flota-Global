using FlotaGlobal.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    [Authorize(Roles = "1")]
    public class UsuariosModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public UsuariosModel(FlotaDbContext context)
        {
            _context = context;
        }

        public class UsuarioViewModel
        {
            public int IdUsuario { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public string Correo { get; set; } = string.Empty;
            public string NombreRol { get; set; } = string.Empty;
            public string Estado { get; set; } = string.Empty;
        }

        public List<UsuarioViewModel> ListaUsuarios { get; set; } = new();

        [TempData] public string? Mensaje { get; set; }

        public async Task OnGetAsync()
        {
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();

            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    u.IdUsuario,
                    u.Nombre,
                    u.Correo,
                    ISNULL(r.Nombre, 'Sin Rol') AS NombreRol,
                    ISNULL(u.Estado, 'A') AS Estado
                FROM Usuarios u
                LEFT JOIN Roles r ON u.IdRol = r.IdRol
                ORDER BY u.IdUsuario ASC";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ListaUsuarios.Add(new UsuarioViewModel
                {
                    IdUsuario = reader.GetInt32(0),
                    Nombre = reader.GetString(1),
                    Correo = reader.GetString(2),
                    NombreRol = reader.GetString(3),
                    Estado = reader.GetString(4).Trim()
                });
            }
        }

        // Inactiva o elimina lógicamente al usuario usando tu SP ProUsuariosEliminar
        public async Task<IActionResult> OnPostInactivarAsync(int id)
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.ProUsuariosEliminar @IdUsuario",
                    new SqlParameter("@IdUsuario", id)
                );
                Mensaje = "El estado del usuario ha sido actualizado correctamente.";
            }
            catch (SqlException ex)
            {
                Mensaje = $"Error: {ex.Message}";
            }

            return RedirectToPage("/Usuarios");
        }
    }
}
