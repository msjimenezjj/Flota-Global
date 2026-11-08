using System.Security.Claims;
using FlotaGlobal.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class LoginModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public LoginModel(FlotaDbContext context)
        {
            _context = context;
        }

        // Estas propiedades son obligatorias para que el HTML compile correctamente
        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        [BindProperty]
        public string Contrasena { get; set; } = string.Empty;

        [BindProperty]
        public string? ErrorMensaje { get; set; }

        public void OnGet()
        {
            ErrorMensaje = string.Empty;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Correo) || string.IsNullOrEmpty(Contrasena))
            {
                ErrorMensaje = "Por favor, complete todos los campos.";
                return Page();
            }

            // Buscamos el usuario en base a tu estructura real de la tabla Usuarios
            var usuario = await _context.Set<FlotaGlobal.Models.Usuarios>()
                .FirstOrDefaultAsync(u => u.Correo == Correo);

            // Verificación directa de las credenciales
            if (usuario == null || usuario.Contrasena != Contrasena)
            {
                ErrorMensaje = "El correo electrónico o la contraseña son incorrectos.";
                return Page();
            }

            // Verificación del Estado (asumiendo que 'I' o '0' significa Inactivo)
            if (usuario.Estado?.ToString().ToUpper().Trim() == "I" || usuario.Estado?.ToString() == "0")
            {
                ErrorMensaje = "Este usuario se encuentra inactivo. Consulte con soporte técnico.";
                return Page();
            }

            // Mapeamos los Claims para la sesión de ASP.NET Core
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Nombre),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Role, usuario.IdRol.ToString())
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            // Generamos la Cookie de acceso segura
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

            // Redirigimos al listado principal de vehículos autorizados
            return RedirectToPage("/Vehiculos");
        }
    }
}
