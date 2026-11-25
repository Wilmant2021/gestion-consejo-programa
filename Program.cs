using Microsoft.EntityFrameworkCore;
using Modules.Profile.Application.Services;
using Modules.Requests.Application.Services;
using Plataforma.Modules.Profile.Infrastructure;
using Plataforma.Modules.Requests.Infrastructure; // Ajusta si tu namespace de Infrastructure es distinto

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext del módulo Profile
builder.Services.AddDbContext<PerfilDbContext>(options =>
    options.UseSqlServer(connectionString));

// DbContext del módulo Requests (con tabla de migraciones separada en el esquema 'requests')
builder.Services.AddDbContext<RequestsDbContext>(options =>
    options.UseSqlServer(connectionString, sqlOptions =>
        sqlOptions.MigrationsHistoryTable("__EFMigrationsHistory_Requests", "requests")));

// 1. OBLIGATORIO: Necesario para que el servicio local lea las cabeceras HTTP (X-Simulated-User-Id)
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. CAMBIO AQUÍ: Usar AddSwaggerGen en lugar de AddOpenApi
builder.Services.AddSwaggerGen();

// 2. OBLIGATORIO: Registrar la interfaz de identidad con su implementación de desarrollo local
builder.Services.AddScoped<ICurrentUserService, LocalDevCurrentUserService>();

// Servicios del módulo Profile
builder.Services.AddScoped<EstudianteService>();
builder.Services.AddScoped<CoordinadorService>();
builder.Services.AddScoped<AutentificacionService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProgramaService>();
builder.Services.AddScoped<RolService>();

// Servicios del módulo Requests
builder.Services.AddScoped<TipoSolicitudService>();
builder.Services.AddScoped<SubTipoSolicitudService>();

// Añade esto en tu Program.cs antes de builder.Build():
builder.Services.AddAuthentication("Identity.Application")
    .AddCookie("Identity.Application");

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // 2. CAMBIO AQUÍ: Habilitar Swagger y SwaggerUI
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// --- MIDDLEWARE DE SIMULACIÓN LOCAL PARA DESARROLLO ---
app.Use(async (context, next) =>
{
    if (context.Request.Headers.TryGetValue("X-Simulated-User-Id", out var userIdHeader))
    {
        // Creamos las credenciales simuladas con el rol de Administrador para pruebas
        var claims = new[]
        {
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.NameIdentifier, userIdHeader.ToString()),
            new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Role, "Administrador")
        };
        var identity = new System.Security.Claims.ClaimsIdentity(claims, "LocalDevSimulation");
        context.User = new System.Security.Claims.ClaimsPrincipal(identity);
    }
    await next();
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();