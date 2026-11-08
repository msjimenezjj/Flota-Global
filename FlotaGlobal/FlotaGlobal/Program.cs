using FlotaGlobal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS (Antes de Build) 
builder.Services.AddRazorPages();

// Configuración de la Base de Datos (Entity Framework)
builder.Services.AddDbContext<FlotaDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Flota")));

// ?? CORRECCIÓN AQUÍ: Le decimos a ASP.NET Core que el esquema de login por defecto SON las Cookies
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login"; // Si intentan entrar a algo protegido, los manda para acá
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20); // Duración del acceso
});

// 2. CONSTRUCCIÓN DE LA APLICACIÓN
var app = builder.Build();

// 3. CONFIGURACIÓN DEL PIPELINE HTTP (Middlewares - Después de Build) 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// El orden estricto de middlewares para la seguridad
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();


