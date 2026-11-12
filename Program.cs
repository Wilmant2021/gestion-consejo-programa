using Plataforma.Modules.Profile.Infrastructure;
using Modules.Profile.Application.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<PerfilDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 1. OBLIGATORIO: Necesario para que el servicio local lea las cabeceras HTTP (X-Simulated-User-Id)
builder.Services.AddHttpContextAccessor();
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 1. CAMBIO AQUÍ: Usar AddSwaggerGen en lugar de AddOpenApi
builder.Services.AddSwaggerGen();

// 2. OBLIGATORIO: Registrar la interfaz de identidad con su implementación de desarrollo local
builder.Services.AddScoped<ICurrentUserService, LocalDevCurrentUserService>();

builder.Services.AddScoped<EstudianteService>();
builder.Services.AddScoped<CoordinadorService>();
builder.Services.AddScoped<AutentificacionService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<ProgramaService>(); // <--- Nuevo
builder.Services.AddScoped<RolService>();

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

// Opcional: Puedes quitar el endpoint de weatherforecast si ya no lo necesitas, 
// o dejarlo. Asegurar que mapee los controladores con app.MapControllers();
app.MapControllers();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
