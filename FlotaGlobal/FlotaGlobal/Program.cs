using FlotaGlobal.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS Y PROTECCIÓN DE PÁGINAS
builder.Services.AddRazorPages(options =>
{
    // Exige iniciar sesión en todas las páginas por defecto
    options.Conventions.AuthorizeFolder("/");

    // Permite entrar sin sesión únicamente a estas pantallas públicas
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Privacy");
    options.Conventions.AllowAnonymousToPage("/Login");
    options.Conventions.AllowAnonymousToPage("/Logout");       
    options.Conventions.AllowAnonymousToPage("/AccessDenied");
    options.Conventions.AllowAnonymousToPage("/ForgotPassword");
});

// Configuración de la Base de Datos
builder.Services.AddDbContext<FlotaDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Flota")));

// Configuración de Cookies de Autenticación
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(options =>
{
    options.LoginPath = "/Login";
    options.AccessDeniedPath = "/AccessDenied"; // Pantalla cuando un rol no tiene permiso
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();

