using FlotaGlobal.Data;
using FlotaGlobal.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace FlotaGlobal.Pages
{
    public class ProgramacionModel : PageModel
    {
        private readonly FlotaDbContext _context;

        public ProgramacionModel(FlotaDbContext context)
        {
            _context = context;
        }

        // Estructura limpia para mostrar los datos cruzados en la tabla izquierda
        public class TurnoProgramadoViewModel
        {
            public int IdAsignacion { get; set; }
            public string Horario { get; set; } = string.Empty;
            public string Ruta { get; set; } = string.Empty;
            public string Vehiculo { get; set; } = string.Empty;
            public string Conductor { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public string Estado { get; set; } = string.Empty;
        }

        public List<TurnoProgramadoViewModel> TurnosProgramados { get; set; } = new();

        // Listas para los desplegables del formulario derecho
        public List<Rutas> ListaRutas { get; set; } = new();
        public List<Vehiculos> ListaVehiculos { get; set; } = new();
        public List<Conductores> ListaConductores { get; set; } = new();

        // Propiedades enlazadas al formulario "Crear Turno de Despacho"
        [BindProperty] public int IdRuta { get; set; }
        [BindProperty] public int IdVehiculo { get; set; }
        [BindProperty] public int IdConductor { get; set; }
        [BindProperty] public DateTime Fecha { get; set; } = DateTime.Today;
        [BindProperty] public TimeSpan HoraInicio { get; set; } = new TimeSpan(8, 0, 0);
        [BindProperty] public TimeSpan HoraFin { get; set; } = new TimeSpan(12, 0, 0);

        [TempData] public string? MensajeExito { get; set; }
        public string? ErrorAsignacion { get; set; }

        public async Task OnGetAsync()
        {
            await CargarDatosAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validar permisos en el backend: solo Rol 1 y Rol 2 pueden guardar turnos
            if (!User.IsInRole("1") && !User.IsInRole("2"))
            {
                return Forbid();
            }

            try
            {
                var paramIdOut = new SqlParameter("@IdAsignacion", SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

                await _context.Database.ExecuteSqlRawAsync(
                    "EXEC dbo.ProAsignacionesInsertar @IdAsignacion OUTPUT, @IdVehiculo, @IdConductor, @IdRuta, @Fecha, @HoraInicio, @HoraFin",
                    paramIdOut,
                    new SqlParameter("@IdVehiculo", IdVehiculo),
                    new SqlParameter("@IdConductor", IdConductor),
                    new SqlParameter("@IdRuta", IdRuta),
                    new SqlParameter("@Fecha", Fecha.Date),
                    new SqlParameter("@HoraInicio", HoraInicio),
                    new SqlParameter("@HoraFin", HoraFin)
                );

                MensajeExito = "Turno de despacho programado y asignado correctamente.";
                return RedirectToPage("/Programacion");
            }
            catch (SqlException ex)
            {
                // Captura el THROW del procedimiento (ej. Vehículo en mantenimiento o ocupado)
                ErrorAsignacion = ex.Message;
                await CargarDatosAsync();
                return Page();
            }
        }

        private async Task CargarDatosAsync()
        {
            // 1. Cargar los desplegables
            ListaRutas = await _context.Set<Rutas>().Where(r => r.Estado == "A").ToListAsync();
            ListaVehiculos = await _context.Vehiculos.ToListAsync();
            ListaConductores = await _context.Set<Conductores>().Where(c => c.Estado == "A").ToListAsync();

            // 2. Consultar los turnos asignados cruzando las tablas con SQL directo para evitar problemas de navegación
            var conn = _context.Database.GetDbConnection();
            await conn.OpenAsync();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT 
                    a.IdAsignacion,
                    ISNULL(CONVERT(VARCHAR(5), t.HoraInicio, 108), '--:--') + ' - ' + ISNULL(CONVERT(VARCHAR(5), t.HoraFin, 108), '--:--') AS Horario,
                    r.Origen + ' - ' + r.Destino AS Ruta,
                    v.Placa AS Vehiculo,
                    c.PrimerNombre + ' ' + c.PrimerApellido AS Conductor,
                    a.FechaAsignacion AS Fecha,
                    a.Estado
                FROM Asignaciones a
                INNER JOIN Rutas r ON a.IdRuta = r.IdRuta
                INNER JOIN Vehiculos v ON a.IdVehiculo = v.IdVehiculo
                INNER JOIN Conductores c ON a.IdConductor = c.IdConductor
                LEFT JOIN Turnos t ON a.IdTurno = t.IdTurno
                ORDER BY a.FechaAsignacion DESC, t.HoraInicio ASC";

            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                TurnosProgramados.Add(new TurnoProgramadoViewModel
                {
                    IdAsignacion = reader.GetInt32(0),
                    Horario = reader.GetString(1),
                    Ruta = reader.GetString(2),
                    Vehiculo = reader.GetString(3),
                    Conductor = reader.GetString(4),
                    Fecha = reader.GetDateTime(5),
                    Estado = reader.IsDBNull(6) ? "A" : reader.GetString(6)
                });
            }
        }
    }
}
