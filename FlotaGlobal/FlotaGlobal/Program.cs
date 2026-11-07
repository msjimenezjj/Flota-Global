using FlotaGlobal.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. CONFIGURACIÓN DE SERVICIOS (Antes de Build) 
builder.Services.AddRazorPages();

// Configuración de la Base de Datos (Entity Framework)
builder.Services.AddDbContext<FlotaDbContext>(o =>
    o.UseSqlServer(builder.Configuration.GetConnectionString("Flota")));

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

app.UseAuthorization();

app.MapRazorPages();

app.Run();


