using FlotaGlobal.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace FlotaGlobal.Pages
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public ForgotPasswordModel(FlotaDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public string Correo { get; set; } = string.Empty;

        public string? MensajeExito { get; set; }
        public string? MensajeError { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (string.IsNullOrEmpty(Correo))
            {
                MensajeError = "Por favor, ingrese su correo electrónico.";
                return Page();
            }

            // Buscamos el usuario en tu tabla real 'Usuarios'
            var usuario = await _context.Set<FlotaGlobal.Models.Usuarios>()
                .FirstOrDefaultAsync(u => u.Correo == Correo);

            if (usuario == null)
            {
                MensajeError = "El correo electrónico no se encuentra registrado en el sistema.";
                return Page();
            }

            // Simulación académica de recuperación (Mostrando la clave directamente en pantalla)
            MensajeExito = $"Validación exitosa. Su contraseña registrada es: {usuario.Contrasena}";
            return Page();
        }
    }
}
